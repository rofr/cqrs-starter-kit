using System.Collections.Generic;

namespace WebFrontend.Models
{
    public class OrderModel
    {
        public class OrderItem
        {
            public int MenuNumber { get; set; }
            public string Description { get; set; }
            public int NumberToOrder { get; set; }
        }

        public List<OrderItem> Items { get; set; }
    }

    public class OpenTabModel
    {
        public string Waiter { get; set; }
        public int TableNumber { get; set; }
    }
}