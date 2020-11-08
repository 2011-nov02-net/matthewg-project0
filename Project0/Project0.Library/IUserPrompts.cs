using System;
using System.Collections.Generic;
using System.Text;
using Project0.Library.Models;

namespace Project0.Library {
    public interface IUserPrompts {
        IStore Store { get; }

        void WelcomeMessage();

        IUser StartupPrompt(IUserInputInterpreter interpreter);

        IUser RegisterPrompt(IUserInputInterpreter interpreter);

        void ReturningCustomerPrompt(IUser customer);

        bool? StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer);

        bool? AdminPrompt(IUserInputInterpreter interpreter);

        bool NewStoreLocation(IUserInputInterpreter interpreter);

        bool RestockPrompt(IUserInputInterpreter interpreter);

        bool NewProductPrompt(IUserInputInterpreter interpreter);

        bool? LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer);

        void CheckoutPrompt(IOrder order);

        bool? CartPrompt(IUserInputInterpreter interpreter, Customer customer);
    }
}
