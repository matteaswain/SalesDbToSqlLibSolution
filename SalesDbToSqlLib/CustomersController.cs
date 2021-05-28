using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDbToSqlLib
{
    public class CustomersController
    {
        private static Connection connection { get; set;}

        public CustomersController(Connection connection)
        {
            CustomersController.connection = connection;
        }

        private Customer FillCustomerForSqlRow(SqlDataReader sqldatareader)
        {
            var customer = new Customer()
            {
                Id = Convert.ToInt32(sqldatareader["Id"]),
                Name = Convert.ToString(sqldatareader["Name"]),
                City = Convert.ToString(sqldatareader["City"]),
                State = Convert.ToString(sqldatareader["State"]),
                Sales = Convert.ToDecimal(sqldatareader["Sales"]),
                Active = Convert.ToBoolean(sqldatareader["Active"])
            };
            return customer;
        }

        private void FillParameterForSql(SqlCommand cmd, Customer customer)
        {
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@City", customer.City);
            cmd.Parameters.AddWithValue("@State", customer.City);
            cmd.Parameters.AddWithValue("@Sales", customer.Sales);
            cmd.Parameters.AddWithValue("@Active", customer.Active);
        }

        public Customer GetByCustomerName(string customername)
        {
            var sql = " SELECT * from Customers Where name = @name; ";
            var cmd = new SqlCommand(sql, connection.sqlconn);
            cmd.Parameters.AddWithValue("@name", customername);
            var sqldatareader = cmd.ExecuteReader();

            if (!sqldatareader.HasRows)
            {
                sqldatareader.Close();
                return null;
            }

            var customernames = new List<Customer>();
            while(sqldatareader.Read())
            {
                var customer = FillCustomerForSqlRow(sqldatareader);
                customernames.Add(customer);
            }
            sqldatareader.Close();
            return customername;
        }

        public List<Customer> GetAll()
        {
            var sql = " SELECT * from Customers; ";
            var cmd = new SqlCommand(sql, connection.sqlconn);
            var sqldatareader = cmd.ExecuteReader();
            var customers = new List<Customer>();

            while (sqldatareader.Read())
            {
                var customer = FillCustomerForSqlRow(sqldatareader);
                customers.Add(customer);
            }
            sqldatareader.Close();
            return customers;
        }

        public Customer GetByPK(int id)
        {
            var sql = $" Select * from Customers Where Id = {id}; "; // 
            var cmd = new SqlCommand(sql, connection.sqlconn);
            cmd.Parameters.AddWithValue("@id", id);
            var sqldatareader = cmd.ExecuteReader();

            if (!sqldatareader.HasRows)
            {
                sqldatareader.Close();
                return null;
            }
            sqldatareader.Read();

            var customer = FillCustomerForSqlRow(sqldatareader);
            sqldatareader.Close();
            return customer;
        }
//CREATES NEW CUSTOMER
        public bool Create( Customer customer)
        {
            var sql = " Insert in Customers " +
                " Name, City, State, Sales, Active " +
                " VALUES " +
                " @Name, @City, @State, @Sales, @Active; ";

            var cmd = new SqlCommand(sql, connection.sqlconn);
            FillParameterForSql(cmd, customer);

            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
// UPDATE CUSTOMER BY ID 
        public bool Update(Customer customer)
        {
            var sql = " UPDATE Customers set " +
                " Name = @Name, City = @City, State = @State, Sales = @Sales, Active = @Active " +
                " Where Id = @Id; ";

            var cmd = new SqlCommand(sql, connection.sqlconn);
            FillParameterForSql(cmd, customer);
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }
        public bool Delete(Customer customer)
        {
            var sql = " Delete from Customers Where Id = @Id; ";
            var cmd = new SqlCommand(sql, connection.sqlconn);
            cmd.Parameters.AddWithValue("@Id", customer.Id); // 
            var rowsAffected = cmd.ExecuteNonQuery();
            return (rowsAffected == 1);
        }



    }
}
