using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendingMachineKata.Models
{
    public class CoinValues
    {
        public int Count { get; set; } // Number of coins in our coin pool

        public decimal Value { get; set; } //Value of the coin
    }
}