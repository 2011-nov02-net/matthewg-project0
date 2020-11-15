using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// TODO:
// - display all order history of a store location
// - documentation with <summary> XML comments on all public types and members (optional: <params> and <return>)
// - rejects orders with unreasonably high product quantities
// - at least 10 test methods
// - focus on unit testing business logic; testing the console app is very low priority
// - do proper TDD for at least one class in the app
// - (optional: order history can be sorted by earliest, latest, cheapest, most expensive)
// - (optional: get a suggested order for a customer based on his order history)
// - (optional: display some statistics based on order history)
// - (optional: asynchronous network & file I / O)
// - (optional: customer has a default store location to order from)
// - (optional: some additional business rules, like special deals)
// - (optional: for at least one product, more than one inventory item decrements when ordering that product)





namespace Project0.Library.Models {
    [DataContract(Name = "Store", Namespace = "", IsReference = true)]
    [KnownType(typeof(Customer))]
    [KnownType(typeof(Location))]
    [KnownType(typeof(Order))]
    [KnownType(typeof(Product))]
    public class Store {
        [DataMember(Name = "_product_ID_seed")]
        private int _product_ID_seed;

        [DataMember(Name = "Products")]
        public List<Product> Products { get; private set; }

        [DataMember(Name = "Locations")]
        public List<Location> Locations { get; private set; }

        [DataMember(Name = "Customers")]
        public List<Customer> Customers { get; private set; }

        [DataMember(Name = "OrderHistory")]
        public List<Order> OrderHistory { get; private set; }

        public Store() {
            _product_ID_seed = 1000;
            Products = new List<Product>();
            Locations = new List<Location>();
            Customers = new List<Customer>();
            OrderHistory = new List<Order>();
        }

        public Order PlaceOrder(Customer customer, Location location) {
            Order order = new Order(location, customer, DateTime.Now);
            OrderHistory.Add(order);
            customer.NewCart();
            return order;
        }

        public bool AddStandardLocation(string name) {
            foreach (var loc in Locations) {
                if (loc.Name == name) {
                    return false;
                }
            }

            Locations.Add(new Location());
            return true;
        }
        public Customer AddCustomer(string first_name, string last_name, string email) {
            if (SearchCustomerByEmail(email) != null) {
                throw new ArgumentException("Email already in use.");
            }
            var customer = new Customer(first_name, last_name, email);
            Customers.Add(customer);
            return customer;
        }

        public bool AddStock(Location location, Product product, int qty) {
            return location.AddStock(product, qty);
        }

        public void AddProduct(string name) {
            Products.Add(new Product(name));
        }

        public ICollection<Customer> SearchCustomerByName(string s) {
            ICollection<Customer> users = new HashSet<Customer>();
            foreach (var customer in Customers) {
                string customer_name = $"{customer.FirstName} {customer.LastName}";

                if (customer_name.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0) {
                    users.Add(customer);
                }
            }
            return users;
        }

        public Customer SearchCustomerByEmail(string s) {
            foreach (var customer in Customers) {
                if (customer.Email == s) {
                    return customer;
                }
            }
            return null;
        }

        public ICollection<Order> SearchOrderHistoryByCustomer(Customer customer) {
            ICollection<Order> orders = new List<Order>();
            foreach (var order in OrderHistory) {
                if (order.Customer == customer) {
                    orders.Add(order);
                }
            }
            return orders;
        }
    }
}
