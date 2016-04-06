using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using PointOfSales.Permissions;

namespace PointOfSales.Users
{
	// An employee is someone who works for the store. Inherits from StoreUser.
	public class Employee : StoreUser
	{
		public Employee()
			: base()
		{ }

		public Employee(int id, string Name, CivicAddress Address, string PhoneNumber, DateTime Birthday, ulong permissions, string permission_group)
			: base(id, Name, Address, PhoneNumber, Birthday)
		{
            Permissions = permissions;
            PermissionGroup = permission_group;
		}

        public bool HasPermisison( string permission )
        {
            return PointOfSales.Permissions.Permissions.CheckPermissions( this, permission );
        }

        public void SetPermission( string permission, bool value )
        {
            PointOfSales.Permissions.Permissions.SetPermission( ref Permissions, value, permission );
        }

        public void TogglePermisison( string permission )
        {
            PointOfSales.Permissions.Permissions.TogglePermission( ref Permissions, permission );
        }


		// Employees by default are allowed to use the register.
		// See PointOfSales.Permissions for details.
		public ulong Permissions;
        public string PermissionGroup { get; set; }
	}
}
