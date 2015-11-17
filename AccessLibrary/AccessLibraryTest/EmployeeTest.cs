using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Permissions;
namespace AccessLibraryTest
{
	[TestClass]
	public class EmployeeTest
	{
		[TestMethod]
		public void EmployeeLoginTest()
		{
			PointOfSales.Users.Employee SallySue = MockConnections.MockLogin.Login("SallySue", "abc123");

			Assert.AreEqual("123-123-1234", SallySue.phoneNumber);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void EmployeeIncorrectLoginTest()
		{
			PointOfSales.Users.Employee SallySue = MockConnections.MockLogin.Login("SallySue", "WRONG_PASSWORD");
		}

		[TestMethod]
		public void ChangePermissionsTest()
		{
			PointOfSales.Users.Employee SallySue = MockConnections.MockLogin.Login("SallySue", "abc123");

			Permissions.AddPermission(SallySue, SallySue, Permissions.PointOfSalesPermissions.UseRegister);

			Assert.IsTrue(Permissions.CheckPermissions(SallySue, Permissions.PointOfSalesPermissions.UseRegister));
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void EmployeeInvalidPermissionsTest()
		{
			PointOfSales.Users.Employee SallySue = MockConnections.MockLogin.Login("SallySue", "abc123");
			//Removing permissions to change permissions before changing permissions.
			Permissions.RemovePermissions(SallySue, SallySue, Permissions.PointOfSalesPermissions.ChangePermissions);
			Permissions.AddPermission(SallySue, SallySue, Permissions.PointOfSalesPermissions.UseRegister);
		}
    }
}
