﻿using Project0.Library.Interfaces;
using System.Collections.Generic;

namespace Project0.Library.Models {
    public class Customer : IUser {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Dictionary<Product, int> Cart { get; set; }
        public Location CurrentLocation { get; set; }

        public Customer() {
            Cart = new Dictionary<Product, int>();
            CurrentLocation = null;
        }

        public Customer(string first_name, string last_name, string email) {
            FirstName = first_name;
            LastName = last_name;
            Email = email;
            Cart = new Dictionary<Product, int>();
            CurrentLocation = null;
        }

        /// <summary>
        /// Add a product and quantity to a customer's cart, subtracting it from the store's stock
        /// </summary>
        /// <param name="product">Product object to be added to the cart</param>
        /// <param name="qty">Integer amount to be added</param>
        /// <returns>True if product was successfully added to the cart. False if the quantity is less than 1, or if the store does not contain the product.</returns>
        public bool AddToCart(Product product, int qty) {
            if (qty < 1) {
                return false;
            }
            if (CurrentLocation.Stock.ContainsKey(product) && CurrentLocation.Stock[product] >= qty) {
                if (Cart.ContainsKey(product)) {
                    Cart[product] += qty;
                } else {
                    Cart.Add(product, qty);
                }
                CurrentLocation.AddStock(product, -qty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove an amount of some product from a customer's cart, adding it back to the store's stock
        /// </summary>
        /// <param name="product">Product object to be removed from the cart</param>
        /// <param name="qty">Integer amount to be removed</param>
        /// <returns>True if the product was successfully removed. False if the quantity is less than 1, or if the store does not contain the product.</returns>
        public bool RemoveFromCart(Product product, int qty) {
            if (qty < 1) {
                return false;
            }
            if (Cart.ContainsKey(product)) {
                if (Cart[product] > qty) {
                    Cart[product] -= qty;

                } else if (Cart[product] == qty) {
                    Cart.Remove(product);
                }
                CurrentLocation.AddStock(product, qty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove all products from cart.
        /// </summary>
        public void EmptyCart() {
            foreach (var product in Cart.Keys) {
                RemoveFromCart(product, Cart[product]);
            }
        }

        /// <summary>
        /// Set a brand new cart for the customer
        /// </summary>
        public void NewCart() {
            Cart = new Dictionary<Product, int>();
        }
    }
}
