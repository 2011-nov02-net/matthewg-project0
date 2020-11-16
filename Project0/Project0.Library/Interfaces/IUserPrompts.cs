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

        void OrderHistoryPrompt(IUserInputInterpreter interpreter);

        bool NewStoreLocation(IUserInputInterpreter interpreter);

        bool RestockPrompt(IUserInputInterpreter interpreter);

        decimal? ProductPricePrompt(IUserInputInterpreter interpreter);

        bool NewProductPrompt(IUserInputInterpreter interpreter);

        bool UserLookupPrompt(IUserInputInterpreter interpreter);

        Customer CustomerEmailEntry(IUserInputInterpreter interpreter);

        Location LocationEntry(IUserInputInterpreter interpreter);

        bool PrintOrderHistory();

        bool PrintOrderHistory(Customer customer);

        bool PrintOrderHistory(Location location);

        bool? LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer);

        void CheckoutPrompt(Order order);

        bool? CartPrompt(IUserInputInterpreter interpreter, Customer customer);

        bool? RemoveProductFromCartPrompt(IUserInputInterpreter interpreter, Customer customer);
    }
}
