using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class Coupon
    {
        public Coupon(string coup_id, string barcode, string name, float price_change, bool active)
        {
            m_coupon_code = coup_id;
            m_related_barcode = barcode;
            m_discount = price_change;
            m_active = active;
            m_name = name;
        }

        public float m_discount { get; set; }
        public string m_coupon_code { get; set; }
        public string m_related_barcode { get; set; }
        public bool m_active { get; set; }

        public string m_name { get; set; }
    }
}
