using System.Collections.Generic;

namespace Project0.Library.Models {
    public class Location {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string Zip { get; }
        public string Phone { get; set; }
        public Dictionary<Product, int> Stock { get; set; }
        public Dictionary<Product, decimal> Prices { get; set; }

        public Location() {
            Stock = new Dictionary<Product, int>();
            Prices = new Dictionary<Product, decimal>();
        }

        public Location(string name, string address, string city, string state, string country, string zip, string phone) {
            Name = name;
            Address = address;
            City = city;
            State = state;
            Country = country;
            Zip = zip;
            Phone = phone;
            Stock = new Dictionary<Product, int>();
            Prices = new Dictionary<Product, decimal>();
        }

        public bool AddStock(Product product, int qty) {
            if (Stock.ContainsKey(product)) {
                Stock[product] += qty;
                return true;
            } else if (qty > 0) {
                Stock.Add(product, qty);
                return true;
            }
            return false;
        }

        public bool AddPrice(Product product, decimal price) {
            if (Prices.ContainsKey(product)) {
                Prices[product] = price;
                return true;
            } else if (price > 0) {
                Prices.Add(product, price);
                return true;
            }
            return false;
        }

    }
}
