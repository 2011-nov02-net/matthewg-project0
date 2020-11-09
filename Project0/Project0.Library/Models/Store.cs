using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Store : IStore {

        // private int _customer_ID_seed;
        private int _product_ID_seed;
        public List<IProduct> Products { get; }
        public List<ILocation> Locations { get; }

        public IDictionary<string, Customer> Customers { get; }

        public ICollection<IOrder> OrderHistory { get; }

        public Store() {
            Products = new List<IProduct>();
            Locations = new List<ILocation>();
            Customers = new Dictionary<string, Customer>();
            OrderHistory = new List<IOrder>();
            // _customer_ID_seed = 0;
        }

        public IOrder PlaceOrder(IOrder order) {
            OrderHistory.Add(order);
            order.Customer.NewCart();
            return order;
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
        public Customer AddCustomer(string first_name, string last_name, string email) {
            var customer = new Customer(first_name, last_name, email);
            Customers.Add(email, customer);
            return customer;
        }

        public bool AddStock(ILocation location, IProduct product, int qty) {
            return location.AddStock(product, qty);
        }

        public void AddProduct(string name, double price) {
            Products.Add(new Product(++_product_ID_seed, name, price));
        }
    }
}
