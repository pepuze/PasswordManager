using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordKeeper
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }

        private void CreateColumnsLog()
        {
            dataGridView1.Columns.Add("idProtocol", "ID операции");
            dataGridView1.Columns.Add("dateTimeInfo", "Дата и время");
            dataGridView1.Columns.Add("userInfo", "Пользователь");
            dataGridView1.Columns.Add("operation", "Операция");
            dataGridView1.Columns[0].Width = 90;
            dataGridView1.Columns[1].Width = 140;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[3].Width = 305;
        }

        static public void ReadSingleRowLog(DataGridView dataGrid, IDataRecord record) //Интерфейс IDataRecord предоставляет доступ к значениям столбцов в каждой строке.
        {
            dataGrid.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2),
                record.GetString(3));
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

        static public void RefreshDataGridLog(DataGridView dataGrid)
        {
            dataGrid.Rows.Clear();
            SqlConnection connect = get_connection_protocol();
            string query = $"select * from operations_info";
            SqlCommand cmd = new SqlCommand(query, connect);
            openConnection(connect);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { ReadSingleRowLog(dataGrid, reader); }
            reader.Close();
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            CreateColumnsLog();
            RefreshDataGridLog(dataGridView1);
        }
    }
}
