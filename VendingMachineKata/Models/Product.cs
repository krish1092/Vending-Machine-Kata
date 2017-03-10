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
        public double ProductPrice { get; set; }

        [DataType(DataType.Currency)]
        public int ProductCount { get; set; }

        public string DisplayProduct
        {
            get
            {
                return string.Format("{0} {1}", ProductName, ProductPrice);
            }
        }

        public Product (string name, double price, int count)
        {
            ProductName = name;
            ProductPrice = price;
            ProductCount = count;
        }
    }
}