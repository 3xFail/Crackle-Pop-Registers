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

        public Change( decimal changeToConvert )
        {
            total = changeToConvert.ToString( "C" );

            hundreds = (int)changeToConvert / 100;
            changeToConvert -= hundreds * 100;

            twenties = (int)changeToConvert / 20;
            changeToConvert -= twenties * 20;

            tens = (int)changeToConvert / 10;
            changeToConvert -= tens * 10;

            fives = (int)changeToConvert / 5;
            changeToConvert -= fives * 5;

            ones = (int)changeToConvert / 1;
            changeToConvert -= ones * 1;

            int cents = (int)( changeToConvert * 100M );

            if( cents == 0 ) return;

            halfdollars = cents / 50;
            cents -= halfdollars * 50;

            if( cents == 0 ) return;

            quarters = cents / 25;
            cents -= quarters * 25;

            if( cents == 0 ) return;

            dimes = cents / 10;
            cents -= dimes * 10;

            if( cents == 0 ) return;

            nickels = cents / 5;
            cents -= nickels * 5;

            if( cents == 0 ) return;

            pennies = cents / 1;
            cents -= pennies * 1;
        }

    }
}
