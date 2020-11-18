using Project0.Library.Interfaces;
using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project0.ConsoleApp {
    public class ConsoleInputInterpreter : IUserInputInterpreter {

        public IUserPrompts Prompts { get; }

        public ConsoleInputInterpreter(ConsolePrompts prompts) {
            Prompts = prompts;
        }

        public IUser ValidUserID(string s, IStoreRepository store) {
            if (s.Equals("admin", StringComparison.OrdinalIgnoreCase)) {
                return new Admin();
            }
            if (s.Equals("register", StringComparison.OrdinalIgnoreCase)) {
                IUser new_user = null;
                while (new_user == null) {
                    new_user = Prompts.RegisterPrompt(this, out bool exit);
                    if (exit) {
                        return null;
                    }
                }
                return new_user;
            }
            if (s.Equals("q", StringComparison.OrdinalIgnoreCase)) {
                // DataPersistence.Write(store, "../../../store_data.xml");
                Environment.Exit(0);
            }
            Customer customer;
            try {
                customer = store.GetCustomerByEmail(s);
            } catch (InvalidOperationException) {
                Console.WriteLine("Invalid input.");
                return null;
            }
            Prompts.ReturningCustomerPrompt(customer);
            return customer;
        }

        public string[] ParseNewCustomer(string s, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return null;
            }
            string pattern = "\\w+\\s\\w+\\s(.+)(@)(.+)[.](.+)";
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, pattern)) {
                return new string[0];
            }
            return s.Split();
        }

        public Customer RegisterCustomer(string[] details, IStoreRepository store) {
            Customer customer = new Customer(details[0], details[1], details[2]);
            try {
                store.AddCustomer(customer);
                store.Save();
            } catch (Exception) {
                return null;
            }
            return customer;
        }

        public Location ValidLocation(string s, IStoreRepository store, Customer customer, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase) || s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return null;
            }
            Location location;
            int location_index;
            try {
                location_index = int.Parse(s);
            } catch (Exception) { return null; }
            if (location_index < store.GetLocations().Count + 1 && location_index > 0) {
                location = store.GetLocations()[location_index - 1];
                if (customer != null) {
                    customer.CurrentLocation = location;
                    Prompts.LocationInventoryPrompt(this, customer, out exit);
                }
                return location;
            }
            return null;
        }

        public void CustomerOperations(string s, Customer customer, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
            }
            if (s.Equals("2", StringComparison.OrdinalIgnoreCase)) {
                Prompts.PrintOrderHistory(customer);
            }
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                Prompts.EnterStoreLocationPrompt(this, customer);
            }
        }

        public Product ValidProduct(string s, IStoreRepository store, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return null;
            }
            int product_index;
            try {
                product_index = int.Parse(s);
            } catch (Exception) { return null; }
            if (product_index < store.GetProducts().Count + 1&& product_index > 0) {
                return store.GetProducts()[product_index - 1];
            }
            return null;
        }

        public Product ValidCustomerProduct(string s, Customer customer, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return null;
            }
            int product_index;
            try {
                product_index = int.Parse(s);
            } catch (Exception) { return null; }
            if (product_index < customer.Cart.Count + 1 && product_index > 0) {
                return customer.Cart.ElementAt(product_index - 1).Key;
            }
            return null;
        }

        public void AdminOperations(string s, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
            }
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                Prompts.NewStoreLocation(this);
            }
            if (s.Equals("2", StringComparison.OrdinalIgnoreCase)) {
                Prompts.RestockPrompt(this);
            }
            if (s.Equals("3", StringComparison.OrdinalIgnoreCase)) {
                Prompts.NewProductPrompt(this);
            }
            if (s.Equals("4", StringComparison.OrdinalIgnoreCase)) {
                Prompts.UserLookupPrompt(this);
            }
            if (s.Equals("5", StringComparison.OrdinalIgnoreCase)) {
                Prompts.OrderHistoryPrompt(this);
            }
        }

        public void ValidOrderHistoryOption(string s, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return;
            }
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                Prompts.PrintOrderHistory();
            }
            if (s.Equals("2", StringComparison.OrdinalIgnoreCase)) {
                Customer customer = Prompts.CustomerEmailEntry(this);
                if (customer == null) {
                    return;
                }
                Prompts.PrintOrderHistory(customer);
            }
            if (s.Equals("3", StringComparison.OrdinalIgnoreCase)) {
                Location location = Prompts.LocationEntry(this);
                if (location == null) {
                    return;
                }
                Prompts.PrintOrderHistory(location);
            }
            
        }

        public bool GenerateLocation(string s, IStoreRepository store, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return false;
            }
            Location location = new Location(s, "", "", "", "", "", ""); // TODO: Console prompts for location info
            store.AddLocation(location);
            store.Save();
            return true;
        }

        public bool RestockLocation(IStoreRepository store, Location location, Product product, int qty) {
            decimal price;
            if (!location.Prices.ContainsKey(product)) {
                price = Prompts.ProductPricePrompt(this, out bool exit);
                if (exit) {
                    return false;
                }
                location.AddPrice(product, price);
            }
            location.AddStock(product, qty);
            store.UpdateLocationStock(location, product);
            store.Save();
            return true;
        }

        public decimal ParsePrice(string s, out bool exit) {
            decimal price;
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return 0;
            }
            try {
                price = decimal.Parse(s);
            } catch (Exception) { return 0; }
            return price;
        }

        public int ParseQuantity(string s, out bool exit) {
            int qty;
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return 0;
            }
            try {
                qty = int.Parse(s);
            } catch (Exception) { return 0; }
            return qty;
        }

        public bool GenerateProduct(string s, IStoreRepository store) {
            Product product = new Product() { DisplayName = s };
            store.AddProduct(product);
            store.Save();
            return true;
        }

        public ICollection<Customer> UserLookup(string s, IStoreRepository store, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return null;
            }
            string pattern = "\\w+\\s\\w+";
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, pattern)) {
                return null;
            }
            string[] name = s.Split();
            ICollection<Customer> customers;
            try {
                customers = store.GetCustomersByName(name[0], name[1]);
            } catch (InvalidOperationException) {
                return null;
            }
            return customers;
        }

        public Customer CustomerEmailLookup(string s, IStoreRepository store, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
                return null;
            }
            Customer customer;
            try {
                customer = store.GetCustomerByEmail(s);
            } catch (InvalidOperationException) {
                return null;
            }
            return customer;
        }

        public KeyValuePair<Product, int>? ProductSelection(string s, Customer customer, out bool exit) {
            exit = false;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                customer.EmptyCart();
                customer.CurrentLocation = null;
                exit = true;
                return null;
            }
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                Prompts.CartPrompt(this, customer, out bool checkout);
                if (checkout) {
                    exit = true;
                }
                return null;
            }
            int product_index;
            try {
                product_index = int.Parse(s);
            } catch (Exception) {
                return null;
            }
            if (product_index < customer.CurrentLocation.Stock.Count && product_index >= 0) {
                return customer.CurrentLocation.Stock.ElementAt(product_index);
            }
            return null;
        }

        public void QuantitySelection(string s, KeyValuePair<Product, int>? purchase_item, Customer customer) {
            int qty;
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                return;
            }
            try {
                qty = int.Parse(s);
            } catch (Exception) { return; }
            if (qty <= purchase_item.Value.Value && qty > 0) {
                customer.AddToCart(purchase_item.Value.Key, qty);
            }
        }

        public void CartCommands(string s, Customer customer, IStoreRepository store, out bool exit, out bool checkout) {
            exit = false;
            checkout = false;
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                Prompts.RemoveProductFromCartPrompt(this, customer);
            }
            if (s.Equals("2", StringComparison.OrdinalIgnoreCase)) {
                if (customer.Cart.Count == 0) {
                    return;
                }
                Order order = new Order(customer.CurrentLocation, customer, DateTime.Now);
                store.AddOrder(order);
                store.Save();
                customer.NewCart();
                customer.CurrentLocation = null;
                Prompts.CheckoutPrompt(order);
                exit = true;
                checkout = true;
            }
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                exit = true;
            }
        }
        
    }
}
