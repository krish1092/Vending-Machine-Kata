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

        



        // GET: Product/GetProducts
        public ActionResult GetProducts()
        {
            return PartialView(VendingService.GetProductList());
        }


        /// <summary>
        /// Refill the vending machine
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult RefillVendingMachine()
        {
            VendingService.RefillVendingMachine();
            return Json(new { Message = "The vending machine has been refilled" });
        }


        // POST : VendingMachine/Complete
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