﻿using System;
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

        // GET: Vending/
        public ActionResult OpenVendingMachine()
        {
            return View();
        }



        // GET: Vending/GetProducts
        public ActionResult GetProducts()
        {
            return PartialView(VendingService.GetProductList());
        }

        // GET : Vending/Console
        public ActionResult GetConsole()
        {
            return PartialView();
        }

        //GET : Vending/Coins

        public ActionResult GetCoins()
        {
            return PartialView(VendingService.GetAcceptedCoins());
        }

        
        /// <summary>
        /// The collection area of the vending mavchine
        /// </summary>
        /// <returns>Partial View</returns>
        // GET : Vending/Console
        public ActionResult GetCollection()
        {
            return PartialView();
        }


        /// <summary>
        /// Get the coin value of the inserted coin
        /// </summary>
        /// <param name="Coin"></param>
        /// <returns>Coin Value</returns>

        //GET : Vending/GetCoinValue
        [HttpGet]
        public JsonResult GetCoinValue(Coin Coin)
        {
            double CoinValue = VendingService.GetCoinValue(Coin);
            return Json(new { CoinValue = CoinValue });
        }


        /// <summary>
        /// Get the Product count value of the chosen product
        /// </summary>
        /// <param name="ProductName"></param>
        /// <returns>ProductCount</returns>

        //GET : Vending/GetProductValue
        [HttpGet]
        public JsonResult GetProductCount(string ProductName)
        {
            int ProductCount = VendingService.GetProductCount(ProductName);
            return Json(new { ProductCount = ProductCount });
        }



        // GET : Vending/GetProducts
        /// <summary>
        /// Refill the vending machine
        /// </summary>
        /// <returns> Message of refill</returns>
        [HttpGet]
        public JsonResult RefillVendingMachine()
        {
            VendingService.RefillVendingMachine();
            return Json(new { Message = "The vending machine has been refilled" });
        }


        // POST : Vending/Complete
        /// <summary>
        /// This method completes the vending process by adding the inserted coins to the existing coin pool ,subtracting the product count and 
        /// tenders the change
        /// </summary>
        /// <param name="CoinsInserted"></param>
        /// <param name="ProductName"></param>
        /// <param name="RemainingAmount"></param>
        /// <returns>A jsonresult containing the result of validation and remaining amount</returns>

        [HttpPost]
        public JsonResult CompleteVending(Coin[] CoinsInserted, string ProductName, double RemainingAmount)
        {
            //Validate the coins
            bool Validated = VendingService.ValidateInsertedCoins(CoinsInserted);
            
            if (Validated)
            {
                //Add inserted coins to the existing pool of coins
                VendingService.AddToExistingChange(CoinsInserted);

                //Subtract Product Count
                VendingService.SubtractProductCount(ProductName);

                //Tender the change;
                RemainingAmount = VendingService.TenderChange(RemainingAmount);
            }
            
            //It does not carry any sensitive information - So we can allow get.
            return Json(new { RemainingAmount = RemainingAmount, Validated = Validated }, JsonRequestBehavior.AllowGet);
        }
    }
}