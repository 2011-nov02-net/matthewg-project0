using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Project0.Library.Models {
    public class Customer : IUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Dictionary<IProduct, int> Cart { get; set; }
        public ILocation CurrentLocation { get; set; }
        public IOrder LastOrder { get; set; }

        public Customer() { }

        public Customer(string first_name, string last_name, string email) {
            FirstName = first_name;
            LastName = last_name;
            Email = email;
            Cart = new Dictionary<IProduct, int>();
            CurrentLocation = null;
        }

        public bool AddToCart(IProduct product, int qty) {
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

        public IOrder PlaceOrder() {
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
