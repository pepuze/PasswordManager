using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Security.Policy;
using System.Reflection;
using System.IO;

namespace PasswordKeeper
{
    public partial class Form1 : Form
    {
        int selectedRow;
        static public string userName; //Èìÿ ïîëüçîâàòåëÿ

        public Form1(string _userName)
        {
            userName = _userName;
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_pass_info", "¹");
            dataGridView1.Columns.Add("site_name", "Ñàéò");
            dataGridView1.Columns.Add("pass_login", "Ëîãèí");
            dataGridView1.Columns.Add("password_str", "Ïàðîëü");
            dataGridView1.Columns.Add("user_name", "ÔÈÎ");
            dataGridView1.Columns.Add("phone", "Òåëåôîí");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 160;
            dataGridView1.Columns[2].Width = 300;
            dataGridView1.Columns[3].Width = 150;
        }

        static public void ReadSingleRow(DataGridView dataGrid, IDataRecord record) //Èíòåðôåéñ IDataRecord ïðåäîñòàâëÿåò äîñòóï ê çíà÷åíèÿì ñòîëáöîâ â êàæäîé ñòðîêå.
        {
            dataGrid.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4),
                record.GetString(5));
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

        static public SqlConnection get_connection_process()
        {
            SqlConnection db_connect_process = new SqlConnection("Data Source=" + SystemInformation.ComputerName +
                $@"\SQLEXPRESS;Initial Catalog={userName};Integrated Security=True");
            return db_connect_process;
        }

        static public SqlConnection get_connection_protocol()
        {
            SqlConnection db_connect_protocol = new SqlConnection("Data Source=" + SystemInformation.ComputerName +
                @"\SQLEXPRESS;Initial Catalog=protocol_db;Integrated Security=True");
            return db_connect_protocol;
        }

        static public void RefreshDataGrid(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            SqlConnection connect = get_connection_process();
            string query = $"select * from password_table";
            SqlCommand cmd = new SqlCommand(query, connect);
            openConnection(connect);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { ReadSingleRow(dataGrid, reader); }
            reader.Close();
        }

        private void Search(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            SqlConnection connect = get_connection_process();
            string stringQuery = $"select * from password_table where concat (site_name, pass_login) " + //concat - ñëîæåíèå ñòðîê ïðè âûáîðêå èç ÁÄ
                $"like '%" + textBox1.Text + "%'";
            SqlCommand cmd = new SqlCommand(stringQuery, connect);
            openConnection(connect);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { ReadSingleRow(dataGrid, reader); }
            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //dbUserCreate(userName); //Ñîçäàíèå ÁÄ äëÿ ïîëüçîâàòåëÿ

            CreateColumns();
            RefreshDataGrid(dataGridView1);
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];
                label2.Text = "ÔÈÎ: " + row.Cells[4].Value.ToString();
                label3.Text = "Òåëåôîí: " + row.Cells[5].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            SqlConnection connectProcess = get_connection_process();
            SqlConnection connectProtocol = get_connection_protocol();
            if (f2.ShowDialog() == DialogResult.OK)
            {
                openConnection(connectProcess);
                if (f2.f2_site != "" && f2.f2_userLogin != "" && f2.f2_userPass != "")
                {
                    var addQuery = $"insert into password_table (site_name, pass_login, password_str, user_name, phone) values " +
                    $"('{f2.f2_site}', '{f2.f2_userLogin}', '{f2.f2_userPass}', '{f2.f2_userName}', '{f2.f2_userPhone}')";
                    var command = new SqlCommand(addQuery, connectProcess);
                    command.ExecuteNonQuery();
                }
                closeConnection(connectProcess);
                DateTime now = DateTime.Now;
                RefreshDataGrid(dataGridView1);
                openConnection(connectProtocol);
                var addQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +
                    $"('{now.ToString()}', '{userName}', '{"Äîáàâëåíèå ïàðîëÿ"}')";
                var command_protocol = new SqlCommand(addQuery_Protocol, connectProtocol);
                command_protocol.ExecuteNonQuery();
                closeConnection(connectProtocol);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Âàì íàäî ñíà÷àëà äîáàâèòü çàïèñü äëÿ óäàëåíèÿ ïàðîëÿ!", "Âíèìàíèå");
                return;
            }
            else
            {
                if (MessageBox.Show("Âû äåéñòâèòåëüíî õîòèòå óäàëèòü çàïèñü?", "Âíèìàíèå", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
                SqlConnection connectProcess = get_connection_process();
                SqlConnection connectProtocol = get_connection_protocol();
                openConnection(connectProcess);
                int index = dataGridView1.CurrentCell.RowIndex;
                var idNum = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                var deleteQuery = $"delete from password_table where id_pass_info = {idNum}";

                var command = new SqlCommand(deleteQuery, connectProcess);
                command.ExecuteNonQuery();
                closeConnection(connectProcess);
                DateTime now = DateTime.Now;
                RefreshDataGrid(dataGridView1);
                openConnection(connectProtocol);
                var deleteQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +
                    $"('{now.ToString()}', '{userName}', '{"Óäàëåíèå ïàðîëÿ"}')";
                var command_protocol = new SqlCommand(deleteQuery_Protocol, connectProtocol);
                command_protocol.ExecuteNonQuery();
                closeConnection(connectProtocol);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Âàì íàäî ñíà÷àëà äîáàâèòü çàïèñü äëÿ ðåäàêòèðîâàíèÿ ïàðîëÿ è/èëè äîïîëíèòåëüíîé èíôîðìàöèè!", "Âíèìàíèå");
                return;
            }
            else
            {
                int index = dataGridView1.CurrentCell.RowIndex;
                var idNum = dataGridView1.Rows[index].Cells[0].Value.ToString();
                Form2 f2 = new Form2();
                f2.f2_site = dataGridView1.Rows[index].Cells[1].Value.ToString();
                f2.f2_userLogin = dataGridView1.Rows[index].Cells[2].Value.ToString();
                f2.f2_userPass = dataGridView1.Rows[index].Cells[3].Value.ToString();
                f2.f2_userName = dataGridView1.Rows[index].Cells[4].Value.ToString();
                f2.f2_userPhone = dataGridView1.Rows[index].Cells[5].Value.ToString();
                f2.SetTextBox();
                SqlConnection connectProcess = get_connection_process();
                SqlConnection connectProtocol = get_connection_protocol();
                if (f2.ShowDialog() == DialogResult.OK)
                {
                    openConnection(connectProcess);
                    if (f2.f2_site != "" && f2.f2_userLogin != "" && f2.f2_userPass != "")
                    {
                        var updateQuery = $"update password_table set site_name = '{f2.f2_site}', pass_login = '{f2.f2_userLogin}', password_str = '{f2.f2_userPass}', " +
                            $"user_name = '{f2.f2_userName}', phone = '{f2.f2_userPhone}' where id_pass_info = '{idNum}'";
                        var command = new SqlCommand(updateQuery, connectProcess);
                        command.ExecuteNonQuery();
                    }
                    closeConnection(connectProcess);
                    DateTime now = DateTime.Now;
                    RefreshDataGrid(dataGridView1);
                    openConnection(connectProtocol);
                    var updateQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +
                        $"('{now.ToString()}', '{userName}', '{"Èçìåíåíèå ïàðîëÿ è/èëè äîï. èíôîðìàöèè"}')";
                    var command_protocol = new SqlCommand(updateQuery_Protocol, connectProtocol);
                    command_protocol.ExecuteNonQuery();
                    closeConnection(connectProtocol);
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (checkBox1.Checked == false)
                if (e.ColumnIndex == 3)
                    if (e.Value != null)
                        e.Value = new string('*', 10);
            if (e.ColumnIndex == 1)
                if (e.Value != null)
                {
                    Uri site_uri = new Uri(e.Value.ToString());
                    string site_host = site_uri.Host;
                    e.Value = new string(site_host);
                }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        static public void dbUserCreate(string userName)
        {
            string str_db, protocol_db;
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
                MessageBox.Show("Áàçà äàííûõ ïðîòîêîëèðîâàíèÿ óæå ñóùåñòâóåò", "Âíèìàíèå");
                myConn_db.Close();
            }

            str_db = 
                $"CREATE DATABASE {userName} ON " +
                $"( NAME = N'{userName}_Data', FILENAME = N'{dataPath}\\{userName}_Data.mdf' , SIZE = 1024KB , MAXSIZE = 25MB, FILEGROWTH = 1024KB ) " +
                $"LOG ON " +
                $"( NAME = N'{userName}_Log', FILENAME = N'{logPath}\\{userName}_Log.mdf', SIZE = 1024KB, MAXSIZE = 8MB, FILEGROWTH = 1024KB ) ";

            SqlCommand myCommand1 = new SqlCommand(str_db, myConn_db);

            myConn_db.Open();
            myCommand1.ExecuteNonQuery();
            myConn_db.Close();
            
            string str_table;
            SqlConnection myConn_table = new SqlConnection("Data Source=" + SystemInformation.ComputerName + $@"\SQLEXPRESS;Initial Catalog={userName};Integrated Security=True");

            str_table = "CREATE TABLE password_table (id_pass_info INT PRIMARY KEY IDENTITY, site_name NVARCHAR(50), pass_login "
            + "NVARCHAR(100), password_str VARCHAR(50), user_name VARCHAR(120), phone VARCHAR(17));";

            SqlCommand myCommand2 = new SqlCommand(str_table, myConn_table);
            myConn_table.Open();
            myCommand2.ExecuteNonQuery();
            myConn_table.Close();
        }
    }
}
