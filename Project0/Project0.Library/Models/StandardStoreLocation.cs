using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class StandardStoreLocation : ILocation {

        private IStore _store;
        public string Name { get; }
        public IDictionary<IProduct, int> Stock { get; }

        public StandardStoreLocation(string name, IStore store) {
            Name = name;
            _store = store;
            Stock = new Dictionary<IProduct, int>();
        }

        public IOrder PlaceOrder(Customer customer) {
            return _store.PlaceOrder(new Order(this, customer, DateTime.Now));
        }

        public bool AddStock(IProduct product, int qty) {
            if (Stock.ContainsKey(product)) {
                Stock[product] += qty;
                return true;
            } else if (qty > 0) {
                Stock.Add(product, qty);
                return true;
            }
            return false;
        }

    }
}
