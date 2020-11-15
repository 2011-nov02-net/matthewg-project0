using Project0.Library.Models;
using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IStoreRepository {
        List<Location> GetLocations();
        IEnumerable<Customer> GetCustomers();
        ICollection<Customer> GetCustomersByName(string firstName, string lastName);
        ICollection<Order> GetOrders();
        List<Product> GetProducts();
        Location GetLocationById(int id);
        Customer GetCustomerById(int id);
        Customer GetCustomerByEmail(string s);
        Order GetOrderById(int id);
        Product GetProductById(int id);
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        void RemoveLocation(Location location);
        void AddCustomer(Customer customer);
        void UpdateCustomer(Customer customer, string firstName, string lastName, string email);
        void RemoveCustomer(Customer customer);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void RemoveOrder(Order order);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void RemoveProduct(Product product);
        void UpdateLocationStock(Location location, Product product);
        List<Order> GetCustomerOrders(Customer customer);
        IEnumerable<Order> GetLocationOrders(Location location);
        void Save();
    }
}
