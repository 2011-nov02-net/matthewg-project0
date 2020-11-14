using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IOrder {

        Dictionary<IProduct, int> Products { get; }
        ILocation Location { get; }
        Customer Customer { get; }
        DateTime Time { get; }
    }
}
