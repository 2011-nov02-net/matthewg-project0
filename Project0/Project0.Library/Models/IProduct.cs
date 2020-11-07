using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface IProduct {

        int Id { get; }
        string DisplayName { get; }
        double Price { get; }
    }
}
