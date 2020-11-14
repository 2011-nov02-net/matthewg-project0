using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Interfaces {
    public interface IUser {

        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; }
    }
}
