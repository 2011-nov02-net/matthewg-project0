using Project0.Library.Models;

namespace Project0.Library.Interfaces {
    public interface IUserPrompts {
        IStoreRepository Store { get; }

        void WelcomeMessage();

        IUser StartupPrompt(IUserInputInterpreter interpreter);

        IUser RegisterPrompt(IUserInputInterpreter interpreter);

        void ReturningCustomerPrompt(IUser customer);

        bool? StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer);

        bool? AdminPrompt(IUserInputInterpreter interpreter);

        bool NewStoreLocation(IUserInputInterpreter interpreter);

        bool RestockPrompt(IUserInputInterpreter interpreter);

        decimal? ProductPricePrompt(IUserInputInterpreter interpreter);

        bool NewProductPrompt(IUserInputInterpreter interpreter);

        bool UserLookupPrompt(IUserInputInterpreter interpreter);

        bool PrintOrderHistory(Customer customer);

        bool? LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer);

        void CheckoutPrompt(Order order);

        bool? CartPrompt(IUserInputInterpreter interpreter, Customer customer);
    }
}
