using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace PointOfSales.Users
{
	// An employee is someone who works for the store. Inherits from StoreUser.
	public class Employee : StoreUser
	{
		public Employee()
			: base()
		{ }

		public Employee(int id, string Name, CivicAddress Address, string PhoneNumber, DateTime Birthday)
			: base(id, Name, Address, PhoneNumber, Birthday)
		{ }

		public int EmployeePermissions;
	}
}
