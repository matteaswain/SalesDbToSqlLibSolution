using SalesDbToSqlLib;
using System;
using System.Linq;

namespace TestSalesDbToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlconn = new Connection("localhost\\sqlexpress01", "SalesDb");

            var custycontroller = new CustomersController(sqlconn);
            var customers = custycontroller.GetByPK(8);




            sqlconn.Disconnect();
        }
    }
}
