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
            Console.WriteLine("\nWelcome to Matt's Shopping App.\n");
        }

        public IUser StartupPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter your email address or 'register' if you are a new customer. Enter [Q] to quit.");
            string input = Console.ReadLine();
            return interpreter.ValidUserID(input, Store);
        }

        public IUser RegisterPrompt(IUserInputInterpreter interpreter, out bool exit) {
            Console.WriteLine("\nEnter your first name, last name, and email address - separated by whitespace, [0] Cancel.");
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
            Console.WriteLine($"\nWelcome back, {customer.FirstName} {customer.LastName}.");
        }

        public void EnterStoreLocationPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Location location = null;
            while (location == null) {
                Console.WriteLine("\nPlease select the store location you would like to order from.");
                Console.WriteLine("[0] Back to main menu.");
                int i = 1;
                foreach (var loc in Store.GetLocations()) {
                    Console.WriteLine($"[{i++}] {loc.Name} - {loc.Address}, {loc.City}, {loc.State}");
                }
                string input = Console.ReadLine();
                location = interpreter.ValidLocation(input, Store, customer, out bool exit);
                if (exit) {
                    return;
                }
            }
        }

        public void StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer, out bool exit) {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("[0] Logout");
            Console.WriteLine("[1] Place an order");
            Console.WriteLine("[2] View order history");
            string input = Console.ReadLine();
            interpreter.CustomerOperations(input, customer, out exit);
        }

        public void AdminPrompt(IUserInputInterpreter interpreter, out bool exit) {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("[0] Logout");
            Console.WriteLine("[1] Add new store location");
            Console.WriteLine("[2] Restock stores");
            Console.WriteLine("[3] Add new product");
            Console.WriteLine("[4] Search users");
            Console.WriteLine("[5] View order history");
            string input = Console.ReadLine();

            interpreter.AdminOperations(input, out exit);
        }

        public void OrderHistoryPrompt(IUserInputInterpreter interpreter) {
            bool exit = false;
            while (!exit) {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("[0] Cancel");
                Console.WriteLine("[1] All orders");
                Console.WriteLine("[2] Search a customer");
                Console.WriteLine("[3] Search a location");
                string input = Console.ReadLine();
                interpreter.ValidOrderHistoryOption(input, out exit);
            }
        }

        public void NewStoreLocation(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter the name of the new location. [0] Cancel.");
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
                Console.WriteLine("[0] Cancel");
                i = 1;
                foreach (var loc in Store.GetLocations()) {
                    Console.WriteLine($"[{i++}] {loc.Name}  - {loc.Address}, {loc.City}, {loc.State}");
                }
                input = Console.ReadLine();
                location = interpreter.ValidLocation(input, Store, null, out bool exit);
                if (exit) {
                    return;
                }
            }

            while (product == null) {
                Console.WriteLine("\nChoose a product:");
                Console.WriteLine("[0] Cancel");
                i = 1;
                foreach (var pro in Store.GetProducts()) {
                    Console.WriteLine($"[{i++}] {pro.DisplayName}");
                }
                input = Console.ReadLine();
                product = interpreter.ValidProduct(input, Store, out bool exit);
                if (exit) {
                    return;
                }
            }

            while (qty <= 0) {
                Console.WriteLine("\nEnter a quantity to add. [0] Cancel.");
                input = Console.ReadLine();
                if (input.Equals("0", StringComparison.OrdinalIgnoreCase)) {
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
                Console.WriteLine("\nEnter price. [0] Cancel.");
                string price_input = Console.ReadLine();
                price = interpreter.ParsePrice(price_input, out exit);
                if (exit) {
                    return 0;
                }
            }
            return price;
        }

        public void NewProductPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("\nEnter product name. [0] Cancel.");
            string product_name = Console.ReadLine();
            if (product_name.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                return;
            }
            interpreter.GenerateProduct(product_name, Store);
        }

        public void UserLookupPrompt(IUserInputInterpreter interpreter) {
            ICollection<Customer> users = null;
            while (users == null) {
                Console.WriteLine("\nEnter a user's first and last name. [0] Cancel.");
                string input = Console.ReadLine();
                users = interpreter.UserLookup(input, Store, out bool exit);
                if (exit) {
                    return;
                }
            }
            Console.WriteLine();
            foreach (var user in users) {
                Console.WriteLine($"{user.LastName}, {user.FirstName} - {user.Email}");
            }
            Console.WriteLine();
        }

        public Customer CustomerEmailEntry(IUserInputInterpreter interpreter) {
            Customer customer = null;
            while (customer == null) {
                Console.WriteLine("\nEnter customer's email address. [0] Cancel.");
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
                Console.WriteLine($"[0] Cancel");
                i = 1;
                foreach (var loc in Store.GetLocations()) {
                    Console.WriteLine($"[{i++}] {loc.Name}");
                }
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
                for (int i = 0; i < 120; i++) {
                    Console.Write("\u2500");
                }
                Console.WriteLine();
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                decimal total_price = 0;
                foreach (var item in order.Products) {
                    decimal item_price = order.PricePaid[item.Key] * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName,-20} x {item.Value,-5}-- {item_price,-5:c}");
                }
                for (int i = 0; i < 120; i++) {
                    Console.Write("\u2500");
                }
                Console.WriteLine();
                Console.WriteLine($"Total:{total_price,30:c}");
                Console.WriteLine();
            }
        }

        public void PrintOrderHistory(Customer customer) {
            ICollection<Order> orders = Store.GetCustomerOrders(customer);
            Console.WriteLine();
            foreach (var order in orders) {
                for (int i = 0; i < 120; i++) {
                    Console.Write("\u2500");
                }
                Console.WriteLine();
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                decimal total_price = 0;
                foreach (var item in order.Products) {
                    decimal item_price = order.PricePaid[item.Key] * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName,-20} x {item.Value,-5}-- {item_price,-5:c}");
                }
                for (int i = 0; i < 120; i++) {
                    Console.Write("\u2500");
                }
                Console.WriteLine();
                Console.WriteLine($"Total: {total_price,30:c}");
                Console.WriteLine();
            }
        }

        public void PrintOrderHistory(Location location) {
            ICollection<Order> orders = Store.GetLocationOrders(location);
            Console.WriteLine();
            foreach (var order in orders) {
                for (int i = 0; i < 120; i++) {
                    Console.Write("\u2500");
                }
                Console.WriteLine();
                Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
                decimal total_price = 0;
                foreach (var item in order.Products) {
                    decimal item_price = order.PricePaid[item.Key] * item.Value;
                    total_price += item_price;
                    Console.WriteLine($"{item.Key.DisplayName,-20} x {item.Value,-5}-- {item_price,-5:c}");
                }
                for (int i = 0; i < 120; i++) {
                    Console.Write("\u2500");
                }
                Console.WriteLine();
                Console.WriteLine($"Total: {total_price,30:c}");
                Console.WriteLine();
            }
        }

        public void LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer, out bool exit) {
            exit = false;
            Location location = customer.CurrentLocation;
            Console.WriteLine($"\nSigned in as {customer.FirstName} {customer.LastName}, shopping at {location.Name}.");
            while (!exit) {
                Console.WriteLine("Select an item you would like to purchase.");
                Console.WriteLine("[0] Leave and abandon cart");
                Console.WriteLine("[1] View/modify your cart");
                int i = 2;
                foreach (Product item in location.Stock.Keys) {
                    int stock = location.Stock[item];
                    decimal price = location.Prices[item];
                    Console.WriteLine($"[{i++}] {item.DisplayName}, {price:c} - {stock} in stock");
                }
                Console.WriteLine();
                string input = Console.ReadLine();
                var store_item = interpreter.ProductSelection(input, customer, out exit);
                if (store_item == null) {
                    continue;
                }
                Console.WriteLine($"\nHow many would you like to purchase? ({store_item.Value.Value} in stock). [0] to cancel purchase.");
                input = Console.ReadLine();
                interpreter.QuantitySelection(input, store_item, customer);
            }
        }

        public void CheckoutPrompt(Order order) {
            Console.WriteLine("\nThank you for your purchase. Order details below:\n");
            for (int i = 0; i < 120; i++) {
                Console.Write("\u2500");
            }
            Console.WriteLine($"{order.Time} - {order.Location.Name} - {order.Customer.Email}");
            decimal total_price = 0;
            foreach (var item in order.Products) {
                Store.UpdateLocationStock(order.Location, item.Key);
                decimal item_price = order.PricePaid[item.Key] * item.Value;
                total_price += item_price;
                Console.WriteLine($"\t{item.Key.DisplayName,-10} x {item.Value,-5} - {item_price,-5:c}");
            }
            Store.Save();
            for (int i = 0; i < 120; i++) {
                Console.Write("\u2500");
            }
            Console.WriteLine($"Total: {total_price,30:c}");
        }

        public void CartPrompt(IUserInputInterpreter interpreter, Customer customer, out bool checkout) {
            while (true) {
                Console.WriteLine("Your Cart:");
                foreach (var item in customer.Cart) {
                    Console.WriteLine($"-\t{item.Key.DisplayName} x {item.Value}");
                }
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("[0] Continue shopping");
                Console.WriteLine("[1] Remove an item from your cart");
                Console.WriteLine("[2] Checkout");
                string input = Console.ReadLine();
                interpreter.CartCommands(input, customer, Store, out bool exit, out checkout);
                if (exit) {
                    return;
                }
            }
        }

        public void RemoveProductFromCartPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Product product = null;
            int qty = 0;
            while (product == null) {
                Console.WriteLine("\nSelect an item you would like to remove.");
                Console.WriteLine("[0] Cancel removal");
                int i = 1;
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
                Console.WriteLine($"\nHow many would you like to remove? Currently holding: {customer.Cart[product]}. [0] Cancel removal.");
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
