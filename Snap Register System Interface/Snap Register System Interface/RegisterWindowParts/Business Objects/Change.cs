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
        public string total { get; }

        public Change(decimal changeToConvert)
        {
            total = changeToConvert.ToString("C");

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

            halfdollars = Convert.ToInt32(changeToConvert / .50M);
            changeToConvert -= halfdollars * .50M ;

            quarters = Convert.ToInt32(changeToConvert / .25M );
            changeToConvert -= quarters * .25M;

            //TODO: WHY DOES THIS ALWAYS MESS UP AT DIMES
            dimes = Convert.ToInt32(changeToConvert / .10M >= 1 ? Convert.ToInt32(changeToConvert / .10M) : 0);
            changeToConvert -= Convert.ToDecimal(dimes * .10);

            nickels = Convert.ToInt32(changeToConvert / .05M );
            changeToConvert -= nickels * .05M;

            pennies = Convert.ToInt32(changeToConvert / .01M );
            changeToConvert -= pennies * .01M ;
        }


    }
}
