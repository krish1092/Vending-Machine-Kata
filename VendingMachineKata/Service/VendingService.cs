using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VendingMachineKata.Models;

namespace VendingMachineKata.Service
{
    public static class VendingService
    {

        //For getting the list of products
        private static Dictionary<string, Product> ProductDictionary = new Dictionary<string, Product>(){
            {"Cola", new Product("Cola", 1.00, 10) },
            {"Chips", new Product("Chips", 0.50, 10) },
            {"Candy", new Product("Candy", 0.65, 10) }
        };

        //For Validating coins & Tendering Change- Dictionary holds the mapping of a coin and its value; Note that, the Coin Class by itself does not know the Coin's value
        private static Dictionary<Coin, CoinValues> AcceptedCoinsDictionary = new Dictionary<Coin, CoinValues> {
            { new Coin {Size = 3, Weight = 20, Name = "Quarter" }, new CoinValues { Count = 10, Value = 0.25 } },
            { new Coin {Size = 1, Weight = 5, Name = "Dime" }, new CoinValues { Count = 10, Value = 0.10 } },
            { new Coin {Size = 2, Weight = 10 , Name = "Nickel" }, new CoinValues { Count = 10, Value = 0.05 } }
        };
        
        /// <summary>
        /// The algorithm goes from quarters to nickels. Tries to give as many quarters,dimes, nickels(in this order) as possible
        /// </summary>
        /// <param name="RemainingAmount">The remaining amount to be tendered to the user</param>
        /// <returns></returns>
        
        public static double TenderChange(double RemainingAmount)
        {

            foreach (Coin coin in AcceptedCoinsDictionary.Keys)
            {

                CoinValues CoinValues = AcceptedCoinsDictionary[coin];

                //Number of coins for the current coin value
                int n = (int)(RemainingAmount / CoinValues.Value);

                int i = 0;

                //n > 0 only if the remaining Amount is greater than coin value
                if (n > 0)
                {
                    //Eg: This check ensures that the maximum number of coins <=n are subtracted without the coin count running negative.
                    while (CoinValues.Count > 0 && i < n)
                    {
                        i++;
                        CoinValues.Count--;
                        RemainingAmount = RemainingAmount - CoinValues.Value; //Subtract the amount taken off
                    }
                }


                if (RemainingAmount == 0)
                    return 0; //Indicates that the amount to be tendered has been fulfilled.

            }
            return RemainingAmount; //Vending machine unable to tender this change
        }

        /// <summary>
        /// Add the inserted coins to the existing pool of coins
        /// </summary>
        /// <param name="coinsInserted"></param>
        public static void AddToExistingChange(Coin[] coinsInserted)
        {
            //The customer has inserted a few coins as well. Add them to the pool of coins that already exist and then tender change
            foreach (Coin Coin in coinsInserted)
            {
                Coin CoinInTheDictionary = AcceptedCoinsDictionary.Keys.Where(key => key.Equals(key, Coin)).First();
                AcceptedCoinsDictionary[CoinInTheDictionary].Count++; //Increment the corresponding coin value
            }
            
        }

        /// <summary>
        /// Get product list to show to the user
        /// </summary>
        /// <returns></returns>
        public static List<Product> GetProductList()
        {
            return new List<Product>(ProductDictionary.Values);
        }


        public static List<Coin> GetAcceptedCoins()
        {
            return new List<Coin>(AcceptedCoinsDictionary.Keys);
        }

        /// <summary>
        /// This method is called during the vending process
        /// </summary>
        public static void SubtractProductCount(string product)
        {
            ProductDictionary[product].ProductCount--;
        }

        /// <summary>
        /// Refill Vending Machine
        /// </summary>
        /// <param name="product"></param>
        public static void RefillVendingMachine()
        {
            //Refilling Products
            foreach (string productName in ProductDictionary.Keys)
                ProductDictionary[productName].ProductCount = 10;
            
        }

        /// <summary>
        /// Backend Validation of inserted Coins
        /// </summary>
        /// <param name="coins"></param>
        /// <returns></returns>
        public static bool ValidateInsertedCoins(Coin[] coins)
        {
            foreach(Coin coin in coins)
            {
                if(AcceptedCoinsDictionary.Keys.Where(key => key.Equals(key, coin)).Count() == 0) //If the count is = 0, then coin is not acceptable
                    return false;
            }
            return true;
        }



        /// <summary>
        /// Get the coin value
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        public static double GetCoinValue(Coin coin)
        {
            Coin CoinInTheDictionary = AcceptedCoinsDictionary.Keys.Where(key => key.Equals(key, coin)).First();
                    return CoinInTheDictionary == null ? 0 : AcceptedCoinsDictionary[CoinInTheDictionary].Value;
            
        }

        /// <summary>
        /// Get the product count value
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        public static int GetProductCount(string ProductName)
        {
            if(ProductDictionary.ContainsKey(ProductName))
                return ProductDictionary[ProductName].ProductCount;

            return 0;
        }


    }
}