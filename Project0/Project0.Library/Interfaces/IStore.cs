using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IStore {

        List<IProduct> Products { get; }
        List<ILocation> Locations { get; }
        List<IUser> Customers { get; }
        List<IOrder> OrderHistory { get; }

        IOrder PlaceOrder(Customer customer, Location location);
        bool AddStandardLocation(string name);
        Customer AddCustomer(string first_name, string last_name, string email);
        void AddProduct(string name, double price);
        bool AddStock(ILocation location, IProduct product, int qty);
        ICollection<IUser> SearchCustomerByName(string s);
        IUser SearchCustomerByEmail(string s);
        ICollection<IOrder> SearchOrderHistoryByCustomer(IUser customer);
    }
}
