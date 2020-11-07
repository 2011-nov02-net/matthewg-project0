using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface IOrder {

        IDictionary<int, int> Products { get; }
        ILocation Location { get; }
        Customer Customer { get; }
        DateTime Time { get; }
    }
}
