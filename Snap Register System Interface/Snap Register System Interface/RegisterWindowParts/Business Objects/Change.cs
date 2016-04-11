using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class Change
    {
        public int hundreds { get; }
        public int twenties { get; }
        public int tens { get; }
        public int fives { get; }
        public int ones { get; }
        public int halfdollars { get; }
        public int quarters { get; }
        public int dimes { get; }
        public int nickels { get; }
        public int pennies { get; }
        public decimal total { get; }

        public Change(decimal changeToConvert)
        {
            total = changeToConvert;

            hundreds = Convert.ToInt32(changeToConvert) / 100;
            changeToConvert -= hundreds * 100;

            twenties = Convert.ToInt32(changeToConvert) / 20;
            changeToConvert -= twenties * 20;

            tens = Convert.ToInt32(changeToConvert) / 10;
            changeToConvert -= tens * 10;

            fives = Convert.ToInt32(changeToConvert) / 5;
            changeToConvert -= fives * 5;

            ones = Convert.ToInt32(changeToConvert) / 1;
            changeToConvert -= ones * 1;

            halfdollars = Convert.ToInt32(changeToConvert / Convert.ToDecimal(.50));
            changeToConvert -= halfdollars * Convert.ToDecimal(.50);

            quarters = Convert.ToInt32(changeToConvert / Convert.ToDecimal(.25));
            changeToConvert -= quarters * Convert.ToDecimal(.25);

            //TODO: WHY DOES THIS ALWAYS MESS UP AT DIMES
            dimes = Convert.ToInt32(changeToConvert / Convert.ToDecimal(.10));
            changeToConvert -= dimes * Convert.ToDecimal(.10);

            nickels = Convert.ToInt32(changeToConvert / Convert.ToDecimal(.05));
            changeToConvert -= nickels * Convert.ToDecimal(.05);

            pennies = Convert.ToInt32(changeToConvert / Convert.ToDecimal(.01));
            changeToConvert -= pennies * Convert.ToDecimal(.01);






        }


    }
}
