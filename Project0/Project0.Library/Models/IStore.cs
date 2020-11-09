using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface IStore {

        List<IProduct> Products { get; }
        List<ILocation> Locations { get; }
        IDictionary<string, Customer> Customers { get; }
        ICollection<IOrder> OrderHistory { get; }

        IOrder PlaceOrder(IOrder order);
        bool AddStandardLocation(string name);
        Customer AddCustomer(string first_name, string last_name, string email);
        void AddProduct(string name, double price);
        bool AddStock(ILocation location, IProduct product, int qty);
    }
}
