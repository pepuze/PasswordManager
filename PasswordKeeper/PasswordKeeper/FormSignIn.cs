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
using System.Security.Cryptography;
using System.Diagnostics;
using Microsoft.VisualBasic.Logging;
using System.Reflection.Metadata;

namespace PasswordKeeper
{
    public partial class FormSignIn : Form
    {
        public static readonly string usersDocPath = "users.xml";
        Dictionary<string, UsbDriveInfo> usbDrives = new Dictionary<string, UsbDriveInfo>();
        Dictionary<string, UserData> users = new Dictionary<string, UserData>();
        XmlDocument usersDoc = new XmlDocument();
        TimeoutTimer? timer = null;
        int passTryCount = 3;
        int timeOutTimeMs = 60000;


        public FormSignIn()
        {
            if (!File.Exists(usersDocPath))
            {
                using (XmlWriter writer = XmlWriter.Create(usersDocPath))
                {
                    writer.WriteStartDocument(false);
                    writer.WriteStartElement("Users");
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                }
            }
            try
            {
                usersDoc.Load(usersDocPath);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            populateDrives();

            InitializeComponent();
            populateUsers();
        }

        private void populateUsers()
        {
            XmlElement? root = usersDoc.DocumentElement;
            if (root != null && root.Name == "Users")
            {
                foreach (XmlElement node in root)
                {
                    if(node.Name == "timeout_left")
                    {
                        beginTimeOut(Convert.ToInt32(node.InnerText));
                        root.RemoveChild(node);
                        usersDoc.Save(FormSignIn.usersDocPath);
                    }
                    var user = UserData.createUserFromXML(node);
                    if (user != null) users.Add(user.Login, user);
                }
            }
        }

        private void populateDrives()
        {
            var drivesRemovable = DriveInfo.GetDrives().
                Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable).
                ToList();
            if (drivesRemovable.Count < 1) return;

            foreach (ManagementObject drive in new ManagementObjectSearcher(
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
                if (logicalDisk == null) continue;

                var volumes = new ManagementObjectSearcher(String.Format(
                    "select VolumeName from Win32_LogicalDisk " +
                     "where Name='{0}'",
                    logicalDisk["Name"])).Get();
                ManagementObject? volume = null;
                foreach (ManagementObject v in volumes) volume = v;
                if (volume == null) continue;

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
                    usbDrives.Add(pnpID, new UsbDriveInfo(id, pnpID, vol, log));
                    drivesRemovable.RemoveAt(foundDriveIndex);
                }
            }
        }

        private void bCreateAccount_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCreateAccount fca = new FormCreateAccount(users);
            fca.StartPosition = FormStartPosition.CenterScreen;
            if (fca.ShowDialog() == DialogResult.OK)
            {
                users.Add(fca.user.Login, fca.user);
                fca.user.saveToXML(usersDoc);
                Form1.dbUserCreate(fca.user.Login);
            }
            this.Show();
        }

        private void bLogIn_Click(object sender, EventArgs e)
        {
            string login = tb_Login.Text;
            string password = tb_Password.Text;
            bool loggedIn = false;
            if (users.ContainsKey(login))
            {
                var desiredHash = users[login].PasswordHash;
                var hash = getStringHash(password);
                if (areEqualHashes(hash, desiredHash))
                    loggedIn = true;
            }

            if (loggedIn)
            {
                Form1 form = new Form1(login);
                this.Hide();
                form.ShowDialog();
                tb_Login.Text = "";
                tb_Password.Text = "";
                this.Show();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                --passTryCount;
                if (passTryCount <= 0)
                    beginTimeOut(timeOutTimeMs);
            }
        }

        private void beginTimeOut(int timeMs)
        {
            timer = new TimeoutTimer(timeMs);
            bLogIn.Enabled = false;
            bCreateAccount.Enabled = false;
            timer.Value.timerMain.Elapsed += endTimeOut;
            lTimeOutTIme.Visible = true;
            lTimeOutTIme.Text = "Превышено количество попыток входа,\n\rтайм-аут...";
            timer.Value.start();
        }

        private void endTimeOut(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timer = null;
            lTimeOutTIme.Invoke(() => lTimeOutTIme.Visible = false);

            bLogIn.Invoke(() => bLogIn.Enabled = true);
            bCreateAccount.Invoke(() => bCreateAccount.Enabled = true);
            passTryCount = 3;
        }

        private void tlu_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {

        }

        public static byte[] getStringHash(string str)
        {
            using (HashAlgorithm hasher = SHA256.Create()) return hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
        }

        public static bool areEqualHashes(in byte[] hash1, in byte[] hash2)
        {
            if (hash1.Length != hash2.Length) return false;
            for (int i = 0; i < hash1.Length; ++i)
            {
                if (hash1[i] != hash2[i]) return false;
            }
            return true;
        }

        private void FormSignIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (timer != null) timer.Value.saveToXML(usersDoc);
        }
    }

    public struct TimeoutTimer
    {
        private static readonly string node_time = "timeout_left";

        public int _timeoutMs;
        public System.Timers.Timer timerMain;
        public Stopwatch stopwatch;
        public TimeoutTimer(int timeoutMs)
        {
            _timeoutMs = timeoutMs;
            timerMain = new System.Timers.Timer();
            timerMain.Interval = (double)_timeoutMs;
            stopwatch = new Stopwatch();
        }

        public void start()
        {
            timerMain.Start();
            stopwatch.Start();
        }

        public void stop()
        {
            timerMain.Stop();
            stopwatch.Stop();
        }

        public void saveToXML(XmlDocument document)
        {
            XmlElement timeNode = document.CreateElement(node_time);
            timeNode.InnerText = (_timeoutMs - stopwatch.ElapsedMilliseconds).ToString();

            var root = document.DocumentElement;
            root.AppendChild(timeNode);

            document.Save(FormSignIn.usersDocPath);
        }
    }
}
