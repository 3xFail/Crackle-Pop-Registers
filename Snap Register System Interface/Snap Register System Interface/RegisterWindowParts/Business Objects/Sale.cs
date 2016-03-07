using System;

namespace SnapRegisters
{
    public class Sale: IDiscount
    {
        public Sale( bool flat, string name, double amt )
        {
            Flat = flat;
            Name = name;
            Amount = amt;
        }

        //Don't let the value go below 0.
        public double ChangeAmountTo( double amt )
        {
            return Math.Max( Flat ? amt - Amount : amt * Amount, 0 );
        }

        public override string ToString()
        {
            return "Sale: " + Name;
        }

        public bool Flat { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; } = 0.0;

    }
}
