using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapRegisters
{
    public class Coupon: IDiscount
    {

        //*************************************************************************************************************
        // public class Item
        //		SUMMARY: 
        //			This class represents an coupon in the sale. It contains basic information about it such as discount,
        //			name and its id number, and the item barcode it relates too.
        //		MEMBERS:
        //			public string m_coupon_code;
        //				The UPC code of this coupon.
        //			public string m_name
        //				The name of this coupon.
        //			public double m_discount
        //				The price of this coupon.
        //			public bool m_active
        //              whether the coupon is active or not
        //		FUNCTIONS:
        //			public Coupon(string coup_id, string barcode, string name, float price_change, bool active)
        //				Basic constructor.
        //			public Coupon(Coupon source)
        //				Copy Constructor.
        //		PERMISSIONS:
        //			None.
        //*************************************************************************************************************
        //fun fact, string.Empty is not a compile time constant and thus cannot be used as a default param
        public Coupon( string barcode, bool flat, string name, double amt )
        {
            Barcode = barcode;
            Flat = flat;
            Name = name;
            Amount = amt;
        }

        public void AddRelatedID( int ID )
        {
            RelatedIDs.Add( ID );
        }

        public bool AppliesTo( Item item )
        {
            return RelatedIDs.Contains( item.ID );
        }

        //Don't let the value go below 0.
        public double ChangeAmountTo( double amt )
        {
            return Math.Max( Flat ? amt - Amount : amt * Amount, 0 );
        }

        public override string ToString()
        {
            return "Coupon: " + Name;
        }

        public bool IsFlat() { return Flat; }
        public double Discount() { return Amount; }

        public string Barcode { get; set; } = string.Empty;
        public bool Flat { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public double Amount { get; set; } = 0.0;
        private List<int> RelatedIDs { get; set; } = new List<int>();
    }
}
