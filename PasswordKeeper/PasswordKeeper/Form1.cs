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

namespace PasswordKeeper
{
    public partial class Form1 : Form
    {
        DB_Class dataBase = new DB_Class();
        int selectedRow;

        public Form1()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_pass_info", "№");
            dataGridView1.Columns.Add("site_name", "Сайт");
            dataGridView1.Columns.Add("pass_login", "Логин");
            dataGridView1.Columns.Add("password_str", "Пароль");
            dataGridView1.Columns.Add("user_name", "ФИО");
            dataGridView1.Columns.Add("phone", "Телефон");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 160;
            dataGridView1.Columns[2].Width = 300;
            dataGridView1.Columns[3].Width = 150;
        }

        private void ReadSingleRow(DataGridView dataGrid, IDataRecord record)
        {
            dataGrid.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4),
                record.GetString(5));
        }

        private void RefreshDataGrid(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            string query = $"select * from password_table";
            SqlCommand cmd = new SqlCommand(query, dataBase.get_connection_process());
            dataBase.openConnection_Process();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { ReadSingleRow(dataGrid, reader); }
            reader.Close();
        }

        private void Search(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            string stringQuery = $"select * from password_table where concat (site_name, pass_login) " +
                $"like '%" + textBox1.Text + "%'";
            SqlCommand cmd = new SqlCommand(stringQuery, dataBase.get_connection_process());
            dataBase.openConnection_Process();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { ReadSingleRow(dataGrid, reader); }
            reader.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                label2.Text = "ФИО: " + row.Cells[4].Value.ToString();
                label3.Text = "Телефон: " + row.Cells[5].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            if (f2.ShowDialog() == DialogResult.OK)
            {
                dataBase.openConnection_Process();
                if (f2.f2_site != "" && f2.f2_userLogin != "" && f2.f2_userPass != "")
                {
                    var addQuery = $"insert into password_table (site_name, pass_login, password_str, user_name, phone) values " +
                    $"('{f2.f2_site}', '{f2.f2_userLogin}', '{f2.f2_userPass}', '{f2.f2_userName}', '{f2.f2_userPhone}')";
                    var command = new SqlCommand(addQuery, dataBase.get_connection_process());
                    command.ExecuteNonQuery();
                }
                dataBase.closeConnection_Process();
                DateTime now = DateTime.Now;
                RefreshDataGrid(dataGridView1);
                dataBase.openConnection_Protocol();
                var addQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +
                    $"('{now.ToString()}', '{"user"}', '{"Добавление пароля"}')";
                var command_protocol = new SqlCommand(addQuery_Protocol, dataBase.get_connection_protocol());
                command_protocol.ExecuteNonQuery();
                dataBase.closeConnection_Protocol();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Search(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить запись?", "Внимание", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) == DialogResult.No)
                return;
            dataBase.openConnection_Process();
            int index = dataGridView1.CurrentCell.RowIndex;
            var idNum = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            var deleteQuery = $"delete from password_table where id_pass_info = {idNum}";

            var command = new SqlCommand(deleteQuery, dataBase.get_connection_process());
            command.ExecuteNonQuery();
            dataBase.closeConnection_Process();
            DateTime now = DateTime.Now;
            RefreshDataGrid(dataGridView1);
            dataBase.openConnection_Protocol();
            var deleteQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +
                $"('{now.ToString()}', '{"user"}', '{"Удаление пароля"}')";
            var command_protocol = new SqlCommand(deleteQuery_Protocol, dataBase.get_connection_protocol());
            command_protocol.ExecuteNonQuery();
            dataBase.closeConnection_Protocol();
        }

        private void button2_Click(object sender, EventArgs e)
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
            if (f2.ShowDialog() == DialogResult.OK)
            {
                dataBase.openConnection_Process();
                if (f2.f2_site != "" && f2.f2_userLogin != "" && f2.f2_userPass != "")
                {
                    var updateQuery = $"update password_table set site_name = '{f2.f2_site}', pass_login = '{f2.f2_userLogin}', password_str = '{f2.f2_userPass}', " +
                        $"user_name = '{f2.f2_userName}', phone = '{f2.f2_userPhone}' where id_pass_info = '{idNum}'";
                    var command = new SqlCommand(updateQuery, dataBase.get_connection_process());
                    command.ExecuteNonQuery();
                }
                dataBase.closeConnection_Process();
                DateTime now = DateTime.Now;
                RefreshDataGrid(dataGridView1);
                dataBase.openConnection_Protocol();
                var updateQuery_Protocol = $"insert into operations_info (dateTimeInfo, userInfo, operation) values " +
                    $"('{now.ToString()}', '{"user"}', '{"Изменение пароля и/или доп. информации"}')";
                var command_protocol = new SqlCommand(updateQuery_Protocol, dataBase.get_connection_protocol());
                command_protocol.ExecuteNonQuery();
                dataBase.closeConnection_Protocol();
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
    }
}
