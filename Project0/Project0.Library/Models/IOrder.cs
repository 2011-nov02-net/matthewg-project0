using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface IOrder {

        IDictionary<IProduct, int> Products { get; }
        ILocation Location { get; }
        Customer Customer { get; }
        DateTime Time { get; }
    }
}
