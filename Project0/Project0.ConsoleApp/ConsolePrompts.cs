using Project0.Library;
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
            Console.WriteLine("\nEnter your user ID or 'register' if you are a new customer. Enter [Q] to quit.");
            string input = Console.ReadLine();
            return interpreter.ValidUserID(input, Store);
        }

        public IUser RegisterPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("Enter your first and last name separated by a space.");
            string pattern = "\\w\\s{1}\\w";
            string input = Console.ReadLine();
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, pattern)) {
                Console.WriteLine("Invalid name.");
                return null;
            }
            var new_customer = interpreter.RegisterCustomer(input, Store);
            Console.WriteLine($"Welcome {new_customer.FirstName} {new_customer.LastName}.\n" +
                $"Your new user ID is {new_customer.Id}, use this to sign in from now on.");
            return new_customer;
        }

        public void ReturningCustomerPrompt(IUser customer) {
            Console.WriteLine($"Welcome back, {customer.FirstName} {customer.LastName}.");
        }

        public bool? StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Console.WriteLine("Please select the store location you are visiting.");
            int i = 0;
            foreach (ILocation loc in Store.Locations) {
                Console.WriteLine($"[{i++}] {loc.Name}");
            }
            Console.WriteLine("[logout] Exit session.");
            string input = Console.ReadLine();
            return interpreter.ValidLocation(input, Store, customer);
        }

        public bool? AdminPrompt(IUserInputInterpreter interpreter) {
            Console.WriteLine("What would you like to do?\n[0] Add new store location\n[logout] Exit session.");
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

        public bool? LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer) {
            Console.WriteLine($"Signed in as {customer.FirstName} {customer.LastName}, shopping at {customer.CurrentLocation.Name}.");
            Console.WriteLine("Select an item you would like to purchase:");
            int i = 0;
            foreach (var item in customer.CurrentLocation.Stock) {
                Console.WriteLine($"[{i++}] {item.Key.DisplayName} - {item.Value} in stock");
            }
            string input = Console.ReadLine();
            return false;
        }
    }
}
