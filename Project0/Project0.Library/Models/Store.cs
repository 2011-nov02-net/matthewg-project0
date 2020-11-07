using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Store : IStore {
        public List<ILocation> Locations { get; }

        public IDictionary<string, IUser> Customers { get; }

        public ICollection<IOrder> OrderHistory { get; }

        public Store() {
            Locations = new List<ILocation>();
            Customers = new Dictionary<string, IUser>();
            OrderHistory = new List<IOrder>();
        }

        public bool PlaceOrder(IOrder order) {
            OrderHistory.Add(order);
            order.Customer.NewCart();
            return true;
        }

        public bool AddStandardLocation(string name) {
            foreach (var loc in Locations) {
                if (loc.Name == name) {
                    return false;
                }
            }

            Locations.Add(new StandardStoreLocation(name, this));
            return true;
        }
        public Customer AddCustomer(string first_name, string last_name) {
            var customer = new Customer(first_name, last_name);
            Customers.Add(customer.Id, customer);
            return customer;
        }

    }
}
