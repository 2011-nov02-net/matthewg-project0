﻿using Project0.Library.Models;
using System.Collections.Generic;

namespace Project0.Library.Interfaces {
    public interface IStoreRepository {
        List<Location> GetLocations();
        IEnumerable<Customer> GetCustomers();
        ICollection<Customer> GetCustomersByName(string firstName, string lastName);
        List<Order> GetOrders();
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
        List<Order> GetLocationOrders(Location location);
        void Save();
    }
}
