using SnapRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap_Register_System_Interface.RegisterWindowParts.Business_Objects
{
    public class Customer
    {
        public Customer()
        {
            fname = null;
            lname = null;
            phone_number = null;
            rewards_id = 0;
            cust_id = 0;
        }

        public Customer(string first_name, string last_name, string phone, int rew_id, int rew_points, int customer_id)
        {
            fname = first_name;
            lname = last_name;
            phone_number = phone;
            rewards_id = rew_id;
            rewards_points = rew_points;
            cust_id = customer_id;
        }

        public string fname { get; set; } = null;
        public string lname { get; set; } = null;
        public string phone_number { get; set; } = null;
        public int rewards_id { get; set; }
        public int rewards_points { get; set; }
        public int cust_id { get; set; }
        
    }
}
