using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VendingMachineKata.Models
{
    public class Coin : IEqualityComparer<Coin>
    {
        public double Size { get; set; } //Size of the coin (Likely, Diameter)
        public double Weight { get; set; } //Weight of the coin
        public int Count { get; set; } // Number of coins in our coin pool

        /// <summary>
        /// Comparing two differnt coin objects
        /// </summary>
        /// <param name="x">Parameter 1</param>
        /// <param name="y">Paramter 2</param>
        /// <returns></returns>
        public bool Equals(Coin x, Coin y)
        {
            return x != null && y != null && x.Weight == y.Weight && x.Size == y.Size;
        }

        public int GetHashCode(Coin obj)
        {
            return base.GetHashCode();
        }
    }
    
}