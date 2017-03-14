using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VendingMachineKata.Models
{
	public class Product
	{
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }

        [DataType(DataType.Currency)]
        public int ProductCount { get; set; }

        public string DisplayProduct
        {
            get
            {
                return string.Format("{0}:{1}", ProductName, ProductPrice);
            }
        }

        public Product (string name, decimal price, int count)
        {
            ProductName = name;
            ProductPrice = price;
            ProductCount = count;
        }
    }


    //Other Code
}