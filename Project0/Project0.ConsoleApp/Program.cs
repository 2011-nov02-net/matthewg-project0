using System;
using System.Reflection.Metadata.Ecma335;
using Project0.Library;
using Project0.Library.Models;

namespace Project0.ConsoleApp {
    class Program {
        static void Main(string[] args) {
            string path = args[0];
            // path = "../../../store_data.json";
            IStore store = DataPersistence.Read(path);
            var prompts = new ConsolePrompts(store);
            var interpreter = new ConsoleInputInterpreter(prompts);
            var user_interface = new StoreInterface(prompts, interpreter);

            user_interface.Launch();
            
        }
    }
}
