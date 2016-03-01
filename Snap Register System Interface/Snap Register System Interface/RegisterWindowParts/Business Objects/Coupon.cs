using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class Coupon
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
        public Coupon(string coup_id, string barcode, string name, float price_change, bool active)
        {
            m_coupon_code = coup_id;
            m_related_barcode = barcode;
            m_discount = price_change;
            m_active = active;
            m_name = name;
        }

        public Coupon(Coupon source)
        {
            m_coupon_code = source.m_coupon_code;
            m_related_barcode = source.m_related_barcode;
            m_discount = source.m_discount;
            m_active = source.m_active;
            m_name = source.m_name;
        }

        public float m_discount { get; set; }
        public string m_coupon_code { get; set; }
        public string m_related_barcode { get; set; }
        public bool m_active { get; set; }

        public string m_name { get; set; }
    }
}
