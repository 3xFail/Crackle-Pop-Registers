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

        public double ChangeAmountTo( double amt )
        {
            //Don't let the value go below 0 if it's a subtraction.
            return Flat ? Math.Max( amt - Amount, 0 ) : amt * Amount;
        }

        public override string ToString()
        {
            return "Sale: " + Name;
        }

        public bool IsFlat() { return Flat; }
        public double Discount() { return Amount; }

        public bool Flat { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; } = 0.0;

    }
}
