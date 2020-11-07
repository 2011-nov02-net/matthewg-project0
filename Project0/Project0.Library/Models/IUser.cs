using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public interface IUser {

        string FirstName { get; set; }
        string LastName { get; set; }
        string Id { get; }
    }
}
