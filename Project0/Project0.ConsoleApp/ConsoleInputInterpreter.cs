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
                    new_user = Prompts.RegisterPrompt(this);
                }
                return new_user;
            }
            if (s.Equals("q", StringComparison.OrdinalIgnoreCase)) {
                // DataPersistence.Write(store, "../../../store_data.xml");
                Environment.Exit(0);
            }
            var customer = store.GetCustomerByEmail(s);
            if (customer != null) {
                Prompts.ReturningCustomerPrompt(customer);
                return customer;
            }
            Console.WriteLine("Invalid input.");
            return null;
        }

        public string[] ParseNewCustomer(string s) {
            if (s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            string pattern = "\\w+\\s\\w+\\s(.+)(@)(.+)[.](.+)";
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, pattern)) {
                return null;
            }
            return s.Split();
        }

        public Customer RegisterCustomer(string[] details, IStoreRepository store) {
            Customer customer = new Customer(details[0], details[1], details[2]);
            try {
                store.AddCustomer(customer);
            } catch (Exception) {
                return null;
            }
            store.Save();
            return customer;
        }

        public bool? ValidLocation(string s, IStoreRepository store, Customer customer, out Location location) {
            if (s.Equals("logout", StringComparison.OrdinalIgnoreCase)) {
                location = null;
                return null;
            }
            if (s.Equals("history", StringComparison.OrdinalIgnoreCase) && customer != null) {
                location = null;
                return Prompts.PrintOrderHistory(customer);
            }
            if (s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                location = null;
                return null;
            }
            int location_index;
            try {
                location_index = int.Parse(s);
            } catch (Exception) { location = null;  return true; }
            if (location_index < store.GetLocations().Count && location_index >= 0) {
                location = store.GetLocations()[location_index];
                if (customer != null) {
                    customer.CurrentLocation = location;
                }
                return false;
            }
            location = null;
            return true;
        }

        public bool? ValidProduct(string s, IStoreRepository store, out Product product) {
            if (s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                product = null;
                return null;
            }
            int product_index;
            try {
                product_index = int.Parse(s);
            } catch (Exception) { product = null; return true; }
            if (product_index < store.GetProducts().Count && product_index >= 0) {
                product = store.GetProducts()[product_index];
                return false;
            }
            product = null;
            return true;
        }

        public bool? ValidAdminCommand(string s, IStoreRepository store) {
            if (s.Equals("logout", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                return Prompts.NewStoreLocation(this);
            }
            if (s.Equals("1", StringComparison.OrdinalIgnoreCase)) {
                return Prompts.RestockPrompt(this);
            }
            if (s.Equals("2", StringComparison.OrdinalIgnoreCase)) {
                return Prompts.NewProductPrompt(this);
            }
            if (s.Equals("3", StringComparison.OrdinalIgnoreCase)) {
                return Prompts.UserLookupPrompt(this);
            }
            if (s.Equals("4", StringComparison.OrdinalIgnoreCase)) {
                return Prompts.PrintOrderHistory(null);
            }
            return false;
        }

        public bool GenerateLocation(string s, IStoreRepository store) {
            Location location = new Location(s, "", "", "", "", "", ""); // TODO: Console prompts for location info
            store.AddLocation(location);
            store.Save();
            return true;
        }

        public bool RestockLocation(IStoreRepository store, Location location, Product product, int qty) {
            decimal? price;
            if (!location.Prices.ContainsKey(product)) {
                price = Prompts.ProductPricePrompt(this);
                if (price == null) {
                    return true;
                }
                location.AddPrice(product, (decimal)price);
            }
            location.AddStock(product, qty);
            store.UpdateLocationStock(location, product);
            store.Save();
            return true;
        }

        public decimal? ParsePrice(string s) {
            decimal price;
            if (s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            try {
                price = decimal.Parse(s);
            } catch (Exception) { return 0; }
            return price;
        }

        public bool GenerateProduct(string s, IStoreRepository store) {
            Product product = new Product() { DisplayName = s };
            store.AddProduct(product);
            store.Save();
            return true;
        }

        public ICollection<Customer> UserLookup(string s, IStoreRepository store) {
            string pattern = "\\w+\\s\\w+";
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, pattern)) {
                return null;
            }
            string[] name = s.Split();
            return store.GetCustomersByName(name[0], name[1]);
        }

        public KeyValuePair<Product, int>? ProductSelection(string s, Customer customer, out int exit_status) {
            if (s.Equals("leave", StringComparison.OrdinalIgnoreCase)) {
                customer.EmptyCart();
                exit_status = 1;
                return null;
            }
            if (s.Equals("cart", StringComparison.OrdinalIgnoreCase)) {
                bool? response = true;
                while (response ?? false) {
                    response = Prompts.CartPrompt(this, customer);
                }
                if (response != null) {
                    exit_status = 2;
                } else {
                    exit_status = 0;
                }
                return null;
            }
            int product_index;
            try {
                product_index = int.Parse(s);
            } catch (Exception) {
                exit_status = 0;
                return null;
            }
            if (product_index < customer.CurrentLocation.Stock.Count && product_index >= 0) {
                exit_status = 0;
                return customer.CurrentLocation.Stock.ElementAt(product_index);
            }
            exit_status = 0;
            return null;
        }

        public bool QuantitySelection(string s, KeyValuePair<Product, int>? purchase_item, Customer customer) {
            int qty;
            try {
                qty = int.Parse(s);
            } catch (Exception) { return true; }
            if (qty <= purchase_item.Value.Value && qty > 0) {
                customer.AddToCart(purchase_item.Value.Key, qty);
                return true;
            }
            return true;
        }

        public bool? CartCommands(string s, Customer customer, IStoreRepository store) {
            if (s.Equals("remove", StringComparison.OrdinalIgnoreCase)) { // TODO: functionality to remove a product from the cart
                return true;
            }
            if (s.Equals("checkout", StringComparison.OrdinalIgnoreCase)) {
                Order order = new Order(customer.CurrentLocation, customer, DateTime.Now);
                store.AddOrder(order);
                store.Save();
                customer.CurrentLocation = null;
                Prompts.CheckoutPrompt(order);
                return false;
            }
            if (s.Equals("back", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            return true;
        }
        
    }
}
