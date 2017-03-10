using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingMachineKata.Models;

namespace VendingMachineKata.Controllers
{
    public class VendingController : Controller
    {

        private static List<Product> ListOfProducts = new List<Product>(){ new Product("Cola", 1.00, 10),
                                                                        new Product("Chips", 0.50, 10),
                                                                        new Product("Chips", 0.50, 10) };
       
        // GET: Product/GetProducts
        public ActionResult GetProducts()
        {
           
            return View(ListOfProducts);
        }


        // POST : Product/Vending
        [HttpPost]
        public string CompleteVending()
        {

            return "";
        }
    }
}