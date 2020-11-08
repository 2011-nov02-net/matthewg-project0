﻿using Project0.Library;
using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project0.ConsoleApp {
    public class ConsoleInputInterpreter : IUserInputInterpreter {

        public IUserPrompts Prompts { get; }

        public ConsoleInputInterpreter(ConsolePrompts prompts) {
            Prompts = prompts;
        }

        public IUser ValidUserID(string s, IStore store) {
            if (store.Customers.ContainsKey(s)) {
                Prompts.ReturningCustomerPrompt(store.Customers[s]);
                return store.Customers[s];
            }
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
                Environment.Exit(0);
            }
            Console.WriteLine("Invalid input.");
            return null;
        }

        public Customer RegisterCustomer(string s, IStore store) {
            string pattern = "\\w\\s{1}\\w";
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, pattern)) {
                return null;
            }
            string[] name = s.Split(" ");
            return store.AddCustomer(name[0], name[1]);
        }

        public bool? ValidLocation(string s, IStore store, Customer customer, out ILocation location) {
            if (s.Equals("logout", StringComparison.OrdinalIgnoreCase)) {
                location = null;
                return null;
            }
            if (s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                location = null;
                return null;
            }
            int location_index;
            try {
                location_index = int.Parse(s);
            } catch (Exception) { location = null;  return true; }
            if (location_index < store.Locations.Count && location_index >= 0) {
                location = store.Locations[location_index];
                if (customer != null) {
                    customer.CurrentLocation = location;
                }
                return false;
            }
            location = null;
            return true;
        }

        public bool? ValidProduct(string s, IStore store, out IProduct product) {
            if (s.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                product = null;
                return null;
            }
            int product_index;
            try {
                product_index = int.Parse(s);
            } catch (Exception) { product = null; return true; }
            if (product_index < store.Products.Count && product_index >= 0) {
                product = store.Products[product_index];
                return false;
            }
            product = null;
            return true;
        }

        public bool? ValidAdminCommand(string s, IStore store) {
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
            return false;
        }

        public bool GenerateLocation(string s, IStore store) {
            return store.AddStandardLocation(s);
        }

        public bool RestockLocation(IStore store, ILocation location, IProduct product, int qty) {
            return store.AddStock(location, product, qty);
        }

        public bool GenerateProduct(string s, IStore store, double price) {
            store.AddProduct(s, price);
            return true;
        }

        public KeyValuePair<IProduct, int>? ProductSelection(string s, Customer customer, out int exit_status) {
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

        public bool QuantitySelection(string s, KeyValuePair<IProduct, int>? purchase_item, Customer customer) {
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

        public bool? CartCommands(string s, Customer customer) {
            if (s.Equals("remove", StringComparison.OrdinalIgnoreCase)) { // TODO: functionality to remove a product from the cart
                return true;
            }
            if (s.Equals("checkout", StringComparison.OrdinalIgnoreCase)) {
                customer.LastOrder = customer.PlaceOrder();
                return false;
            }
            if (s.Equals("back", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            return true;
        }
        
    }
}
