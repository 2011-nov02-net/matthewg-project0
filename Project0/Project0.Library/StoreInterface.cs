using Project0.Library.Interfaces;
using Project0.Library.Models;

namespace Project0.Library {
    public class StoreInterface {

        private IUserPrompts _prompts;
        private IUserInputInterpreter _interpreter;

        public StoreInterface(IUserPrompts prompts, IUserInputInterpreter interpreter) {
            _prompts = prompts;
            _interpreter = interpreter;
        }

        public void Launch() {
            _prompts.WelcomeMessage();
            while (true) {
                IUser user = null;
                while (user == null) {
                    user = _prompts.StartupPrompt(_interpreter);
                }
                Login(user);
            }
        }

        private void Login(IUser user) {
            if (user is Customer) {
                Customer customer = user as Customer;
                LoginCustomer(customer);
            } else if (user is Admin) {
                LoginAdmin();
            }
        }

        private void LoginCustomer(Customer customer) {
            bool exit = false;
            while (!exit) {
                _prompts.StoreEntryPrompt(_interpreter, customer, out exit);
            }
        }

        private void LoginAdmin() {
            bool exit = false;
            while (!exit) {
                _prompts.AdminPrompt(_interpreter, out exit);
            }
        }
    }
}
