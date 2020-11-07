using Project0.Library;
using Project0.Library.Models;
using System;
using System.Collections.Generic;
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
            string[] name = s.Split(" ");
            return store.AddCustomer(name[0], name[1]);
        }

        public bool? ValidLocation(string s, IStore store, Customer customer) {
            if (s.Equals("logout", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            int location_index;
            ILocation location;
            try {
                location_index = int.Parse(s);
            } catch (Exception) { return true; }
            if (location_index < store.Locations.Count && location_index >= 0) {
                location = store.Locations[location_index];
                customer.CurrentLocation = location;
                return false;
            }
            return true;
        }

        public bool? ValidAdminCommand(string s, IStore store) {
            if (s.Equals("logout", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            if (s.Equals("0", StringComparison.OrdinalIgnoreCase)) {
                return Prompts.NewStoreLocation(this);
            }
            return false;
        }

        public bool GenerateLocation(string s, IStore store) {
            return store.AddStandardLocation(s);
        }
    }
}
