using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Project0.UnitTests {
    public class BusinessTests {

        [Fact]
        public void CartAddNewProduct() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.AddToCart(product, 1);

            Assert.Contains(product, customer.Cart.Keys);
            Assert.Equal(1, customer.Cart[product]);
        }

        [Fact]
        public void CartAddMoreOfProduct() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.AddToCart(product, 1);
            customer.AddToCart(product, 2);

            Assert.Contains(product, customer.Cart.Keys);
            Assert.Equal(3, customer.Cart[product]);
        }

        [Fact]
        public void CartRemoveLastOfProduct() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.Cart.Add(product, 1);
            customer.RemoveFromCart(product, 1);

            Assert.DoesNotContain(product, customer.Cart.Keys);
        }

        [Fact]
        public void CartRemoveSomeOfProduct() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.Cart.Add(product, 5);
            customer.RemoveFromCart(product, 2);

            Assert.Contains(product, customer.Cart.Keys);
            Assert.Equal(3, customer.Cart[product]);
        }

        [Fact]
        public void CartIsEmpty() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.Cart.Add(product, 1);
            customer.EmptyCart();

            Assert.Empty(customer.Cart);
        }

        [Fact]
        public void LocationStockAddNewProduct() {
            Product product = new Product();
            Location location = new Location();

            location.AddStock(product, 1);

            Assert.Contains(product, location.Stock.Keys);
            Assert.Equal(1, location.Stock[product]);
        }

        [Fact]
        public void LocationStockAddMoreOfProduct() {
            Product product = new Product();
            Location location = new Location();

            location.AddStock(product, 1);
            location.AddStock(product, 2);

            Assert.Contains(product, location.Stock.Keys);
            Assert.Equal(3, location.Stock[product]);
        }

        [Fact]
        public void LocationStockRemoveProduct() {
            Product product = new Product();
            Location location = new Location();
            location.Stock.Add(product, 10);

            location.AddStock(product, -5);

            Assert.Equal(5, location.Stock[product]);
        }

        [Fact]
        public void LocationStockRemoveTooMuchProduct() {
            Product product = new Product();
            Location location = new Location();
            location.Stock.Add(product, 10);

            location.AddStock(product, -15);

            Assert.Equal(10, location.Stock[product]);
        }

        [Fact]
        public void LocationStockChangesWithCartAdd() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.AddToCart(product, 2);

            Assert.Equal(8, location.Stock[product]);
        }

        [Fact]
        public void LocationStockChangesWithCartRemove() {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);
            customer.Cart.Add(product, 2);

            customer.RemoveFromCart(product, 2);

            Assert.Equal(12, location.Stock[product]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CartDoesNotAddWithInvalidInput(int qty) {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            location.Stock.Add(product, 10);

            customer.AddToCart(product, qty);

            Assert.Empty(customer.Cart);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public void CartDoesNotRemoveWithInvalidInput(int qty) {
            Customer customer = new Customer();
            Product product = new Product();
            Location location = new Location();
            customer.CurrentLocation = location;
            customer.Cart.Add(product, 5);

            customer.RemoveFromCart(product, qty);

            Assert.Equal(5, customer.Cart[product]);
        }
    }
}
