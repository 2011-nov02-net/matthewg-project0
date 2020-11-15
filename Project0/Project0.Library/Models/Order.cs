﻿using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Order {
        public int Id { get; set; }
        public Dictionary<Product, int> Products { get; set; }
        public Dictionary<Product, decimal> PricePaid { get; set; }
        public Location Location { get; set; }
        public Customer Customer { get; set; }
        public DateTime Time { get; set; }

        public Order() { }

        public Order(Location location, Customer customer, DateTime time) {
            Products = customer.Cart;
            Location = location;
            Customer = customer;
            Time = time;
        }
    }
}
