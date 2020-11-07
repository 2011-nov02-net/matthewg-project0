using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library.Models {
    public class Admin : IUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; }

        public Admin() {
            FirstName = "admin";
            LastName = "";
            Id = "0000a";
        }
    }
}
