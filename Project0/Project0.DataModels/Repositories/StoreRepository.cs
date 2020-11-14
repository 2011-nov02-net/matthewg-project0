using Microsoft.EntityFrameworkCore;
using Project0.DataModels.Entities;
using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project0.DataModels.Repositories {
    public class StoreRepository : IStoreRepository {
        private readonly Project0Context _dbContext;

        public StoreRepository(Project0Context context) {
            _dbContext = context ?? throw new ArgumentNullException("Null Context.");
        }

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
                    Quantity = item.Value
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

        /// <summary>
        /// Retrieve Business-Model Customer object from database via customer id
        /// </summary>
        /// <param name="id">Customer id</param>
        /// <returns>Business-Model Customer object</returns>
        public Library.Models.Customer GetCustomerById(int id) {
            var dbCustomer = _dbContext.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderContents)
                
                .First(c => c.Id == id);

            var customer = new Library.Models.Customer() {
                Id = dbCustomer.Id,
                FirstName = dbCustomer.FirstName,
                LastName = dbCustomer.LastName,
                Email = dbCustomer.Email
            };

            foreach (Order o in dbCustomer.Orders) {
                Dictionary<Library.Models.Product, int> products = new Dictionary<Library.Models.Product, int>();
                foreach (OrderContent oc in o.OrderContents) {
                    Library.Models.Product product = new Library.Models.Product() { Id = oc.ProductId, DisplayName = oc.Product.Name };
                    products.Add(product, oc.Quantity);
                }
                Library.Models.Location location = new Library.Models.Location(o.Location.Name, o.Location.Address, o.Location.City, o.Location.State, o.Location.Country, o.Location.PostalCode, o.Location.Phone) { Id = o.LocationId };
                customer.Orders.Add(new Library.Models.Order() { Id = o.Id, Customer = customer, Products = products, Location = location, Time = o.Date});
            }
            return customer;
        }

        /// <summary>
        /// Retrieve all customers in the database
        /// </summary>
        /// <returns>A group of Business-Model customer objects</returns>
        public IEnumerable<Library.Models.Customer> GetCustomers() {
            var dbCustomers = _dbContext.Customers.ToList();
            List<Library.Models.Customer> customers = new List<Library.Models.Customer>();
            foreach (var customer in dbCustomers) {
                customers.Add(GetCustomerById(customer.Id));
            }
            return customers;
        }

        public Library.Models.Location GetLocationById(int id) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all locations in the database
        /// </summary>
        /// <returns>A group of Business-Model location objects</returns>
        public IEnumerable<Library.Models.Location> GetLocations() {
            var dbLocations = _dbContext.Locations.ToList();
            var locations = dbLocations.Select(l => new Library.Models.Location(l.Name, l.Address, l.City, l.State, l.Country, l.PostalCode, l.Phone)).ToList();

            return locations;
        }

        public Library.Models.Order GetOrderById(int id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Library.Models.Order> GetOrders() {
            throw new NotImplementedException();
        }

        public Library.Models.Product GetProductById(int id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Library.Models.Product> GetProducts() {
            throw new NotImplementedException();
        }

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
        /// <summary>
        /// Commit transaction to the database
        /// </summary>
        public void Save() {
            _dbContext.SaveChanges();
        }

        public void StockLocation(Library.Models.Location location, Library.Models.Product product, int qty) {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Library.Models.Customer customer) {
            throw new NotImplementedException();
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
    }
}
