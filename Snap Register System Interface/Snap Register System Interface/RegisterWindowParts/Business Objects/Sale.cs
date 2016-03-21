using System;

namespace SnapRegisters
{
    public class Sale: IDiscount
    {
        public Sale( bool flat, string name, decimal amt )
        {
            Flat = flat;
            Name = name;
            Amount = amt;
        }

        public decimal ChangeAmountTo( decimal amt )
        {
            //Don't let the value go below 0 if it's a subtraction.
            return Flat ? Math.Max( amt - Amount, 0 ) : amt * ( 1 - Amount );
        }

        public override string ToString()
        {
            return "Sale: " + Name;
        }

        public bool IsFlat() { return Flat; }
        public decimal Discount() { return Amount; }

        public bool Flat { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0M;

    }
}
