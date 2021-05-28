using System;
using System.Collections.Generic;
using System.Text;

namespace SalesDbToSqlLib
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Sales { get; set; }
        public bool Active { get; set; }


    }
}
