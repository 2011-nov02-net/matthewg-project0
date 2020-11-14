using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IUserInputInterpreter {

        IUserPrompts Prompts { get; }

        IUser ValidUserID(string s, IStore store);

        string[] ParseName(string s);

        Customer RegisterCustomer(string[] name, string s, IStore store);

        bool? ValidLocation(string s, IStore store, Customer customer, out ILocation location);

        bool? ValidProduct(string s, IStore store, out Product product);

        bool? ValidAdminCommand(string s, IStore store);

        bool GenerateLocation(string s, IStore store);

        bool RestockLocation(IStore store, ILocation location, Product product, int qty);

        bool GenerateProduct(string s, IStore store, double price);

        ICollection<IUser> UserLookup(string s, IStore store);

        KeyValuePair<Product, int>? ProductSelection(string s, Customer customer, out int exit_status);

        bool QuantitySelection(string s, KeyValuePair<Product, int>? purchase_item, Customer customer);

        bool? CartCommands(string s, Customer customer);
    }
}
