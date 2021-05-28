using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDbToSqlLib
{
    public class OrdersController
    {
        private static Connection connection { get; set; }

        public OrdersController(Connection connection)
        {

            OrdersController.connection = connection;
        }

        private Order FillOrderForSqlRow(SqlDataReader sqldatareader)
        {
            var order = new Order()
            {
                Id = Convert.ToInt32(sqldatareader["Id"]),
                CustomerId = Convert.ToInt32(sqldatareader["CustomerId"]),
                Date = Convert.ToDateTime(sqldatareader["Date"]),
                Description = Convert.ToString(sqldatareader["Description"])
            };

            return order;
        }

        private void FillParametersForSql(SqlCommand cmd, Order order)
        {
            cmd.Parameters.AddWithValue("@CustomerId", order.CustomerId);
            cmd.Parameters.AddWithValue("@Date", order.Date);
            cmd.Parameters.AddWithValue("@Description", order.Description);
        }

        public List<Order> GetAll()
        {
            var sql = "  Select * from Orders; ";
            var cmd = new SqlCommand(sql, connection.sqlconn);
            var sqldatareader = cmd.ExecuteReader();
            var orders = new List<Order>();

            while (sqldatareader.Read())
            {
                var order = FillOrderForSqlRow(sqldatareader);
                orders.Add(order);

            }
            sqldatareader.Close();

            foreach(var o in orders)
            {

                GetCustomerForOrder(o);
            }
            return orders;
        }

        private void GetCustomerForOrder(Order order)
        {
            var custyCntrl = new CustomersController(connection);
            order.customer = custyCntrl.GetByPK(order.CustomerId);

        }

        public Order GetByPk(int id)
        {
            var sql = $" Select * from Orders Where @Id = {id}; ";
            var cmd = new SqlCommand(sql, connection.sqlconn);
            cmd.Parameters.AddWithValue("@Id", id);
            var sqldatareader = cmd.ExecuteReader();

            if (!sqldatareader.HasRows)
            {
                sqldatareader.Close();
                return null;
            }

            sqldatareader.Read();
            var order = FillOrderForSqlRow(sqldatareader);
            sqldatareader.Close();
            return order;
        }
  //CREATE BY CUSTOMER ID
        public bool Create(Order order)
        {
            var sql = $" Insert into Orders " +
                " ( CustomerId, Date, Description) " +
                " VALUES " +
                " (@CustomerId, @Date, @Description) ; ";

            var cmd = new SqlCommand(sql, connection.sqlconn);
            FillParametersForSql(cmd, order);
            var rowsAffected = cmd.ExecuteNonQuery();

            return (rowsAffected == 1);
        }
  // CREATE BY CUSTOMER NAME
        public bool Create( Order order, string CustomerName)
        {
            var custyCntrl = new CustomersController(connection);
            var custy = custyCntrl.GetByCustomerName(CustomerName);
            {
                order.CustomerId = custy.Id;
                return Create(order);
            }
        }

        public bool Change(Order order)
        {
            var sql = "  UPDATE Orders set " +
                " CustomerId = @CustomerId, " +
                " Date = @Date, " +
                " Description = @Description " +
                " Where Id = @Id; ";

            var cmd = new SqlCommand(sql, connection.sqlconn);
            FillParametersForSql(cmd, order);
            var rowsAffected = cmd.ExecuteNonQuery();

            return (rowsAffected == 1);
        }

        public bool Delete(Order order)
        {
            var sql = " Delete from Orders where Id = @Id; ";
            var cmd = new SqlCommand(sql, connection.sqlconn);

            cmd.Parameters.AddWithValue("@Id", order.Id);
            var rowsAffected = cmd.ExecuteNonQuery();

            return (rowsAffected == 1);

        }
    }
}
