using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Order : IOrder {
        public int OrderNumber { get; set; }
        public Dictionary<IProduct, int> Products { get; set; }
        public ILocation Location { get; set; }
        public Customer Customer { get; set; }
        public DateTime Time { get; set; }

        public Order() { }

        public Order(int order_number, ILocation location, Customer customer, DateTime time) {
            OrderNumber = order_number;
            Products = customer.Cart;
            Location = location;
            Customer = customer;
            Time = time;
        }
    }
}
