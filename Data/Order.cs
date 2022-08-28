using System;
using System.Collections.Generic;

namespace InventMS.Data
{
    public class Order
    {
        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public double TotalPrice { get; set; }

        public Product Product { get; set; }
    }
}
