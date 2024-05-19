using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Management;

namespace PasswordKeeper
{
    public partial class FormSignIn : Form
    {
        List<UsbDriveInfo> usbDrives = new List<UsbDriveInfo>();
        XmlDocument usersDoc;

        public FormSignIn()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            populateDrives();

            InitializeComponent();
        }

        private void populateDrives()
        {
            var drivesRemovable = DriveInfo.GetDrives().
                Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable).
                ToList();
            if (drivesRemovable.Count < 1) return;

            foreach(ManagementObject drive in new ManagementObjectSearcher(
                "select DeviceID, PNPDeviceID, Model from Win32_DiskDrive " +
                "where InterfaceType='USB'"
                ).
                Get())
            {
                var partitions = new ManagementObjectSearcher(String.Format(
                    "associators of {{Win32_DiskDrive.DeviceID='{0}'}} " +
                    "where AssocClass = Win32_DiskDriveToDiskPartition",
                drive["DeviceID"])).Get();

                ManagementObject? partition = null;
                foreach (ManagementObject p in partitions) partition = p;
                if (partition == null) continue;

                var logicalDisks = new ManagementObjectSearcher(String.Format(
                    "associators of {{Win32_DiskPartition.DeviceID='{0}'}} " +
                    "where AssocClass= Win32_LogicalDiskToPartition",
                partition["DeviceID"])).Get();

                ManagementObject? logicalDisk = null;
                foreach (ManagementObject ld in logicalDisks) logicalDisk = ld;
                if(logicalDisk == null) continue;

                var volumes = new ManagementObjectSearcher(String.Format(
                    "select VolumeName from Win32_LogicalDisk " +
                     "where Name='{0}'",
                    logicalDisk["Name"])).Get();
                ManagementObject? volume = null;
                foreach (ManagementObject v in volumes) volume = v;
                if(volume == null) continue;

                string log = logicalDisk["Name"].ToString();
                string id = drive["DeviceID"].ToString();
                string pnpID = drive["PNPDeviceID"].ToString();
                string vol = volume["VolumeName"].ToString();

                int foundDriveIndex = -1;
                for (int i = 0; i < drivesRemovable.Count; ++i)
                    if (drivesRemovable[i].Name.Contains(log))
                    {
                        foundDriveIndex = i;
                        break;
                    }

                if (foundDriveIndex > -1)
                {
                    usbDrives.Add(new UsbDriveInfo(id, pnpID, vol, log));
                    drivesRemovable.RemoveAt(foundDriveIndex);
                }
            }


        }

        private void bCreateAccount_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCreateAccount fca = new FormCreateAccount();
            fca.StartPosition = FormStartPosition.CenterScreen;
            fca.ShowDialog();
            this.Show();
        }
    }
}
