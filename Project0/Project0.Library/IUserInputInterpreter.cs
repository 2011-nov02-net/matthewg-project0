using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library {
    public interface IUserInputInterpreter {

        IUserPrompts Prompts { get; }

        IUser ValidUserID(string s, IStore store);

        Customer RegisterCustomer(string s, IStore store);

        bool? ValidLocation(string s, IStore store, Customer customer, out ILocation location);

        bool? ValidProduct(string s, IStore store, out IProduct product);

        bool? ValidAdminCommand(string s, IStore store);

        bool GenerateLocation(string s, IStore store);

        bool RestockLocation(IStore store, ILocation location, IProduct product, int qty);

        bool GenerateProduct(string s, IStore store, double price);

        KeyValuePair<IProduct, int>? ProductSelection(string s, Customer customer, out int exit_status);

        bool QuantitySelection(string s, KeyValuePair<IProduct, int>? purchase_item, Customer customer);

        bool? CartCommands(string s, Customer customer);
    }
}
