using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class StandardStoreLocation : ILocation {

        private IStore _store;
        public string Name { get; }
        public IDictionary<int, int> Stock { get; }

        public StandardStoreLocation(string name, IStore store) {
            Name = name;
            _store = store;
            Stock = new Dictionary<int, int>();
        }

        public bool PlaceOrder(Customer customer) {
            return _store.PlaceOrder(new Order(this, customer, DateTime.Now));
        }

        public bool AddStock(int product_id, int qty) {
            if (Stock.ContainsKey(product_id)) {
                Stock[product_id] += qty;
                return true;
            } else if (qty > 0) {
                Stock.Add(product_id, qty);
                return true;
            }
            return false;
        }

    }
}
