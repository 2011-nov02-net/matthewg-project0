using Project0.Library.Interfaces;
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
            bool? response = true;
            while (response ?? true) {
                response = _prompts.StoreEntryPrompt(_interpreter, customer);
                if (response == null) {
                    return;
                } else if (response == false) {
                    response = SendCustomerToStoreLocation(customer);
                }
            }
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

        private bool SendCustomerToStoreLocation(Customer customer) {
            bool? response = true;
            while (response ?? true) {
                response = _prompts.LocationInventoryPrompt(_interpreter, customer);
                if (response == null) {
                    return true;
                }
            }

            _prompts.CheckoutPrompt(customer);
            return false;
        }
    }
}
