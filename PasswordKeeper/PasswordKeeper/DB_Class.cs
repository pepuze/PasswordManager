using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PasswordKeeper
{
    class DB_Class
    {
        SqlConnection db_connect_process = new SqlConnection("Data Source=" + SystemInformation.ComputerName + @"\SQLEXPRESS;Initial Catalog=passwords;Integrated Security=True");
        SqlConnection db_connect_protocol = new SqlConnection("Data Source=" + SystemInformation.ComputerName + @"\SQLEXPRESS;Initial Catalog=protocol_db;Integrated Security=True");
        public void openConnection_Process()
        {
            if (db_connect_process.State == System.Data.ConnectionState.Closed)
                db_connect_process.Open();
        }
        public void closeConnection_Process()
        {
            if (db_connect_process.State == System.Data.ConnectionState.Open)
                db_connect_process.Close();
        }
        public SqlConnection get_connection_process() { return db_connect_process; }

        public void openConnection_Protocol()
        {
            if (db_connect_protocol.State == System.Data.ConnectionState.Closed)
                db_connect_protocol.Open();
        }
        public void closeConnection_Protocol()
        {
            if (db_connect_protocol.State == System.Data.ConnectionState.Open)
                db_connect_protocol.Close();
        }
        public SqlConnection get_connection_protocol() { return db_connect_protocol; }
    }
}
