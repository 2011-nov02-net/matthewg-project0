using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Product {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public Product() { }

        public Product(string name) {
            DisplayName = name;
        }
    }
}
