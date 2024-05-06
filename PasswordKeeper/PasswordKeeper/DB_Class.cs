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
        SqlConnection db_connect = new SqlConnection(@"Data Source=DESKTOP-6FEPCHS\SQLEXPRESS;Initial Catalog=passwords;Integrated Security=True");
        public void openConnection()
        {
            if (db_connect.State == System.Data.ConnectionState.Closed)
                db_connect.Open();
        }
        public void closeConnection()
        {
            if (db_connect.State == System.Data.ConnectionState.Open)
                db_connect.Close();
        }
        public SqlConnection get_connection() { return db_connect; }
    }
}
