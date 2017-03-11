using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VendingMachineKata.Models;
using VendingMachineKata.Service;


namespace VendingMachineKata.Controllers
{
    public class VendingController : Controller
    {
        //Note - This is not the ideal way to store the values as these values are gone at the end of session. The correct way is to use a database 
        //but since it was not part of the requirement, I'm using static fields.

        private static List<Product> ListOfProducts = new List<Product>(){ new Product("Cola", 1.00, 10),
                                                                        new Product("Chips", 0.50, 10),
                                                                        new Product("Candy", 0.65, 10) };

        private static Dictionary<string, Product> productDictionary = new Dictionary<string, Product> {
            { "Cola", new Product("Cola", 1.00, 10) },
            { "Chips", new Product("Chips", 0.50, 10)},
            {"Candy", new Product("Candy", 0.65, 10)}
        };

        
        

        public ActionResult GetVendingMachine()
        {
            return View(productDictionary.Values);
        }

        [HttpGet]
        public ActionResult GetAcceptedCoins()
        {
            
            return PartialView();
        }

        /// <summary>
        /// Refill the vending machine
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string RefillVendingMachine()
        {

            VendingService.RefillVendingMachine();
            return "The vending machine has been refilled";
        }



        // GET: Product/GetProducts
        public ActionResult GetProducts()
        {
            return View(ListOfProducts);
        }


        // POST : Product/Vending
        [HttpPost]
        public JsonResult CompleteVending(double[] CoinsInserted, string ProductName, double RemainingAmount)
        {
            //Add inserted coins to the existing pool of coins
            VendingService.AddToExistingChange(CoinsInserted);

            //Subtract Product Count
            VendingService.SubtractProductCount(ProductName);

            //Tender the change;
            RemainingAmount = VendingService.TenderChange(RemainingAmount);

            return Json(RemainingAmount);
        }
    }
}