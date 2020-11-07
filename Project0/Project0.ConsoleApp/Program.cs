using System;
using System.Reflection.Metadata.Ecma335;
using Project0.Library;
using Project0.Library.Models;

namespace Project0.ConsoleApp {
    class Program {
        static void Main(string[] args) {

            IStore store = new Store(); // TODO: Replace with a read from disk

            Console.WriteLine("Welcome to Matt's store.\n");

            while (true) {
                bool? response = true;
                while (response ?? true) {
                    response = ConsolePrompts.StartupPrompt(store);
                    if (response == null) {
                        return;
                    }

                }

                response = true;
                while (response ?? true) {
                    response = ConsolePrompts.StoreEntryPrompt(store);
                    if (response == null) {
                        break;
                    }
                }
            }
        }
    }
}
