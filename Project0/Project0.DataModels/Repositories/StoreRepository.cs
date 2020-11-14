using Project0.DataModels.Entities;
using Project0.Library.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project0.DataModels.Repositories {
    public class StoreRepository : IStoreRepository {
        // TODO: Implement IStoreRepository interface
        private readonly Project0Context _dbContext;

        public StoreRepository(Project0Context context) {
            _dbContext = context ?? throw new ArgumentNullException("Null Context.");
        }

        public void AddCustomer(Library.Models.Customer customer) {
            throw new NotImplementedException();
        }

        public void AddLocation(Library.Models.Location location) {
            throw new NotImplementedException();
        }

        public void AddOrder(Library.Models.Order order) {
            throw new NotImplementedException();
        }

        public void AddProduct(Library.Models.Product product) {
            throw new NotImplementedException();
        }

        public Library.Models.Customer GetCustomerById(int id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Library.Models.Customer> GetCustomers() {
            throw new NotImplementedException();
        }

        public Library.Models.Location GetLocationById(int id) {
            throw new NotImplementedException();
        }

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

        public void Save() {
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
