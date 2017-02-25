using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VendingMachineKata.Models;

namespace VendingMachineKata.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/GetProducts
        public ActionResult GetProducts()
        {
            Product cola = new Product("Cola", 1.00, 10);
            Product chips = new Product("Chips", 0.50, 10);
            Product candy = new Product("Candy", 0.65, 10);

            List<Product> ListOfProducts = new List<Product>();
            ListOfProducts.Add(cola);
            ListOfProducts.Add(chips);
            ListOfProducts.Add(candy);

            return View(ListOfProducts);
        }
    }
}