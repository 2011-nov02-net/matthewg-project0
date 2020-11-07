using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Customer : IUser {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; }
        public IDictionary<IProduct, int> Cart { get; set; }
        public ILocation CurrentLocation { get; set; }

        public Customer(string first_name, string last_name, string id) {
            FirstName = first_name;
            LastName = last_name;
            Id = id;
            Cart = new Dictionary<IProduct, int>();
            CurrentLocation = null;
        }

        public bool AddToCart(IProduct product, int qty) {
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

        public bool RemoveFromCart(IProduct product, int qty) {
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

        public bool PlaceOrder() {
            return CurrentLocation.PlaceOrder(this);
        }

        public void EmptyCart() {
            foreach (var product in Cart.Keys) {
                RemoveFromCart(product, Cart[product]);
            }
        }

        public void NewCart() {
            Cart = new Dictionary<IProduct, int>();
        }

        public void LeaveStore() {
            EmptyCart();
            CurrentLocation = null;
        }
    }
}
