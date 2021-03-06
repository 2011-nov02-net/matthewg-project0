﻿using Microsoft.EntityFrameworkCore;
using Project0.DataModels.Entities;
using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

// TODO:
//      unit testing for database?

namespace Project0.DataModels.Repositories {
    public class StoreRepository : IStoreRepository {
        private readonly Project0Context _dbContext;

        public StoreRepository(Project0Context context) {
            _dbContext = context ?? throw new ArgumentNullException("Null Context.");
        }

        //
        // Adding Methods
        //

        /// <summary>
        /// Add a new customer to the database
        /// </summary>
        /// <param name="customer">Business-Model customer object</param>
        public void AddCustomer(Library.Models.Customer customer) {
            if (customer.Id != 0) { // IDs are assigned by database, so it already exists if not 0
                throw new ArgumentException("Customer already exists.");
            }

            var dbCustomer = new Customer() {
                FirstName = customer.FirstName,
                LastName =  customer.LastName,
                Email = customer.Email
            };

            _dbContext.Customers.Add(dbCustomer);
        }

        /// <summary>
        /// Add a new store location to the database
        /// </summary>
        /// <param name="location">Business-Model location object</param>
        public void AddLocation(Library.Models.Location location) {
            if (location.Id != 0) { // IDs are assigned by database, so it already exists if not 0
                throw new ArgumentException("Location already exists.");
            }

            var dbLocation = new Location() {
                Name = location.Name,
                Address = location.Address,
                City = location.City,
                State = location.State,
                Country = location.Country,
                PostalCode = location.Zip,
                Phone = location.Phone
            };

            _dbContext.Locations.Add(dbLocation);
        }

        /// <summary>
        /// Add a new order, along with its product dictionary, to the database
        /// </summary>
        /// <param name="order">Business-Model order object</param>
        public void AddOrder(Library.Models.Order order) {
            if (order.Id != 0) { // IDs are assigned by database, so it already exists if not 0
                throw new ArgumentException("Order already exists.");
            }

            var customer = _dbContext.Customers.First(c => c.Id == order.Customer.Id);
            var location = _dbContext.Locations.First(l => l.Id == order.Location.Id);
            var dbOrder = new Order() {
                Customer = customer,
                Location = location,
                Date = order.Time
            };

            _dbContext.Orders.Add(dbOrder);

            foreach (var item in order.Products) {
                var product = _dbContext.Products.First(p => p.Id == item.Key.Id);

                var dbOrderContent = new OrderContent() {
                    Order = dbOrder,
                    Product = product,
                    Quantity = item.Value,
                    Price = order.Location.Prices[item.Key]
                };

                _dbContext.OrderContents.Add(dbOrderContent);
            }
        }

        /// <summary>
        /// Add a new product to the database
        /// </summary>
        /// <param name="product">Business-Model product object</param>
        public void AddProduct(Library.Models.Product product) {
            if (product.Id != 0) { // IDs are assigned by database, so it already exists if not 0
                throw new ArgumentException("Product already exists.");
            }

            var dbProduct = new Product() {
                Name = product.DisplayName
            };

            _dbContext.Products.Add(dbProduct);
        }

        //
        // Fetching Methods
        //

        /// <summary>
        /// Retrieve Business-Model Customer object from database via customer id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>Business-Model customer object</returns>
        public Library.Models.Customer GetCustomerById(int id) {
            var dbCustomer = _dbContext.Customers.First(c => c.Id == id);

            return new Library.Models.Customer() {
                Id = dbCustomer.Id,
                FirstName = dbCustomer.FirstName,
                LastName = dbCustomer.LastName,
                Email = dbCustomer.Email
            };
        }

        /// <summary>
        /// Retrieve Business-Model Customer object from database via customer email address
        /// </summary>
        /// <param name="id">Customer email</param>
        /// <returns>Business-Model customer object</returns>
        public Library.Models.Customer GetCustomerByEmail(string s) {
            var dbCustomer = _dbContext.Customers.First(c => c.Email == s);

            return new Library.Models.Customer() {
                Id = dbCustomer.Id,
                FirstName = dbCustomer.FirstName,
                LastName = dbCustomer.LastName,
                Email = dbCustomer.Email
            };
        }

        /// <summary>
        /// Retrieve all customers in the database
        /// </summary>
        /// <returns>A group of Business-Model customer objects</returns>
        public IEnumerable<Library.Models.Customer> GetCustomers() {
            var dbCustomers = _dbContext.Customers.ToList();
            return dbCustomers.Select(c => new Library.Models.Customer() {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }).ToList();
        }

        /// <summary>
        /// Retrieve all customers with a given name
        /// </summary>
        /// <param name="firstName">Customer's first name</param>
        /// <param name="lastName">Customer's last name</param>
        /// <returns>A collection of Business-Model customer objects</returns>
        public ICollection<Library.Models.Customer> GetCustomersByName(string firstName, string lastName) {
            var dbCustomers = _dbContext.Customers.ToList();
            return dbCustomers.Select(c => new Library.Models.Customer() {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email
            }).Where(c => c.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase)
                        && c.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Retrieve Business-Model location object from database via location id
        /// </summary>
        /// <param name="id">Location id</param>
        /// <returns>Business-Model location object</returns>
        public Library.Models.Location GetLocationById(int id) {
            var dbLocation = _dbContext.Locations
                .Include(l => l.LocationInventories)
                    .ThenInclude(li => li.Product)
                .First(l => l.Id == id);

            Dictionary<Library.Models.Product, int> stock = new Dictionary<Library.Models.Product, int>();
            Dictionary<Library.Models.Product, decimal> prices = new Dictionary<Library.Models.Product, decimal>();
            foreach (LocationInventory li in dbLocation.LocationInventories) {
                Library.Models.Product product = new Library.Models.Product() { Id = li.ProductId, DisplayName = li.Product.Name };
                stock.Add(product, li.Stock);
                prices.Add(product, li.Price);
            }

            return new Library.Models.Location(
                dbLocation.Name,
                dbLocation.Address,
                dbLocation.City,
                dbLocation.State,
                dbLocation.Country,
                dbLocation.PostalCode,
                dbLocation.Phone
                ) { Id = dbLocation.Id, Stock = stock, Prices = prices };
        }

        /// <summary>
        /// Retrieve all locations in the database
        /// </summary>
        /// <returns>A list of Business-Model location objects</returns>
        public List<Library.Models.Location> GetLocations() {
            var dbLocations = _dbContext.Locations.ToList();
            List<Library.Models.Location> locations = new List<Library.Models.Location>();
            foreach (var location in dbLocations) {
                locations.Add(GetLocationById(location.Id));
            }
            return locations;
        }

        /// <summary>
        /// Retrieve Business-Model order object from database via order id
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>Business-Model order object</returns>
        public Library.Models.Order GetOrderById(int id) {
            var dbOrder = _dbContext.Orders
                .Include(o => o.Location)
                .Include(o => o.Customer)
                .Include(o => o.OrderContents)
                    .ThenInclude(oc => oc.Product)
                .First(o => o.Id == id);

            var location = new Library.Models.Location(
                dbOrder.Location.Name,
                dbOrder.Location.Address,
                dbOrder.Location.City,
                dbOrder.Location.State,
                dbOrder.Location.Country,
                dbOrder.Location.PostalCode,
                dbOrder.Location.Phone
                ) { Id = dbOrder.LocationId };
            var customer = new Library.Models.Customer() {
                Id = dbOrder.CustomerId,
                FirstName = dbOrder.Customer.FirstName,
                LastName = dbOrder.Customer.LastName,
                Email = dbOrder.Customer.Email
            };

            Dictionary<Library.Models.Product, int> products = new Dictionary<Library.Models.Product, int>();
            Dictionary<Library.Models.Product, decimal> prices = new Dictionary<Library.Models.Product, decimal>();
            foreach (OrderContent oc in dbOrder.OrderContents) {
                Library.Models.Product product = new Library.Models.Product() { Id = oc.ProductId, DisplayName = oc.Product.Name };
                products.Add(product, oc.Quantity);
                prices.Add(product, oc.Price);
            }

            return new Library.Models.Order() { Id = dbOrder.Id, Products = products, PricePaid = prices, Customer = customer, Location = location, Time = dbOrder.Date };
        }  

        /// <summary>
        /// Retrieve all orders in the database
        /// </summary>
        /// <returns>A group of Business-Model order objects</returns>
        public List<Library.Models.Order> GetOrders() {
            var dbOrders = _dbContext.Orders.OrderByDescending(o => o.Date).ToList();
            List<Library.Models.Order> orders = new List<Library.Models.Order>();
            foreach (var order in dbOrders) {
                orders.Add(GetOrderById(order.Id));
            }
            return orders;
        }

        /// <summary>
        /// Retrieve all orders in the database submitted by a particular customer
        /// </summary>
        /// <returns>A list of Business-Model order objects</returns>
        public List<Library.Models.Order> GetCustomerOrders(Library.Models.Customer customer) {
            var dbOrders = _dbContext.Orders
                .Where(o => o.CustomerId == customer.Id)
                .OrderByDescending(o => o.Date).ToList();
            List<Library.Models.Order> orders = new List<Library.Models.Order>();
            foreach (var order in dbOrders) {
                orders.Add(GetOrderById(order.Id));
            }
            return orders;
        }

        /// <summary>
        /// Retrieve all orders in the database submitted from a particular location
        /// </summary>
        /// <returns>A group of Business-Model order objects</returns>
        public List<Library.Models.Order> GetLocationOrders(Library.Models.Location location) {
            var dbOrders = _dbContext.Orders
                .Where(o => o.LocationId == location.Id)
                .OrderByDescending(o => o.Date).ToList();
            List<Library.Models.Order> orders = new List<Library.Models.Order>();
            foreach (var order in dbOrders) {
                orders.Add(GetOrderById(order.Id));
            }
            return orders;
        }

        /// <summary>
        /// Retrieve Business-Model product object from database via product id
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns>Business-Model product object</returns>
        public Library.Models.Product GetProductById(int id) {
            var dbProduct = _dbContext.Products.First(p => p.Id == id);

            return new Library.Models.Product() {
                Id = dbProduct.Id,
                DisplayName = dbProduct.Name
            };
        }

        /// <summary>
        /// Retrieve all products in the database
        /// </summary>
        /// <returns>A list of Business-Model product objects</returns>
        public List<Library.Models.Product> GetProducts() {
            var dbProducts = _dbContext.Products.ToList();
            return dbProducts.Select(p => new Library.Models.Product() {
                Id = p.Id,
                DisplayName = p.Name
            }).ToList();
        }

        //
        // Deletion Methods
        //

        public void RemoveCustomer(Library.Models.Customer customer) {
            throw new NotImplementedException();
        }

        public void RemoveLocation(Library.Models.Location location) {
            throw new NotImplementedException();
        }

        public void RemoveOrder(Library.Models.Order order) {
            throw new NotImplementedException();
        }

        public void RemoveProduct(Library.Models.Product product) {
            throw new NotImplementedException();
        }

        //
        // Update Methods
        //

        /// <summary>
        /// Update the amount of some product that a particular location has in stock
        /// </summary>
        /// <param name="location">Business-Model location object</param>
        /// <param name="product">Business-Model product object</param>
        /// <param name="qty">integer number of items to add</param>
        public void UpdateLocationStock(Library.Models.Location location, Library.Models.Product product) {
            LocationInventory dbLocationInventory;
            try {
                dbLocationInventory = _dbContext.LocationInventories.First(x => x.LocationId == location.Id && x.ProductId == product.Id);
                dbLocationInventory.Stock = location.Stock[product];
            } catch (InvalidOperationException) {
                var dbLocation = _dbContext.Locations.First(l => l.Id == location.Id);
                var dbProduct = _dbContext.Products.First(p => p.Id == product.Id);
                dbLocationInventory = new LocationInventory() {
                    Location = dbLocation,
                    Product = dbProduct,
                    Stock = location.Stock[product],
                    Price = location.Prices[product]
                };
                _dbContext.LocationInventories.Add(dbLocationInventory);
            }
        }

        /// <summary>
        /// Update the details of a customer
        /// </summary>
        /// <param name="customer">Business-Model customer object</param>
        /// <param name="firstName">new first name, remains unchanged if null</param>
        /// <param name="lastName">new last name, remains unchanged if null</param>
        /// <param name="email">new email address, remains unchanged if null</param>
        public void UpdateCustomer(Library.Models.Customer customer, string firstName, string lastName, string email) {
            var dbCustomer = _dbContext.Customers.First(c => c.Id == customer.Id);
            dbCustomer.FirstName = firstName ?? dbCustomer.FirstName;
            dbCustomer.LastName = lastName ?? dbCustomer.LastName;
            dbCustomer.Email = email ?? dbCustomer.Email;
        }

        public void UpdateLocation(Library.Models.Location location) {
            throw new NotImplementedException();
        }

        public void UpdateOrder(Library.Models.Order order) {
            throw new NotImplementedException();
        }

        public void UpdateProduct(Library.Models.Product product) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Commit transaction to the database
        /// </summary>
        public void Save() {
            try {
                _dbContext.SaveChanges();
            } catch (DbUpdateException) {
                var transaction = _dbContext.ChangeTracker.Entries()
                    .Where(x => x.State == EntityState.Added ||
                                x.State == EntityState.Deleted ||
                                x.State == EntityState.Modified).ToList();

                foreach (var entry in transaction) {
                    entry.State = EntityState.Detached;
                }
                throw;
            }
        }
    }
}
