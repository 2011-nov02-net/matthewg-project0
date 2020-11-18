using Project0.Library.Models;

namespace Project0.Library.Interfaces {
    public interface IUserPrompts {
        IStoreRepository Store { get; }

        void WelcomeMessage();

        IUser StartupPrompt(IUserInputInterpreter interpreter);

        IUser RegisterPrompt(IUserInputInterpreter interpreter, out bool exit);

        void ReturningCustomerPrompt(IUser customer);

        void EnterStoreLocationPrompt(IUserInputInterpreter interpreter, Customer customer);

        void StoreEntryPrompt(IUserInputInterpreter interpreter, Customer customer, out bool exit);

        void AdminPrompt(IUserInputInterpreter interpreter, out bool exit);

        void OrderHistoryPrompt(IUserInputInterpreter interpreter);

        void NewStoreLocation(IUserInputInterpreter interpreter);

        void RestockPrompt(IUserInputInterpreter interpreter);

        decimal ProductPricePrompt(IUserInputInterpreter interpreter, out bool exit);

        void NewProductPrompt(IUserInputInterpreter interpreter);

        void UserLookupPrompt(IUserInputInterpreter interpreter);

        Customer CustomerEmailEntry(IUserInputInterpreter interpreter);

        Location LocationEntry(IUserInputInterpreter interpreter);

        void PrintOrderHistory();

        void PrintOrderHistory(Customer customer);

        void PrintOrderHistory(Location location);

        void LocationInventoryPrompt(IUserInputInterpreter interpreter, Customer customer, out bool exit);

        void CheckoutPrompt(Order order);

        void CartPrompt(IUserInputInterpreter interpreter, Customer customer, out bool checkout);

        void RemoveProductFromCartPrompt(IUserInputInterpreter interpreter, Customer customer);
    }
}
