using System;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.ServiceProcess;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PC_Manager
{
    class CommandHandler
    {
        public string GetInfo()
        {
            string spliterStr = "¥¥¥¥¥/¥¥¥¥¥";


            string message = null;

            //Версия ОС
            ComputerInfo CI = new ComputerInfo();
            message += CI.OSFullName + spliterStr;

            //разрядность ОС
            if (Environment.Is64BitOperatingSystem)
                message += "64-битная ОС" + spliterStr;
            else message += "32-битная ОС" + spliterStr;

            //Объём RAM
            ulong mem = ulong.Parse(CI.TotalPhysicalMemory.ToString());
            message += (mem / (1024 * 1024)).ToString() + " МБайт" + " ( ≈" + Convert.ToDouble(mem / (1024 * 1024 * 1000)).ToString() + " ГБайт" + " )" + spliterStr;

            //Тип RAM
            SMBIOSdata smbios = new SMBIOSdata();
            smbios.GetRawData();
            smbios.GetTables();

            for (int i = 0; i < smbios.p_oSMBIOStables.Count; i++)
            {
                SMBIOStable table = smbios.p_oSMBIOStables[i];
                message += smbios.ParseTable(table);
            }
            message += spliterStr;

            //Объём HDD
            long summary = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType.ToString() == "Fixed") summary += drive.TotalSize;
            }
            message += (summary / 1073741824).ToString() + " ГБайт" + spliterStr;

            //Процессор
            ManagementObjectSearcher query = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in query.Get())
            {
                message += (queryObj["Name"]) + spliterStr;
                break;
            }

            //Сведения о процессоре
            ManagementObject mo;
            mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
            ushort architecture = (ushort)mo["Architecture"];

            message += "Архитектура: ";

            switch (architecture)
            {
                case 0:
                    message += "32 Bit ";
                    break;
                case 9:
                    message += "64 Bit ";
                    break;
            }

            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_ComputerSystem").Get())
            {
                message += "Сокетов: " + item["NumberOfProcessors"];
            }

            int coreCount = 0;
            foreach (var item in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            message += " Ядра: " + coreCount;

            message += " Логических процессоров: " + Environment.ProcessorCount;
            message += spliterStr;


            //Видео
            query = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (ManagementObject queryObj in query.Get())
            {
                message += queryObj["Name"] + " | ";
            }
            message = message.Substring(0, message.Length - 2);
            message += spliterStr;

            //Материнка
            query = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject queryObj in query.Get())
            {
                message += queryObj["Manufacturer"] + " " + queryObj["Product"] + spliterStr;
                break;
            }

            //S/N
            query = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            foreach (ManagementObject queryObj in query.Get())
            {
                message += (queryObj["SerialNumber"]) + spliterStr;
            }

            //BIOS
            query = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
            foreach (ManagementObject queryObj in query.Get())
            {
                message += queryObj["Manufacturer"] + " " + queryObj["Version"] + spliterStr;
                break;
            }

            //Имя компьютера
            message += Environment.MachineName + spliterStr;

            //Имя пользователя
            message += Environment.UserName;

            return message;
        }

        public string GetProcess()
        {
            string message = null;
            foreach (Process process in Process.GetProcesses())
            {
                message += process.ProcessName;
                message += "/";
            }
            return message;
        }

        public string GetServices()
        {
            string message = null;

            ServiceController[] services = ServiceController.GetServices();
            for (int i = 0; i < services.Length; i++)
                if (services[i].Status == ServiceControllerStatus.Running)
                    message += services[i].DisplayName + "/";
            return message;
        }

        public string GetNetwork()
        {
            string message = null;

            foreach (NetworkInterface net in NetworkInterface.GetAllNetworkInterfaces())
                if (net.Name.Length > 0)
                    message += net.Name + "/";

            return message;
        }
    }
}
