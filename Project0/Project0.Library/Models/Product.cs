using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Product : IProduct {

        private double _price;
        public int Id { get; }
        public string DisplayName { get; }
        public double Price {
            get {
                return _price;
            }
            private set { 
                if (value <= 0) {
                    throw new ArgumentOutOfRangeException("value", "price must be positive");
                }
            }
        }

        public Product(int id, string name, double price) {
            Id = id;
            DisplayName = name;
            Price = price;
        }
    }
}
