using System;
using System.Collections.Generic;
using System.Text;
using Project0.Library.Models;
using System.IO;
using System.Text.Json;
using System.Runtime.Serialization;
using System.Xml;

namespace Project0.ConsoleApp {
    public class DataPersistence {

        public static IStore Read(string filePath) {
            Store data;
            FileStream fs = null;
            XmlDictionaryReader reader = null;
            try {
                fs = new FileStream(filePath, FileMode.Open);
                reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                DataContractSerializer ser = new DataContractSerializer(typeof(Store));

                data = (Store)ser.ReadObject(reader);
            } catch (Exception) {
                Console.WriteLine("No store found.");
                return new Store();
            } finally {
                reader?.Close();
                fs?.Close();
            }
            return data;
        }

        public static void Write(IStore data, string filePath) {
            DataContractSerializer ser = new DataContractSerializer(typeof(Store));
            using var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true });
            ser.WriteObject(writer, data);
        }
    }
}
