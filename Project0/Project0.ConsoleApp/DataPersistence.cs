using System;
using System.Collections.Generic;
using System.Text;
using Project0.Library.Models;
using System.IO;
using System.Text.Json;

namespace Project0.ConsoleApp {
    public class DataPersistence {

        public static IStore Read(string filePath) {
            string json;
            try {
                json = File.ReadAllText(filePath);
            } catch (IOException) {
                return new Store();
            }
            IStore data = JsonSerializer.Deserialize<Store>(json);
            return data;
        }

        public static void Write(IStore data, string filePath) {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }
    }
}
