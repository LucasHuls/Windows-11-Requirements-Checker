﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace Windows_11_Compatibility_Checker_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    internal struct HardRequirements
    {
        public static bool cpu;
        public static bool ram;
        public static bool cpuArchitecture;
        public static bool storage;
        public static bool biosMode;
        public static bool secureBoot;
        public static bool tpm;
    }

    internal struct SoftRequirements
    {
        public static bool cpu;
        public static bool ram;
        public static bool gpu;
        public static bool cpuArchitecture;
        public static bool storage;
        public static bool biosMode;
        public static bool secureBoot;
        public static bool tpm;
        public static bool screenResolution;
    }

    public partial class MainWindow : Window
    {
        public static int numberOfCores;
        //Welcome to this utter mess, aka the Windows 11 Compatibility Checker source code. I've clearly labelled
        //the code of each check with comments so that I don't mess you or myself up when I debug.
        //I've also left 3 lines for each check. Use Control + F to find your way if you get lost.
        //Have a nice day!

        public MainWindow()
        {
            InitializeComponent();
            Main2();
        }

        private async void Main2()
        {
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker");
            await Delay(100);
            Properties.Resources.WindowsCritical.Save(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png");
            Properties.Resources.WindowsWarning.Save(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png");
            Properties.Resources.WindowsSuccess.Save(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png");
            Properties.Resources.WindowsHelp.Save(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png");

            cpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            ramImage.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            gpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            cpuArchitectureStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            storageImage.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            secureBootStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            biosModeStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            tpmStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));
            screenResolutionStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsHelp.png"));

            greenTick.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
            warningIcon.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png"));
            errorIcon.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));

            //Check for Windows 11
            //No Segoe UI Variable in Windows 10, revert to Segoe UI
            RegistryKey checkwindowsbuild = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            if (Convert.ToInt32(checkwindowsbuild.GetValue("CurrentBuildNumber")) < 21390)
            {
                Title.FontWeight = FontWeights.Bold;
                Title.FontFamily = new FontFamily("Segoe UI");
                yourSpecsTitle.FontFamily = new FontFamily("Segoe UI");
                cpuName.FontFamily = new FontFamily("Segoe UI");
                memoryAmount.FontFamily = new FontFamily("Segoe UI");
                gpuText.FontFamily = new FontFamily("Segoe UI");
                cpuArchitectureText.FontFamily = new FontFamily("Segoe UI");
                storageText.FontFamily = new FontFamily("Segoe UI");
                secureBootText.FontFamily = new FontFamily("Segoe UI");
                biosModeText.FontFamily = new FontFamily("Segoe UI");
                tpmText.FontFamily = new FontFamily("Segoe UI");
                screenResolutionText.FontFamily = new FontFamily("Segoe UI");
                greenTickText.FontFamily = new FontFamily("Segoe UI");
                warningIconText.FontFamily = new FontFamily("Segoe UI");
                errorIconText.FontFamily = new FontFamily("Segoe UI");
            }
            await Delay(100);

            try
            {
                //CPU
                foreach (ManagementObject mo in mos.Get())
                {
                    cpuName.Content = "CPU: " + (string)mo["Name"];
                    numberOfCores = Convert.ToInt32(mo["NumberOfCores"]);
                }
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker");
                //File.Create(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)+@"\Lucas Huls\Windows 11 Compatibility Checker\AMD.txt");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\CPUs.txt", Properties.Resources.CPUs);
                using (StreamReader sr = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\CPUs.txt"))
                {
                    string[] lines = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\CPUs.txt");
                    for (int x = 0; x < lines.Length; x++)
                    {
                        if (cpuName.Content.ToString().Contains(lines[x]))
                        {
                            cpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                            sr.Close();
                            SoftRequirements.cpu = true;
                            break;
                        }
                        else if (numberOfCores >= 2)
                        {
                            cpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png"));
                            HardRequirements.cpu = true;
                        }
                        else
                        {
                            cpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                            HardRequirements.cpu = false;
                        }
                    }
                }
                await Delay(200);
            }
            catch (Exception exception)
            {
                ExceptionHandler(exception);
            }

            //RAM
            foreach (ManagementObject mo in mos.Get())
            {
                ManagementScope oMs = new ManagementScope();
                ObjectQuery oQuery = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
                ManagementObjectSearcher oSearcher = new ManagementObjectSearcher(oMs, oQuery);
                ManagementObjectCollection oCollection = oSearcher.Get();

                long MemSize = 0;
                long mCap = 0;

                foreach (ManagementObject obj in oCollection)
                {
                    mCap = Convert.ToInt64(obj["Capacity"]);
                    MemSize += mCap;
                }
                var memory = MemSize / 1024 / 1024 / 1024;
                memoryAmount.Content = "RAM: " + memory + "GB";
                if (memory >= 4)
                {
                    ramImage.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                    HardRequirements.ram = true;
                    SoftRequirements.ram = true;
                }
                else
                {
                    ramImage.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                    HardRequirements.ram = false;
                }
            }
            await Delay(200);

            //GPU
            Process.Start("dxdiag", "/x " + Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\dxv.xml");
            while (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\dxv.xml"))
                Thread.Sleep(100);
            XmlDocument doc = new XmlDocument();
            doc.Load(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\dxv.xml");

            // GET LIST OF GFX ADAPTERS PRESENT
            XmlNodeList dxdevList = doc.SelectNodes("//DisplayDevice");

            // GET INFO ABOUT PRIMARY DISPLAY ADAPTER [INDEX 0]
            XmlNodeList dxvList = dxdevList[0].SelectNodes("//DirectXVersion");
            XmlNodeList dxmList = dxdevList[0].SelectNodes("//DriverModel");

            Version dxversion = new Version(dxvList[dxvList.Count - 1].InnerText.Split(' ')[1] + ".0");
            Version wddmVersion = null;

            // GET LIST OF SUPPORTED WDDM
            foreach (XmlNode wddmversionItem in dxmList)
            {
                Version currentWDDMversionItem = new Version(wddmversionItem.InnerText.Replace("WDDM", string.Empty).TrimStart());

                if (wddmVersion == null ||
                    wddmVersion < currentWDDMversionItem)
                {
                    wddmVersion = currentWDDMversionItem;
                }
            }

            if (dxversion < new Version("12.0"))
            {
                gpuText.Content = "GPU: DirectX " + dxversion + ", WDDM " + wddmVersion + ". Update to DirectX 12 or get a GPU with DX12.";
                gpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png"));
            }
            else
            {
                gpuText.Content = "GPU: DirectX " + dxversion + ", WDDM " + wddmVersion;

                if (wddmVersion < new Version("2.0"))
                {
                    gpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png"));
                }
                else
                {
                    gpuStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                    SoftRequirements.gpu = true;
                }
            }

            await Delay(200);

            //CPU Architecture
            bool is64 = System.Environment.Is64BitOperatingSystem;
            if (is64 == true)
            {
                cpuArchitectureText.Content = "CPU Architecture: 64-bit";
                cpuArchitectureStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                HardRequirements.cpuArchitecture = true;
                SoftRequirements.cpuArchitecture = true;
            }
            else
            {
                cpuArchitectureText.Content = "CPU Architecture: 32-bit";
                cpuArchitectureStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                HardRequirements.cpuArchitecture = false;
            }
            await Delay(200);

            //Storage
            DriveInfo mainDrive = new DriveInfo(System.IO.Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)));
            var totalsize = mainDrive.TotalSize / 1024 / 1024 / 1024;
            storageText.Content = "Storage: " + totalsize + "GB";
            if (totalsize >= 64)
            {
                storageImage.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                HardRequirements.storage = true;
                SoftRequirements.storage = true;
            }
            else
            {
                storageImage.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                HardRequirements.storage = false;
            }
            await Delay(200);

            //BIOS Mode
            Process process2 = new Process();
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.StartInfo.FileName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            process2.StartInfo.Arguments = "bcdedit";
            process2.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process2.StartInfo.CreateNoWindow = true;
            process2.StartInfo.Verb = "runas";
            process2.Start();
            string s2 = process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();

            using (StreamWriter outfile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\BIOSMode.txt"))
            {
                outfile.Write(s2);
            }
            using (StreamReader sr = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\BIOSMode.txt"))
            {
                string[] lines = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\BIOSMode.txt");
                for (var i = 0; i < lines.Length; i++)
                {
                    if (lines[i].ToLower().Contains(@"path                    \windows\system32\winload.efi")/* || lines[20].Contains(@"path                    \WINDOWS\system32\winload.efi")*/)
                    {
                        biosModeStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                        biosModeText.Content = "BIOS Mode: UEFI";
                        SoftRequirements.biosMode = true;
                        HardRequirements.biosMode = true;
                        break;
                    }
                    else
                    {
                        biosModeStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                        biosModeText.Content = "BIOS Mode: Legacy";
                        HardRequirements.biosMode = false;
                    }
                }
            }

            //Secure Boot
            try
            {
                RegistryKey securebootstatuskey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\State");
                var securebootstatus = securebootstatuskey.GetValue("UEFISecureBootEnabled");
                if ((int)securebootstatus == 1)
                {
                    secureBootText.Content = "Secure Boot: Enabled";
                    secureBootStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                    SoftRequirements.secureBoot = true;
                    HardRequirements.secureBoot = true;
                }
                else if ((int)securebootstatus == 0)
                {
                    secureBootText.Content = "Secure Boot: Disabled. Enable Secure Boot in the BIOS.";
                    secureBootStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                    HardRequirements.secureBoot = false;
                }
            }
            catch
            {
                secureBootText.Content = "Secure Boot: Registry Entry not found. You may be using Legacy Boot.";
                secureBootStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                HardRequirements.secureBoot = false;
            }
            await Delay(200);

            //TPM
            Process wmicTPMVersionProcess = new Process();
            wmicTPMVersionProcess.StartInfo.UseShellExecute = false;
            wmicTPMVersionProcess.StartInfo.RedirectStandardOutput = true;
            wmicTPMVersionProcess.StartInfo.FileName = @"C:\Windows\System32\Wbem\wmic.exe";
            wmicTPMVersionProcess.StartInfo.Arguments = @"/namespace:\\root\CIMV2\Security\MicrosoftTpm path Win32_Tpm get /value";
            wmicTPMVersionProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            wmicTPMVersionProcess.StartInfo.CreateNoWindow = true;
            wmicTPMVersionProcess.StartInfo.Verb = "runas";
            wmicTPMVersionProcess.Start();
            string s = wmicTPMVersionProcess.StandardOutput.ReadToEnd();
            wmicTPMVersionProcess.WaitForExit();

            using (StreamWriter outfile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\TPMresult.txt"))
            {
                outfile.Write(s);
            }
            using (StreamReader sr = File.OpenText(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\TPMresult.txt"))
            {
                string[] resultLines = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\TPMresult.txt");
                sr.Close();

                bool tpmEnabled = false;
                bool tpmActivated = false;
                bool tpmOwned = false;
                Version tpmVersion = null;

                foreach (string resultLine in resultLines)
                {
                    if (resultLine == "IsEnabled_InitialValue=TRUE")
                    {
                        tpmEnabled = true;
                    }
                    else if (resultLine == "IsActivated_InitialValue=TRUE")
                    {
                        tpmActivated = true;
                    }
                    else if (resultLine == "IsOwned_InitialValue=TRUE")
                    {
                        tpmOwned = true;
                    }
                    else if (resultLine.Contains("SpecVersion="))
                    {
                        tpmVersion = new Version(resultLine.Replace("SpecVersion=", string.Empty).Split(',')[0].TrimStart().TrimEnd());
                    }
                }

                if (tpmEnabled)
                {
                    if (tpmVersion < new Version("2.0"))
                    {
                        tpmStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png"));
                        tpmText.Content = "TPM: Version " + tpmVersion;
                        HardRequirements.tpm = true;
                    }
                    else
                    {
                        if (tpmActivated && tpmOwned)
                        {
                            tpmStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                            tpmText.Content = "TPM: Version " + tpmVersion + ", Present and enabled";
                            SoftRequirements.tpm = true;
                            HardRequirements.tpm = true;
                        }
                        else
                        {
                            tpmStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                            tpmText.Content = "TPM: Version " + tpmVersion + ", Present but not enabled";
                            HardRequirements.tpm = false;
                        }
                    }
                }
                else
                {
                    tpmStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsCritical.png"));
                    tpmText.Content = "TPM: Not present";
                    HardRequirements.tpm = false;
                }
            }

            //Screen Resolution
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            screenResolutionText.Content = "Screen Resolution: " + screenWidth + "x" + screenHeight;
            if (screenWidth >= 1280 && screenHeight >= 720)
            {
                screenResolutionStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsSuccess.png"));
                SoftRequirements.screenResolution = true;
            }
            else
            {
                screenResolutionStatus.Source = new BitmapImage(new Uri(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\WindowsWarning.png"));
            }

            //Hard floor check
            if (HardRequirements.cpu == true && HardRequirements.ram == true && HardRequirements.cpuArchitecture == true && HardRequirements.storage == true && HardRequirements.biosMode == true && HardRequirements.secureBoot == true && HardRequirements.tpm == true)
            {
                if (SoftRequirements.cpu == true && SoftRequirements.ram == true && SoftRequirements.gpu == true && SoftRequirements.cpuArchitecture == true && SoftRequirements.storage == true && SoftRequirements.biosMode == true && SoftRequirements.secureBoot == true && SoftRequirements.tpm == true && SoftRequirements.screenResolution == true)
                {
                    return;
                }
            }
        }

        private async Task Delay(int howlong)
        {
            await Task.Delay(howlong);
        }

        public static void ExceptionHandler(Exception exception)
        {
            List<string> attachmentList = new List<string>();
            attachmentList.Add(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\dxv.xml");
            attachmentList.Add(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\BIOSMode.txt");
            attachmentList.Add(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Lucas Huls\Windows 11 Compatibility Checker\TPMresult.txt");
        }
    }
}