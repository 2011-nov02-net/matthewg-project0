using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface IStore {

        List<ILocation> Locations { get; }
        IDictionary<string, IUser> Customers { get; }
        ICollection<IOrder> OrderHistory { get; }

        bool PlaceOrder(IOrder order);
        bool AddStandardLocation(string name);
        Customer AddCustomer(string first_name, string last_name);
    }
}
