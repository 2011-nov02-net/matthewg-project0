using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IUserInputInterpreter {

        IUserPrompts Prompts { get; }

        IUser ValidUserID(string s, IStoreRepository store);

        string[] ParseNewCustomer(string s);

        Customer RegisterCustomer(string[] details, IStoreRepository store);

        bool? ValidLocation(string s, IStoreRepository store, Customer customer, out Location location);

        bool? ValidProduct(string s, IStoreRepository store, out Product product);

        bool? ValidAdminCommand(string s, IStoreRepository store);

        bool GenerateLocation(string s, IStoreRepository store);

        bool RestockLocation(IStoreRepository store, Location location, Product product, int qty);

        bool GenerateProduct(string s, IStoreRepository store);

        ICollection<Customer> UserLookup(string s, IStoreRepository store);

        KeyValuePair<Product, int>? ProductSelection(string s, Customer customer, out int exit_status);

        bool QuantitySelection(string s, KeyValuePair<Product, int>? purchase_item, Customer customer);

        bool? CartCommands(string s, Customer customer, IStoreRepository store);
    }
}
