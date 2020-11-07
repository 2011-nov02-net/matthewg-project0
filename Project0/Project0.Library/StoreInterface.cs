using Project0.Library.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project0.Library {
    public class StoreInterface {

        private IUserPrompts _prompts;
        private IUserInputInterpreter _interpreter;

        public StoreInterface(IUserPrompts prompts, IUserInputInterpreter interpreter) {
            _prompts = prompts;
            _interpreter = interpreter;
        }

        public void Launch() {
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
            bool? response = true;
            while (response ?? true) {
                response = _prompts.StoreEntryPrompt(_interpreter, customer);
                if (response == null) {
                    return;
                }
            }

            SendCustomerToStoreLocation(customer);
        }

        private void LoginAdmin() {
            bool? response = true;
            while (response ?? true) {
                response = _prompts.AdminPrompt(_interpreter);
                if (response == null) {
                    return;
                }
            }
        }

        private void SendCustomerToStoreLocation(Customer customer) {
            bool? response = true;
            while (response ?? true) {
                response = _prompts.LocationInventoryPrompt(_interpreter, customer);
                if (response == null) {
                    break;
                }
            }
        }
    }
}
