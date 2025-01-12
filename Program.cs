using System.Text.Json;

namespace CurrencyConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Creating Variables for main program
            string menuOption;
            bool appIsOn = true;
            const string filePath = "CurrencyConverter\\CountryData\\";

            //APP BEGINS HERE
            while (appIsOn)
            {
                menuOption = promptMainMenu(); //Selecting option from the menu
                switch (menuOption)            //Different Functions will be called through switch
                {
                    case "CD": //Creating the DataBase
                        createDatabase(filePath);
                        break;
                    case "LD": //Loading Data
                        loadDataBase(filePath);
                        break;
                    case "CC": //Converting Currency
                        convertDataBaseCurrency(filePath);
                        break;
                    default:
                        Console.WriteLine("You did not input a valid choice");
                        break;
                }
            }
        }//END main

        //- - - - - - - - - - - FUNCTIONS - - - - - - - - - - -

        //Asks what option you would like to select: Create Database, Convert Currency, Load Data
        static string promptMainMenu()
        {
            //Initializing Variables
            string option;

            //Retrieving an option
            Console.Write("Welcome to the Currency Converter\nWhat would you like to do?\nCreate Database (CD) or Convert Currency (CC) or Load Data (LD): ");
            option = Console.ReadLine().ToUpper();

            //Checking each case to make sure a valid option was chosen.
            switch (option)
            {
                case "CD":
                    return option;
                case "CC":
                    return option;
                case "LD":
                    return option;
                default:
                    return "ERROR - Invalid Choice";

            }
        }//END promptMainMenu

        //Creates a Database that is saved locally
        static void createDatabase(string file_path)
        {
            //Initializing Variables
            string countryName;
            string currencyCode;

            string exchangeCurrencyCode;
            double exchangeCurrencyValue;

            bool creatingExchangeRate = true;
            string loopAgain;

            //Asking the Questions:
            Console.Write("Name of the Country: ");
            countryName = getString();

            Console.Write("Currency Code: ");
            currencyCode = getString();

            //Initializing Database Object
            Database db = new Database(countryName, currencyCode, file_path);

            //Loop for creating exchange rates:
            while (creatingExchangeRate)
            {
                //Beginning the Database Creation Process
                Console.WriteLine("Lets add an exchange rate");

                Console.Write("Name of the Currency: ");
                exchangeCurrencyCode = getString().ToUpper();

                //Retrieving Exchange Amount: Ex. 1 USD = 1.27 CAD
                Console.Write($"Exchange Rate from {currencyCode} to {exchangeCurrencyCode}: ");
                exchangeCurrencyValue = double.Parse(getString());

                //Adding them to the dictionary
                db.addExchangeRate(exchangeCurrencyCode, exchangeCurrencyValue);

                //Asking if the user wants to loop again
                Console.WriteLine("\nWould you like to add another exchange rate? (Y or N)");
                loopAgain = Console.ReadLine().ToUpper();

                switch (loopAgain)
                {
                    case "N": //NO more data needs to be added
                        Console.WriteLine("Your database has been created");
                        db.createJSONFile();
                        creatingExchangeRate = false;
                        break;
                    case "Y": //MORE data needs to be added
                        Console.WriteLine("Lets add another exchange rate\n");
                        break;
                }
            }
        }//END createDatabase

        //Load DataBase
        static void loadDataBase(string file_path)
        {
            //Variable Initialization
            string option;
            Dictionary<string, object> countryData = new Dictionary<string, object>(); //Catches incoming dictionary

            //Prompting Question
            Console.Write("Which Country's Database are you wanting to load?: ");
            option = Console.ReadLine().ToUpper();

            //Creating DataHander Object
            DataHandler dh = new DataHandler(file_path);
            countryData = dh.getData(option);

            //Printing out information
            Console.WriteLine($"Country Name: " + countryData["country_name"]);
            Console.WriteLine($"Currency Code: " + countryData["currency_code"]);

            //Safely Handle the Exchange Rates
            if (!countryData.ContainsKey("foreign_exchange_rate")) //Checks to see if it contains the key
            {
                Console.WriteLine("Foreign exchange rates could not be loaded.");
            }
            else if (countryData["foreign_exchange_rate"] is not JsonElement jsonElement) //Checks to see if it isn't a JsonElement
            {
                Console.WriteLine("Foreign exchange rates could not be loaded.");
            }
            else //If all conditions are TRUE, then the following gets executed.
            {
                // Deserialize the foreign_exchange_rate to Dictionary<string, double>
                var foreignExchangeRates = JsonSerializer.Deserialize<Dictionary<string, double>>(jsonElement.GetRawText());

                if (foreignExchangeRates == null) //Checks to make sure nothing is null/empty
                {
                    Console.WriteLine("Foreign exchange rates are empty or null.");
                }
                else //Finally loads all the data
                {
                    foreach (var rate in foreignExchangeRates)
                    {
                        Console.WriteLine($"{rate.Key}: {rate.Value}");
                    }
                }
            }

        }//END loadDatabase

        //Convert currency from database
        static void convertDataBaseCurrency(string file_path)
        {
            //variable initialization
            string countryOption; //user selects which country they want to convert from
            string currencyChoice; //user selects which currency they want to convert to
            double moneyAmount; //user places how much money they want to convert to another currency

            Dictionary<string, object> countryData = new Dictionary<string, object>(); //Dictionary where country's data is saved in

            //Prompting which country you would like
            Console.Write("Which Country's Currency would you like to convert?: ");
            countryOption = Console.ReadLine().ToUpper();

            //create DataHandler Object
            DataHandler dh = new DataHandler(file_path);
            countryData = dh.getData(countryOption); //Retrieving Data

            //Safely seperating foreign exchange rates as a seperate dictionary
            if (!countryData.ContainsKey("foreign_exchange_rate"))  //Checks to make sure dict contains the key
            {
                Console.WriteLine("Foreign exchange rates could not be loaded.");
            }
            else if (countryData["foreign_exchange_rate"] is not JsonElement jsonElement) //Checks to make sure dict is a JsonElement
            {
                Console.WriteLine("Foreign exchange rates could not be loaded.");
            }
            else //If all is True, the following gets executed
            {
                // Deserialize the foreign_exchange_rate to Dictionary<string, double>
                var foreignExchangeRates = JsonSerializer.Deserialize<Dictionary<string, double>>(jsonElement.GetRawText());

                if (foreignExchangeRates == null) //Makes sure the dict isn't null/empty
                {
                    Console.WriteLine("Foreign exchange rates are empty or null.");
                }
                else
                {
                    // Giving Information on the Exchange Rates
                    Console.WriteLine("Here is the list of currencies you can convert to!");
                    foreach (var rate in foreignExchangeRates)
                    {
                        Console.WriteLine($"{rate.Key}: {rate.Value}");
                    }

                    // Prompt user to select which currency and how much
                    Console.Write("\nWhich currency would you like to convert to: ");
                    currencyChoice = Console.ReadLine().ToUpper();
                    Console.Write("\nHow much money would you like to convert: $");
                    moneyAmount = double.Parse(Console.ReadLine());

                    dh.convertCurrency(moneyAmount, currencyChoice, foreignExchangeRates); //The function that converts currency from DataHandler object
                }
            }
        }//END convertDataBaseCurrency

        //Method to return a string without being null
        static string getString()
        {
            //Initializing Variables
            string answer = "Default_Value";      //Answer that will be returned
            bool isNull = true; //Will keep looping until the string is no longer null.

            //Checking Null Loop
            while (isNull)
            {
                answer = Console.ReadLine();
                if (string.IsNullOrEmpty(answer))
                {
                    Console.WriteLine("You did not write anything. Try Again");
                }
                else
                {
                    isNull = false; //Is not empty, good to go!
                }
            }
            return answer;
        }//END getString
    }
}
