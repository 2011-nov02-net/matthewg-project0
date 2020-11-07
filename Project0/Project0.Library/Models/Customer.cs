using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Customer : IUser {

        private static int _seed = 0;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; }
        public IDictionary<int, int> Cart { get; set; }
        public ILocation CurrentLocation { get; set; }

        public Customer(string first_name, string last_name) {
            FirstName = first_name;
            LastName = last_name;
            Id = $"{++_seed}";
            Cart = new Dictionary<int, int>();
            CurrentLocation = null;
        }

        public bool AddToCart(int product_id, int qty) {
            if (CurrentLocation.Stock.ContainsKey(product_id) && CurrentLocation.Stock[product_id] >= qty) {
                if (Cart.ContainsKey(product_id)) {
                    Cart[product_id] += qty;
                } else {
                    Cart.Add(product_id, qty);
                }
                CurrentLocation.AddStock(product_id, -qty);
                return true;
            }
            return false;
        }

        public bool RemoveFromCart(int product_id, int qty) {
            if (Cart.ContainsKey(product_id)) {
                if (Cart[product_id] > qty) {
                    Cart[product_id] -= qty;

                } else if (Cart[product_id] == qty) {
                    Cart.Remove(product_id);
                }
                CurrentLocation.AddStock(product_id, qty);
                return true;
            }
            return false;
        }

        public bool PlaceOrder() {
            return CurrentLocation.PlaceOrder(this);
        }

        public void EmptyCart() {
            foreach (int product_id in Cart.Keys) {
                RemoveFromCart(product_id, Cart[product_id]);
            }
        }

        public void NewCart() {
            Cart = new Dictionary<int, int>();
        }

        public void LeaveStore() {
            EmptyCart();
            CurrentLocation = null;
        }
    }
}
