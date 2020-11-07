using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface ILocation {

        string Name { get; }
        IDictionary<IProduct, int> Stock { get; }

        bool PlaceOrder(Customer customer);
        bool AddStock(IProduct product, int qty);
    }
}
