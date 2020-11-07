using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface ILocation {

        string Name { get; }
        IDictionary<int, int> Stock { get; }

        bool PlaceOrder(Customer customer);
        bool AddStock(int product_id, int qty);
    }
}
