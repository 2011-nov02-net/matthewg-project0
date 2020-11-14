using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Product : IProduct {
        public int Id { get; set; }
        public string DisplayName { get; set; }

        public Product() { }

        public Product(int id, string name) {
            Id = id;
            DisplayName = name;
        }
    }
}
