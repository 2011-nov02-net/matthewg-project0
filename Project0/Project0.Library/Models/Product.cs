using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Product : IProduct {
        private double _price;
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public double Price {
            get {
                return _price;
            }
            set { 
                if (value <= 0) {
                    throw new ArgumentOutOfRangeException("value", "price must be positive");
                } else {
                    _price = value;
                }
            }
        }

        public Product() { }

        public Product(int id, string name, double price) {
            Id = id;
            DisplayName = name;
            Price = price;
        }
    }
}
