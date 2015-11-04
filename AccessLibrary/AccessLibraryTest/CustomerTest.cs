using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales;
using System.Device.Location;
namespace AccessLibraryTest
{
	[TestClass]
	public class CustomerTest
	{
		// Test default constructor.
		[TestMethod]
		public void CreateBlankCustomer()
		{
			string expectedName = "";
			CivicAddress expectedAddress = new CivicAddress();
			DateTime expectedBirthday = new DateTime();

			Customer customer = new Customer();

			Assert.AreEqual(-1, customer.ID);
			Assert.AreEqual(expectedName, customer.name);
			Assert.AreEqual(expectedAddress.AddressLine1, customer.address.AddressLine1);
			Assert.AreEqual(expectedBirthday, customer.birthday);
		}

		// Test overloaded constructor with arguments.
		[TestMethod]
		public void CreteCustomerFromKnownData()
		{
			int id = 4;
			string name = "Sally Sue";
			CivicAddress address = new CivicAddress("1234 Mock dr.", "", "", "City", "Region", "1", "12345", "Oklahoma");
			string phoneNumber = "123-123-1234";
			DateTime birthday = new DateTime(1945, 3, 12);

			Customer customer = new Customer(id, name, address, phoneNumber, birthday);

			Assert.AreEqual(id, customer.ID);
			Assert.AreEqual(name, customer.name);
			Assert.AreEqual(address.AddressLine1, customer.address.AddressLine1);
			Assert.AreEqual(phoneNumber, customer.phoneNumber);
			Assert.AreEqual(birthday, customer.birthday);

		}

		[TestMethod]
		public void AssignProperties()
		{
			int id = 4;
			string name = "Sally Sue";
			CivicAddress address = new CivicAddress("1234 Mock dr.", "", "", "City", "Region", "1", "12345", "Oklahoma");
			string phoneNumber = "123-123-1234";
			DateTime birthday = new DateTime(1945, 3, 12);

			Customer customer = new Customer();

			customer.ID = id;
			customer.name = name;
			customer.address = address;
			customer.birthday = birthday;
			customer.phoneNumber = phoneNumber;

			Assert.AreEqual(id, customer.ID);
			Assert.AreEqual(name, customer.name);
			Assert.AreEqual(address.AddressLine1, customer.address.AddressLine1);
			Assert.AreEqual(phoneNumber, customer.phoneNumber);
			Assert.AreEqual(birthday, customer.birthday);
		}
	}
}
