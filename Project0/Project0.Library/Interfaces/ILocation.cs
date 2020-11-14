using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface ILocation {

        string Name { get; }
        Dictionary<Product, int> Stock { get; }
        Dictionary<Product, decimal> Prices { get; }

        IOrder PlaceOrder(Customer customer);
        bool AddStock(IProduct product, int qty);
    }
}
