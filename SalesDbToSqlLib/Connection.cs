using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace SalesDbToSqlLib
{
    public class Connection
    {
        public SqlConnection sqlconn { get; set; }

        public void Disconnect()
        {
            if(sqlconn == null)
            {
                return;
            }
            sqlconn.Close();
            sqlconn = null;
        } 

        public Connection (string server, string database)
        {
            var connStr = $"server={server};database={database};trusted_connection=true;";
            sqlconn = new SqlConnection(connStr);
            sqlconn.Open();
            if(sqlconn.State != System.Data.ConnectionState.Open)
            {
                sqlconn = null;
                throw new Exception("Connection failed. Try again!");
            }



        }
    }
}
