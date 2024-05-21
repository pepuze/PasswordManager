using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordKeeper
{
    public partial class FormCreateAccount : Form
    {
        public UserData user;
        Dictionary<string, UserData> _usersExisting;

        public FormCreateAccount(in Dictionary<string, UserData> usersExisting)
        {
            _usersExisting = usersExisting;
            InitializeComponent();
        }

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

        private void bCancel_Click(object sender, EventArgs e)
        {

        }

        private void bCreate_Click(object sender, EventArgs e)
        {
            string login = tb_Login.Text,
                   password = tb_Password.Text,
                   passwordRepeat = tb_PasswordRepeat.Text;

            if (password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_usersExisting.ContainsKey(login))
            {
                MessageBox.Show("Учетная запись с таким логином уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!checkIfValid(password))
            {
                MessageBox.Show("Введенный пароль не соответствует требованиям.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            byte[] hash = FormSignIn.getStringHash(password);
            user = new UserData(login, hash, "placeholder_id");

            createProtocolDB();
            SqlConnection connectProtocol = get_connection_protocol();
            openConnection(connectProtocol);
            DateTime now = DateTime.Now;
            var loginSuccessQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +

                $"('{now.ToString()}', '{login}', '{"Регистрация пользователя"}')";

            var command_protocol = new SqlCommand(loginSuccessQuery_Protocol, connectProtocol);
            command_protocol.ExecuteNonQuery();
            closeConnection(connectProtocol);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool checkIfValid(string password)
        {
            if (password.Length < 8) return false;
            bool lowerRegister = false;
            bool upperRegister = false;
            bool specialSymbols = false;
            bool hasDigits = false;

            foreach (char c in password)
            {
                if (!Char.IsLetterOrDigit(c)) specialSymbols = true;
                else if (Char.IsLetter(c))
                {
                    if (!Char.IsUpper(c)) lowerRegister = true;
                    else upperRegister = true;
                }
                else if (Char.IsDigit(c)) hasDigits = true;
            }

            return lowerRegister && upperRegister && specialSymbols && hasDigits;
        }

        private void tb_PasswordRepeat_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            int visibleTime = 5000;
            ToolTip tt = new ToolTip();
            tt.Show("Пароль должен содержать цифры,\r\n буквы разных регистров,\r\n специальные символы", tb, 120, 20, visibleTime);
        }
    }
}
