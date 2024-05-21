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
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PasswordKeeper
{
    public partial class FormSignIn : Form
    {
        public static readonly string usersDocPath = "users.xml";
        Dictionary<string, UsbDriveInfo> usbDrives = new Dictionary<string, UsbDriveInfo>();
        Dictionary<string, UserData> users = new Dictionary<string, UserData>();
        XmlDocument usersDoc = new XmlDocument();
        int passTryCount = 3;
        System.Timers.Timer timerTimeOut;
        System.Timers.Timer timerLabelUpdater;

        static public void openConnection(SqlConnection connect)
        {
            if (connect.State == System.Data.ConnectionState.Closed)
                connect.Open();
        }

        static public void closeConnection(SqlConnection connect)
        {
            if (connect.State == System.Data.ConnectionState.Open)
                connect.Close();
        }

        static public SqlConnection get_connection_protocol()
        {
            SqlConnection db_connect_protocol = new SqlConnection("Data Source=" + SystemInformation.ComputerName +
                @"\SQLEXPRESS;Initial Catalog=protocol_db;Integrated Security=True");
            return db_connect_protocol;
        }

        static public void createProtocolDB()
        {
            string protocol_db;
            SqlConnection myConn_db = new SqlConnection("Server=" + SystemInformation.ComputerName + $@"\SQLEXPRESS;Integrated Security=True;database=master");
            string dataPath = Environment.CurrentDirectory + "\\Data";
            string logPath = Environment.CurrentDirectory + "\\Log";
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            try
            {
                protocol_db =
                $"CREATE DATABASE protocol_db ON " +
                $"( NAME = N'protocol_db_Data', FILENAME = N'{dataPath}\\protocol_db_Data.mdf' , SIZE = 1024KB , MAXSIZE = 25MB, FILEGROWTH = 1024KB ) " +
                $"LOG ON " +
                $"( NAME = N'protocol_db_Log', FILENAME = N'{logPath}\\protocol_db_Log.mdf', SIZE = 1024KB, MAXSIZE = 8MB, FILEGROWTH = 1024KB ) ";

                SqlCommand myCommandProtocolCreate1 = new SqlCommand(protocol_db, myConn_db);

                myConn_db.Open();
                myCommandProtocolCreate1.ExecuteNonQuery();
                myConn_db.Close();

                string str_table_protocol;
                SqlConnection myConn_table_protocol = new SqlConnection("Data Source=" + SystemInformation.ComputerName + $@"\SQLEXPRESS;Initial Catalog=protocol_db;Integrated Security=True");

                str_table_protocol = "CREATE TABLE operations_info (idProtocol INT PRIMARY KEY IDENTITY, dateTimeInfo NVARCHAR(100), userInfo "
                + "NVARCHAR(100), operation VARCHAR(100));";

                SqlCommand myCommandProtocolCreate2 = new SqlCommand(str_table_protocol, myConn_table_protocol);
                myConn_table_protocol.Open();
                myCommandProtocolCreate2.ExecuteNonQuery();
                myConn_table_protocol.Close();
            }
            catch
            {

                myConn_db.Close();
            }
        }

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
            populateUsers();


            InitializeComponent();
        }

        private void populateUsers()
        {
            XmlElement? root = usersDoc.DocumentElement;
            if (root != null && root.Name == "Users")
            {
                foreach (XmlElement node in root)
                {
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
            createProtocolDB();
            SqlConnection connectProtocol = get_connection_protocol();
            openConnection(connectProtocol);
            if (users.ContainsKey(login))
            {
                var desiredHash = users[login].PasswordHash;
                var hash = getStringHash(password);
                if (areEqualHashes(hash, desiredHash))
                    loggedIn = true;
            }

            if (loggedIn)
            {
                DateTime now = DateTime.Now;
                var loginSuccessQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +

                    $"('{now.ToString()}', '{login}', '{"Успешный вход в систему"}')";

                var command_protocol = new SqlCommand(loginSuccessQuery_Protocol, connectProtocol);
                command_protocol.ExecuteNonQuery();

                Form1 form = new Form1(login);
                this.Hide();
                form.ShowDialog();
                tb_Login.Text = "";
                tb_Password.Text = "";
                this.Show();
            }
            else
            {
                DateTime now = DateTime.Now;
                var loginFailureQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +

                    $"('{now.ToString()}', '{login}', '{"Неуспешный вход в систему"}')";

                var command_protocol = new SqlCommand(loginFailureQuery_Protocol, connectProtocol);
                command_protocol.ExecuteNonQuery();

                MessageBox.Show("Неправильный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                --passTryCount;
                if (passTryCount <= 0)
                    beginTimeOut();
            }
            closeConnection(connectProtocol);
        }

        private void beginTimeOut()
        {
            timerTimeOut = new System.Timers.Timer();
            timerTimeOut.Interval = 30000; //10 минут в мс
            bLogIn.Enabled = false;
            bCreateAccount.Enabled = false;
            timerTimeOut.Elapsed += endTimeOut;
            lTimeOutTIme.Text = "Превышено количество попыток входа,\n\rтайм-аут 10 минут.";
            timerTimeOut.Start();
        }

        private void endTimeOut(object? sender, System.Timers.ElapsedEventArgs e)
        {
            timerTimeOut.Stop();
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
            for(int i = 0; i < hash1.Length; ++i)
            {
                if (hash1[i] != hash2[i]) return false;
            }
            return true;
        }
    }
}
