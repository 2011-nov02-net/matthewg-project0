using Project0.Library.Interfaces;

namespace Project0.Library.Models {
    public class Admin : IUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; }

        public Admin() {
            FirstName = "admin";
            LastName = "";
            Email = "a@example.net";
        }
    }
}
