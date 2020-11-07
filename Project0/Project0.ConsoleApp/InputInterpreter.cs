using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.ConsoleApp {
    public class InputInterpreter {

        public static bool? ValidUserID(string s, IStore store) {
            if (store.Customers.ContainsKey(s)) {
                ConsolePrompts.ReturningCustomerPrompt(store.Customers[s]);
                return false;
            }
            if (s.Equals("register", StringComparison.OrdinalIgnoreCase)) {
                ConsolePrompts.RegisterPrompt(store);
                return false;
            }
            if (s.Equals("q", StringComparison.OrdinalIgnoreCase)) {
                return null;
            }
            Console.WriteLine("Invalid input.");
            return true;
        }

        public static Customer RegisterCustomer(string s, IStore store) {
            string[] name = s.Split(" ");
            return store.AddCustomer(name[0], name[1]);
        }

        public static bool? ValidLocation(string s, IStore store) {
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

                return false;
            }
            return true;
        }
    }
}
