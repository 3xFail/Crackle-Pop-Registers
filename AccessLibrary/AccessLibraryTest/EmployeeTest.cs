using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
	}
}
