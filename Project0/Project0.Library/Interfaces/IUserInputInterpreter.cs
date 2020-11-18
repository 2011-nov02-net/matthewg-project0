using Project0.Library.Models;
using System.Collections.Generic;

namespace Project0.Library.Interfaces {
    public interface IUserInputInterpreter {

        IUserPrompts Prompts { get; }

        IUser ValidUserID(string s, IStoreRepository store);

        string[] ParseNewCustomer(string s, out bool exit);

        Customer RegisterCustomer(string[] details, IStoreRepository store);

        void CustomerOperations(string s, Customer customer, out bool exit);

        Location ValidLocation(string s, IStoreRepository store, Customer customer, out bool exit);

        Product ValidProduct(string s, IStoreRepository store, out bool exit);

        Product ValidCustomerProduct(string s, Customer customer, out bool exit);

        void AdminOperations(string s, out bool exit);

        void ValidOrderHistoryOption(string s, out bool exit);

        bool GenerateLocation(string s, IStoreRepository store, out bool exit);

        bool RestockLocation(IStoreRepository store, Location location, Product product, int qty);

        decimal ParsePrice(string s, out bool exit);

        int ParseQuantity(string s, out bool exit);

        bool GenerateProduct(string s, IStoreRepository store);

        ICollection<Customer> UserLookup(string s, IStoreRepository store, out bool exit);

        Customer CustomerEmailLookup(string s, IStoreRepository store, out bool exit);

        KeyValuePair<Product, int>? ProductSelection(string s, Customer customer, out bool exit);

        void QuantitySelection(string s, KeyValuePair<Product, int>? purchase_item, Customer customer);

        void CartCommands(string s, Customer customer, IStoreRepository store, out bool exit, out bool checkout);
    }
}
