using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.ConsoleApp {
    public class ConsolePrompts {

        public static bool? StartupPrompt(IStore store) {
            Console.WriteLine("\nEnter your user ID or 'register' if you are a new customer. Enter [Q] to quit.");
            string input = Console.ReadLine();
            return InputInterpreter.ValidUserID(input, store);
        }

        public static void RegisterPrompt(IStore store) {
            Console.WriteLine("Enter your first and last name separated by a space.");
            string input = Console.ReadLine();
            var new_customer = InputInterpreter.RegisterCustomer(input, store);
            Console.WriteLine($"Welcome {new_customer.FirstName} {new_customer.LastName}.\n" +
                $"Your new user ID is {new_customer.Id}, use this to sign in from now on.");
        }

        public static void ReturningCustomerPrompt(IUser customer) {
            Console.WriteLine($"Welcome back, {customer.FirstName} {customer.LastName}.");
        }

        public static bool? StoreEntryPrompt(IStore store) {
            Console.WriteLine("Please select the store location you are visiting. Enter 'logout' to exit your session.");
            int i = 0;
            foreach (ILocation loc in store.Locations) {
                Console.WriteLine($"[{i++}] {loc.Name}");
            }
            string input = Console.ReadLine();
            return InputInterpreter.ValidLocation(input, store);
        }
    }
}
