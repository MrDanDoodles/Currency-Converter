using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

/* 
Purpose of this class is to quickly create databases with a standardized format for the purpose of converting currencies later on 
*/

namespace CurrencyConverter
{
    internal class Database
    {
        //Creating Variables for the Class
        private string countryName;
        private string currencyCode;
        private string filePath;

        //Creating the dictionary to hold all the data, soon to be sent as a JSON
        private static Dictionary<string, object> ExchangeData = new Dictionary<string, object>
        {
            {"country_name", "" },
            {"currency_code", "" },
            {"foreign_exchange_rate", new Dictionary<string, double> { } }
        };

        //- - - - - - - - - - - CONSTRUCTOR - - - - - - - - - - -
        public Database(string country_name, string currency_code, string file_path)
        {
            countryName = country_name;
            currencyCode = currency_code;
            filePath = file_path;

            ExchangeData["country_name"] = countryName;
            ExchangeData["currency_code"] = currencyCode;
        }

        //- - - - - - - - - - - FUNCTIONS - - - - - - - - - - -

        //Method to add exchange rates to the dictionary
        public void addExchangeRate(string currency_code, double amount)
        {
            //Creates the small dictionary
            var exchangeRate = new Dictionary<string, double> { { currency_code, amount } };

            //appending the exchange rate to 'foreign_exchange_rate'
            if (ExchangeData["foreign_exchange_rate"] is Dictionary<string, double> foreignExchangeRate)
            {
                foreignExchangeRate[currency_code] = amount;
            }   
        }//END addExchangeRate

        //Creating a JSON File
        public void createJSONFile()
        {
            //converting dictionary to JSON string
            string json = JsonSerializer.Serialize(ExchangeData, new JsonSerializerOptions { WriteIndented = true });

            //sending JSON file to a Folder
            File.WriteAllText(filePath + $"{countryName.ToUpper()}.json", json);
        }//END createJSONFile
        
    }
}
