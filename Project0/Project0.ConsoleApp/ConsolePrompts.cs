using Project0.Library.Interfaces;
using Project0.Library.Models;
using System;
using System.Collections.Generic;

namespace Project0.ConsoleApp {
    public class ConsolePrompts : IUserPrompts {

        public IStoreRepository Store { get; }

        public ConsolePrompts(IStoreRepository store) {
            Store = store;
        }

        public void WelcomeMessage() {
            Console.WriteLine("Welcome to the Walmart Shopping App.\n");
        }

        public IUser StartupPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter your email address or 'register' if you are a new customer. Enter [Q] to quit.");
            string input = Console.ReadLine();
            return interpreter.ValidUserID(input, Store);
        }

        public IUser RegisterPrompt(IUserInputInterpreter interpreter, out bool exit) {
            Console.WriteLine("\nEnter your first name, last name, and email address - separated by whitespace, [cancel] to Exit.");
            string input = Console.ReadLine();
            string[] customer_details = interpreter.ParseNewCustomer(input, out exit);
            if (exit) {
                return null;
            }
            if (customer_details.Length == 0) {
                Console.WriteLine("\nInvalid input. Proper syntax: <FirstName> <LastName> <Email>");
                return null;
            }
            var new_customer = interpreter.RegisterCustomer(customer_details, Store);
            if (new_customer == null) {
                Console.WriteLine("\nEmail address is already in use.");
                return null;
            }
            Console.WriteLine($"Welcome {new_customer.FirstName} {new_customer.LastName}.\n" +
                $"Your user account has been created. Please use your email, {new_customer.Email} to sign in from now on.\n");
            return new_customer;
        }

        public void ReturningCustomerPrompt(IUser customer) {
            Console.WriteLine($"Welcome back, {customer.FirstName} {customer.LastName}.");
        }

        public Location StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer, out bool exit) {
            Console.WriteLine("\nPlease select the store location you would like to order from.");
            int i = 0;
            foreach (var loc in Store.GetLocations()) {
                Console.WriteLine($"[{i++}] {loc.Name}");
            }
            Console.WriteLine("[history] View order history");
            Console.WriteLine("[logout] Exit session.");
            string input = Console.ReadLine();
            return interpreter.ValidLocation(input, Store, customer, out exit);
        }

        public void AdminPrompt(IUserInputInterpreter interpreter, out bool exit) {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("[0] Add new store location");
            Console.WriteLine("[1] Restock stores");
            Console.WriteLine("[2] Add new product");
            Console.WriteLine("[3] Search users");
            Console.WriteLine("[4] View order history");
            Console.WriteLine("[logout] Exit session.");
            string input = Console.ReadLine();

            interpreter.ValidAdminCommand(input, out exit);
        }

        public void OrderHistoryPrompt(IUserInputInterpreter interpreter) {
            bool exit = false;
            while (!exit) {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("[0] All orders");
                Console.WriteLine("[1] Search a customer");
                Console.WriteLine("[2] Search a location");
                Console.WriteLine("[cancel] Exit.");
                string input = Console.ReadLine();
                interpreter.ValidOrderHistoryOption(input, out exit);
            }
        }

        public void NewStoreLocation(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter the name of the new location.");
            string input = Console.ReadLine();
            // TODO: Prompts for location info
            bool success = interpreter.GenerateLocation(input, Store, out bool exit);
            if (exit) {
                return;
            }
            if (success) {
                Console.WriteLine($"\nSuccessfully created new store location: {input}.");
            } else {
                Console.WriteLine("\nFailed to create store location.");
            }
        }

        public void RestockPrompt(IUserInputInterpreter interpreter) {
            int i;
            string input;

            Location location = null;
            Product product = null;
            int qty = 0;
            while (location == null) {
                Console.WriteLine("\nChoose a location:");
                i = 0;
                foreach (var loc in Store.GetLocations()) {
                    Console.WriteLine($"[{i++}] {loc.Name}");
                }
                Console.WriteLine("[cancel] EXIT");
                input = Console.ReadLine();
                location = interpreter.ValidLocation(input, Store, null, out bool exit);
                if (exit) {
                    return;
                }
            }

            while (product == null) {
                Console.WriteLine("\nChoose a product:");
                i = 0;
                foreach (var pro in Store.GetProducts()) {
                    Console.WriteLine($"[{i++}] {pro.DisplayName}");
                }
                Console.WriteLine("[cancel] EXIT");
                input = Console.ReadLine();
                product = interpreter.ValidProduct(input, Store, out bool exit);
                if (exit) {
                    return;
                }
            }

            while (qty <= 0) {
                Console.WriteLine("\nEnter a quantity to add. [cancel] to exit.");
                input = Console.ReadLine();
                if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                    return;
                }
                try {
                    qty = int.Parse(input);
                } catch (Exception) { continue; }
            }
            bool success = interpreter.RestockLocation(Store, location, product, qty);
            if (success) {
                Console.WriteLine($"\nSuccessfully restocked store location: {location.Name} with {qty} {product.DisplayName}.");
            }
        }

        public decimal ProductPricePrompt(IUserInputInterpreter interpreter, out bool exit) {
            exit = false;
            decimal price = 0;
            while (price <= 0) {
                Console.WriteLine("\nEnter price. [cancel] to exit.");
                string price_input = Console.ReadLine();
                price = interpreter.ParsePrice(price_input, out exit);
                if (exit) {
                    return 0;
                }
            }
            return price;
        }

        public void NewProductPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter product name. [cancel] to exit.");
            string product_name = Console.ReadLine();
            if (product_name.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                return;
            }
            interpreter.GenerateProduct(product_name, Store); // TODO: check for duplicate product names
        }

        public void UserLookupPrompt(IUserInputInterpreter interpreter) {
            ICollection<Customer> users = null;
            while (users == null) {
                Console.WriteLine("\nEnter a user's first and last name. [cancel] to exit.");
                string input = Console.ReadLine();
                users = interpreter.UserLookup(input, Store, out bool exit);
                if (exit) {
                    return;
                }
            }
            foreach (var user in users) {
                Console.WriteLine($"{user.LastName}, {user.FirstName} - {user.Email}");
            }
            Console.WriteLine();
        }

        public Customer CustomerEmailEntry(IUserInputInterpreter interpreter) {
            Customer customer = null;
            while (customer == null) {
                Console.WriteLine("\nEnter customer's email address. [cancel] to exit.");
                string input = Console.ReadLine();
                customer = interpreter.CustomerEmailLookup(input, Store, out bool exit);
                if (exit) {
                    return null;
                }
            }
            return customer;
        }

        public Location LocationEntry(IUserInputInterpreter interpreter) {
            int i;
            string input;
            Location location = null;
            while (location == null) {
                Console.WriteLine("\nChoose a location:");
                i = 0;
                foreach (var loc in Store.GetLocations()) {
                    Console.WriteLine($"[{i++}] {loc.Name}");
                }
                Console.WriteLine("[cancel] EXIT");
                input = Console.ReadLine();
                location = interpreter.ValidLocation(input, Store, null, out bool exit);
                if (exit) {
                    return null;
                }
            }
            return location;
        }

        public void PrintOrderHistory() {
            ICollection<Order> orders = Store.GetOrders();
            Console.WriteLine();
            foreach (var order in orders) {
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                decimal total_price = 0;
                foreach (var item in order.Products) {
                    decimal item_price = order.PricePaid[item.Key] * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName} x{item.Value} - {item_price:c}");
                }
                Console.WriteLine($"Total: {total_price:c}");
                Console.WriteLine();
            }
        }

        public void PrintOrderHistory(Customer customer) {
            ICollection<Order> orders = Store.GetCustomerOrders(customer);
            Console.WriteLine();
            foreach (var order in orders) {
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                decimal total_price = 0;
                foreach (var item in order.Products) {
                    decimal item_price = order.PricePaid[item.Key] * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName} x{item.Value} - {item_price:c}");
                }
                Console.WriteLine($"Total: {total_price:c}");
                Console.WriteLine();
            }
        }

        public void PrintOrderHistory(Location location) {
            ICollection<Order> orders = Store.GetLocationOrders(location);
            Console.WriteLine();
            foreach (var order in orders) {
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                decimal total_price = 0;
                foreach (var item in order.Products) {
                    decimal item_price = order.PricePaid[item.Key] * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName} x{item.Value} - {item_price:c}");
                }
                Console.WriteLine($"Total: {total_price:c}");
                Console.WriteLine();
            }
        }

        public void LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer, out bool exit) {
            exit = false;
            Location location = customer.CurrentLocation;
            Console.WriteLine($"\nSigned in as {customer.FirstName} {customer.LastName}, shopping at {location.Name}.");
            Console.WriteLine("Select an item you would like to purchase. Enter [cart] to view/modify your cart, or [leave] to abandon your cart.");
            int i = 0;
            foreach (Product item in location.Stock.Keys) {
                int stock = location.Stock[item];
                decimal price = location.Prices[item];
                Console.WriteLine($"[{i++}] {item.DisplayName}, {price:c} - {stock} in stock");
            }
            Console.WriteLine();
            string input = Console.ReadLine();
            var store_item = interpreter.ProductSelection(input, customer, out exit);
            if (store_item == null) {
                return;
            }
            Console.WriteLine($"\nHow many would you like to purchase? ({store_item.Value.Value} in stock). [cancel] to exit.");
            input = Console.ReadLine();
            interpreter.QuantitySelection(input, store_item, customer);
        }

        public void CheckoutPrompt(Order order) {
            Console.WriteLine("\nThank you for your purchase. Order details below:\n");
            Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
            decimal total_price = 0;
            foreach (var item in order.Products) {
                Store.UpdateLocationStock(order.Location, item.Key);
                decimal item_price = order.PricePaid[item.Key] * item.Value;
                total_price += item_price;
                Console.WriteLine($"{item.Key.DisplayName} x{item.Value} - {item_price:c}");
            }
            Store.Save();
            Console.WriteLine($"Total: {total_price:c}");
        }

        public bool? CartPrompt(IUserInputInterpreter interpreter, Customer customer, out bool checkout) {
            while (true) {
                Console.WriteLine("\nEnter [remove] to remove an item from your cart, [checkout] to complete your order, or [back] to continue shopping.\n");
                Console.WriteLine("Your Cart:");
                foreach (var item in customer.Cart) {
                    Console.WriteLine($"{item.Key.DisplayName} x{item.Value}");
                }
                string input = Console.ReadLine();
                interpreter.CartCommands(input, customer, Store, out bool exit, out checkout);
                if (exit) {
                    return true;
                }
            }
        }

        public void RemoveProductFromCartPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Product product = null;
            int qty = 0;
            while (product == null) {
                Console.WriteLine("\nSelect an item you would like to remove. [cancel] exit.");
                int i = 0;
                foreach (var item in customer.Cart) {
                    Console.WriteLine($"[{i++}] {item.Key.DisplayName} x {item.Value}");
                }
                string product_selection = Console.ReadLine();
                product = interpreter.ValidCustomerProduct(product_selection, customer, out bool exit);
                if (exit) {
                    return;
                }
            }

            while (qty > customer.Cart[product] || qty <= 0) {
                Console.WriteLine($"\nHow many would you like to remove? Currently holding: {customer.Cart[product]}. [cancel] exit.");
                string amt = Console.ReadLine();
                qty = interpreter.ParseQuantity(amt, out bool exit);
                if (exit) {
                    return;
                }
            }
            customer.RemoveFromCart(product, qty);
        }
    }
}
