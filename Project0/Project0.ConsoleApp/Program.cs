using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project0.DataModels;
using Project0.DataModels.Entities;
using Project0.DataModels.Repositories;
using Project0.Library;
using Project0.Library.Interfaces;
using Project0.Library.Models;

namespace Project0.ConsoleApp {
    class Program {
        static void Main(string[] args) {
            using var logStream = new StreamWriter("ef-logs.txt");

            var ob = new DbContextOptionsBuilder<Project0Context>();
            ob.UseSqlServer(GetConnectionString());
            ob.LogTo(logStream.WriteLine, LogLevel.Information);

            using var context = new Project0Context(ob.Options);

            IStoreRepository storeRepository = new StoreRepository(context);

            /*string path = "../../../store_data.xml";
            IStore store = DataPersistence.Read(path);
            var prompts = new ConsolePrompts(store);
            var interpreter = new ConsoleInputInterpreter(prompts);
            var user_interface = new StoreInterface(prompts, interpreter);

            user_interface.Launch();*/
            
        }

        static string GetConnectionString() {
            string path = "../../../../../../../Project0-connection-string.json";
            string json;
            try {
                json = File.ReadAllText(path);
            } catch (IOException) {
                Console.WriteLine("Bad path.");
                throw;
            }
            string connectionString = JsonSerializer.Deserialize<string>(json);
            return connectionString;
        }
    }
}
