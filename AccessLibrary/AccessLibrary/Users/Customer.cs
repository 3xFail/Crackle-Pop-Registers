using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace PointOfSales.Users
{
	// A customer is someone who is served at the store. Inherits from StoreUser.
    public class Customer : StoreUser
    {
		public Customer()
			: base()
		{}

		public Customer(int id, string Name, CivicAddress Address, string PhoneNumber, DateTime Birthday)
			: base(id, Name, Address, PhoneNumber, Birthday)
		{}

	}
}
