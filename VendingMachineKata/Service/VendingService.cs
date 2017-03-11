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
        private static Dictionary<string,Product> ProductDictionary = new Dictionary<string, Product>(){
            {"Cola", new Product("Cola", 1.00, 10) },
            {"Chips", new Product("Chips", 0.50, 10) },
            {"Candy", new Product("Candy", 0.65, 10) }
        };

        //For Tendering change as required
        private static Dictionary<double, int> CoinValues = new Dictionary<double, int> {
            { 0.25 , 10},
            { 0.10 , 10},
            { 0.05 , 10}
        };


        /// <summary>
        /// The algorithm goes from quarters to nickels. Tries to give as many quarters,dimes, nickels(in this order) as possible
        /// </summary>
        /// <param name="RemainingAmount"></param>
        /// <returns></returns>
        public static double TenderChange(double RemainingAmount){

            foreach (double coinValue in CoinValues.Keys)
            {
                //Number of coins for the current coin value
                int n = (int)(RemainingAmount / coinValue);

                int i = 0;

                //n > 0 only if the remaining Amount is greater than coin value
                if (n > 0)
                {
                    //Eg: This check ensures that the maximum number of coins <=n are subtracted without the coin count running negative.
                    while (CoinValues[coinValue] > 0 && i < n)
                    {
                        i++;
                        CoinValues[coinValue]--;
                        RemainingAmount = RemainingAmount - coinValue; //Subtract the amount taken off
                    }
                }

               
                if (RemainingAmount == 0)
                    return 0; //Return 0 indicating that the amount to be tendered has been fulfilled.
                
            }
            return RemainingAmount; //Vending machine unable to tender this change
        }


        /// <summary>
        /// Add the inserted coins to the existing pool of coins
        /// </summary>
        /// <param name="coinsInserted"></param>
        public static void AddToExistingChange(double[] coinsInserted)
        {
            //The customer has inserted a few coins as well. Add them to the pool of coins that already exist and then tender change
            foreach (double coinValue in coinsInserted)
            {
                CoinValues[coinValue]++; //Increment the corresponding coin value
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
            foreach (string productName in ProductDictionary.Keys)
                ProductDictionary[productName].ProductCount = 10;

            
        }
        
    }
}