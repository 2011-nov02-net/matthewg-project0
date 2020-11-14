using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IOrder {

        Dictionary<Product, int> Products { get; }
        Location Location { get; }
        Customer Customer { get; }
        DateTime Time { get; }
    }
}
