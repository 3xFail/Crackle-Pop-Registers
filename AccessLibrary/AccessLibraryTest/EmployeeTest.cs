using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Permissions;

namespace AccessLibraryTest
{
	[TestClass]
	public class EmployeeTest
	{

		//[TestMethod]
		//public void ChangePermissionsTest()
		//{
		//	PointOfSales.Users.Employee SallySue = new PointOfSales.Users.Employee(-1, "SallySue", null, "5", new DateTime(1,2,3), 256);

		//	Permissions.AddPermission(SallySue, SallySue, Permissions.PointOfSalesPermissions.UseRegister);

		//	Assert.IsTrue(Permissions.CheckPermissions(SallySue, Permissions.PointOfSalesPermissions.UseRegister));
		//}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void EmployeeInvalidPermissionsTest()
		{
			PointOfSales.Users.Employee SallySue = new PointOfSales.Users.Employee(-1, "SallySue", null, "5", new DateTime(1, 2, 3), 1);
			//Removing permissions to change permissions before changing permissions.
			Permissions.RemovePermissions(SallySue, SallySue, Permissions.SystemPermissions.ChangePermissions);
			Permissions.AddPermission(SallySue, SallySue, Permissions.SystemPermissions.UseRegister);
		}
    }
}
