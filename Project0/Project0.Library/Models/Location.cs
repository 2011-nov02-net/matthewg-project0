using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Location : ILocation {
        
        public IStore Store { get; set; }
        public string Name { get; set; }
        public string Address { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string Zip { get; }
        public string Phone { get; set; }
        public Dictionary<IProduct, int> Stock { get; set; }

        public Location() { }

        public Location(string name, string address, string city, string state, string country, string zip, string phone) {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Country = country;
            Zip = zip;
            Phone = phone;
        }

        public Location(string name, IStore store) {
            Name = name;
            Store = store;
            Stock = new Dictionary<IProduct, int>();
        }

        public IOrder PlaceOrder(Customer customer) {
            return Store.PlaceOrder(customer, this);
        }

        public bool AddStock(IProduct product, int qty) {
            if (Stock.ContainsKey(product)) {
                Stock[product] += qty;
                return true;
            } else if (qty > 0) {
                Stock.Add(product, qty);
                return true;
            }
            return false;
        }

    }
}
