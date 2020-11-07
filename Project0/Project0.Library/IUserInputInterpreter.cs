using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library {
    public interface IUserInputInterpreter {

        IUserPrompts Prompts { get; }

        IUser ValidUserID(string s, IStore store);

        Customer RegisterCustomer(string s, IStore store);

        bool? ValidLocation(string s, IStore store, Customer customer);

        bool? ValidAdminCommand(string s, IStore store);

        bool GenerateLocation(string s, IStore store);
    }
}
