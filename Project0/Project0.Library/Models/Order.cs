using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Order : IOrder {

        public IDictionary<IProduct, int> Products { get; }
        public ILocation Location { get; }
        public Customer Customer { get; }
        public DateTime Time { get; }

        public Order(ILocation location, Customer customer, DateTime time) {
            Products = customer.Cart;
            Location = location;
            Customer = customer;
            Time = time;
        }
    }
}
