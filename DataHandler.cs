using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

/*
The purpose of this class is to handle data such as incoming dictionaries.
    Loads data, converts currencies, retrieves data as dictionaries.
*/

namespace CurrencyConverter
{
    internal class DataHandler
    {
        //Variable Initialization
        private string filePath;

        //- - - - - - - - - - - CONSTRUCTOR - - - - - - - - - - -
        public DataHandler(string file_path)
        {
            filePath = file_path;
        }

        //- - - - - - - - - - - Functions - - - - - - - - - - -

        //This method allows you to access a JSON file and read out all the data.
        public void readData(string country_name)
        {
            //Find the file address for the JSON data
            string listPath = (filePath + country_name + ".json");
            string jsonString = File.ReadAllText(listPath);
            Console.WriteLine(jsonString);
        }

        //This method returns a dictionary
        public Dictionary<string, object> getData(string country_name)
        {
            //Find file address for the JSON data
            string listPath = (filePath + country_name + ".json");
            string jsonString = File.ReadAllText(listPath);

            //Converting jsonString to a dictionary
            var dataDict = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

            return dataDict;
        }

        //This method allows you to convert currencies depending on the amount given
        public void convertCurrency(double dollarAmount, string currencyCodeChoice ,Dictionary<string, double>countryData)
        {
            //Variable Initialization
            double totalAmount;

            //Checking to see if the currency code choice is in the dictionary
            if (!countryData.ContainsKey(currencyCodeChoice))
            {
                Console.WriteLine("This Currency Code does not appear in the database");
            }
            else
            {
                totalAmount = (countryData[currencyCodeChoice] * dollarAmount);
                Console.WriteLine("Total Amount = " + totalAmount);
            }
            
        }
    }
}
