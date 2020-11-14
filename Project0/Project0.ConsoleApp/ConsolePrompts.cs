using Project0.Library;
using Project0.Library.Interfaces;
using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.ConsoleApp {
    public class ConsolePrompts : IUserPrompts {

        public IStore Store { get; }

        public ConsolePrompts(IStore store) {
            Store = store;
        }

        public void WelcomeMessage() {
            Console.WriteLine("Welcome to Matt's store.\n");
        }

        public IUser StartupPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter your email address or 'register' if you are a new customer. Enter [Q] to quit.");
            string input = Console.ReadLine();
            return interpreter.ValidUserID(input, Store);
        }

        public IUser RegisterPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("Enter your first and last name separated by a space.");
            string input = Console.ReadLine();

            string[] name = interpreter.ParseName(input);
            if (name == null) {
                Console.WriteLine("Invalid name.");
                return null;
            }

            Console.WriteLine("Enter your email address.");
            input = Console.ReadLine();
            var new_customer = interpreter.RegisterCustomer(name, input, Store);
            if (new_customer == null) {
                Console.WriteLine("Invalid email address or address is already in use.");
                return null;
            }
            Console.WriteLine($"Welcome {new_customer.FirstName} {new_customer.LastName}.\n" +
                $"Your user account has been created. Please use your email, {new_customer.Email} to sign in from now on.\n");
            return new_customer;
        }

        public void ReturningCustomerPrompt(IUser customer) {
            Console.WriteLine($"Welcome back, {customer.FirstName} {customer.LastName}.");
        }

        public bool? StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Console.WriteLine("Please select the store location you are visiting.");
            int i = 0;
            foreach (var loc in Store.Locations) {
                Console.WriteLine($"[{i++}] {loc.Name}");
            }
            Console.WriteLine("[history] View order history");
            Console.WriteLine("[logout] Exit session.");
            string input = Console.ReadLine();
            return interpreter.ValidLocation(input, Store, customer, out _);
        }

        public bool? AdminPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("[0] Add new store location");
            Console.WriteLine("[1] Restock stores");
            Console.WriteLine("[2] Add new product");
            Console.WriteLine("[3] Search users");
            Console.WriteLine("[4] View order history");
            Console.WriteLine("[logout] Exit session.");
            string input = Console.ReadLine();
            return interpreter.ValidAdminCommand(input, Store);
        }

        public bool NewStoreLocation(IUserInputInterpreter interpreter) {
            Console.WriteLine("Enter the name of the new location.");
            string input = Console.ReadLine();
            bool success = interpreter.GenerateLocation(input, Store);
            if (success) {
                Console.WriteLine($"Successfully created new store location: {input}.");
            } else {
                Console.WriteLine("Failed to create store location.");
            }
            return success;
        }

        public bool RestockPrompt(IUserInputInterpreter interpreter) {
            int i;
            string input;

            ILocation location;
            Product product;
            int qty;
            while (true) {
                Console.WriteLine("Choose a location:");
                i = 0;
                foreach (var loc in Store.Locations) {
                    Console.WriteLine($"[{i++}] {loc.Name}");
                }
                Console.WriteLine("[cancel] EXIT");
                input = Console.ReadLine();
                bool? response = interpreter.ValidLocation(input, Store, null, out location);
                if (response == null) {
                    return true;
                }
                if (response == false) {
                    break;
                }
            }

            while (true) {
                Console.WriteLine("Choose a product:");
                i = 0;
                foreach (var pro in Store.Products) {
                    Console.WriteLine($"[{i++}] {pro.DisplayName}");
                }
                Console.WriteLine("[cancel] EXIT");
                input = Console.ReadLine();
                bool? response = interpreter.ValidProduct(input, Store, out product);
                if (response == null) {
                    return true;
                }
                if (response == false) {
                    break;
                }
            }

            while (true) {
                Console.WriteLine("Enter a quantity to add. [cancel] to exit.");
                input = Console.ReadLine();
                if (input.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
                try {
                    qty = int.Parse(input);
                } catch (Exception) { continue; }
                if (qty > 0) {
                    break;
                }
            }
            return interpreter.RestockLocation(Store, location, product, qty);
        }

        public bool NewProductPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("Enter product name. [cancel] to exit.");
            string product_name = Console.ReadLine();
            double price;
            if (product_name.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
            while (true) {
                Console.WriteLine("Enter price. [cancel] to exit.");
                string price_input = Console.ReadLine();
                if (price_input.Equals("cancel", StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
                try {
                    price = double.Parse(price_input);
                } catch (Exception) { continue; }
                if (price >= 0) {
                    break;
                }
            }
            return interpreter.GenerateProduct(product_name, Store, price);
        }

        public bool UserLookupPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("Enter a user's name.");
            string input = Console.ReadLine();
            ICollection<IUser> users = interpreter.UserLookup(input, Store);
            foreach (var user in users) {
                Console.WriteLine($"{user.LastName}, {user.FirstName} - {user.Email}");
            }
            Console.WriteLine();
            return true;
        }

        public bool PrintOrderHistory(Customer customer) {
            ICollection<IOrder> orders;
            if (customer == null) {
                orders = Store.OrderHistory;
            } else {
                orders = Store.SearchOrderHistoryByCustomer(customer);
            }
            Console.WriteLine();
            foreach (var order in orders) {
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                double total_price = 0;
                foreach (var item in order.Products) {
                    double item_price = item.Key.Price * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName} x{item.Value} - {item_price:c}");
                }
                Console.WriteLine($"Total: {total_price:c}");
                Console.WriteLine();
            }
            return true;
        }

        public bool? LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Console.WriteLine($"\nSigned in as {customer.FirstName} {customer.LastName}, shopping at {customer.CurrentLocation.Name}.");
            Console.WriteLine("Select an item you would like to purchase. Enter [cart] to view/modify your cart, or [leave] to abandon your cart.");
            int i = 0;
            foreach (var item in customer.CurrentLocation.Stock) {
                Console.WriteLine($"[{i++}] {item.Key.DisplayName}, {item.Key.Price:c} - {item.Value} in stock");
            }
            Console.WriteLine();
            string input = Console.ReadLine();
            var store_item = interpreter.ProductSelection(input, customer, out int exit_status);
            if (exit_status == 1) {
                return null;
            }
            if (exit_status == 2) {
                return false;
            }
            if (store_item == null) {
                return true;
            }
            Console.WriteLine($"How many would you like to purchase? ({store_item.Value.Value} in stock)");
            input = Console.ReadLine();
            return interpreter.QuantitySelection(input, store_item, customer);
        }

        public void CheckoutPrompt(IOrder order) {
            Console.WriteLine("\nThank you for your purchase. Order details below:\n");
            Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
            double total_price = 0;
            foreach (var item in order.Products) {
                double item_price = item.Key.Price * item.Value;
                total_price += item_price;
                Console.WriteLine($"{item.Key.DisplayName} x{item.Value} - {item_price:c}");
            }
            Console.WriteLine($"Total: {total_price:c}");
        }

        public bool? CartPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Console.WriteLine("\nEnter [remove] to remove an item from your cart, [checkout] to complete your order, or [back] to continue shopping.\n");
            Console.WriteLine("Your Cart:");
            foreach (var item in customer.Cart) {
                Console.WriteLine($"{item.Key.DisplayName} x{item.Value}");
            }
            string input = Console.ReadLine();
            return interpreter.CartCommands(input, customer);
        }
    }
}
