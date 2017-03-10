using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendingMachineKata.Models
{
    public class Coin
    {
        public double Size { get; set; }
        public double Weight { get; set; }
        public int Value { get; set; }

        public int CountOfCoins { get; set; }
        
    }
}