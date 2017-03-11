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
            return View(VendingService.GetProductList());
        }


        // POST : VendingMachine/Complete
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