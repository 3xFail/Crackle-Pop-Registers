using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Users;
using System.Device.Location;
namespace AccessLibraryTest
{
	[TestClass]
	public class StoreUserTest
	{
		// Test default constructor.
		[TestMethod]
		public void CreateBlankStoreUser()
		{
			string expectedName = "";
			CivicAddress expectedAddress = new CivicAddress();
			DateTime expectedBirthday = new DateTime();

			Customer storeUser = new Customer();

			Assert.AreEqual(-1, storeUser.ID);
			Assert.AreEqual(expectedName, storeUser.name);
			Assert.AreEqual(expectedAddress.AddressLine1, storeUser.address.AddressLine1);
			Assert.AreEqual(expectedBirthday, storeUser.birthday);
		}

		// Test overloaded constructor with arguments.
		[TestMethod]
		public void CreteStoreUserFromKnownData()
		{
			int id = 4;
			string name = "Sally Sue";
			CivicAddress address = new CivicAddress("1234 Mock dr.", "", "", "City", "Region", "1", "12345", "Oklahoma");
			string phoneNumber = "123-123-1234";
			DateTime birthday = new DateTime(1945, 3, 12);

			Customer storeUser = new Customer(id, name, address, phoneNumber, birthday);

			Assert.AreEqual(id, storeUser.ID);
			Assert.AreEqual(name, storeUser.name);
			Assert.AreEqual(address.AddressLine1, storeUser.address.AddressLine1);
			Assert.AreEqual(phoneNumber, storeUser.phoneNumber);
			Assert.AreEqual(birthday, storeUser.birthday);

		}

		[TestMethod]
		public void AssignProperties()
		{
			int id = 4;
			string name = "Sally Sue";
			CivicAddress address = new CivicAddress("1234 Mock dr.", "", "", "City", "Region", "1", "12345", "Oklahoma");
			string phoneNumber = "123-123-1234";
			DateTime birthday = new DateTime(1945, 3, 12);

			Customer storeUser = new Customer();

			storeUser.ID = id;
			storeUser.name = name;
			storeUser.address = address;
			storeUser.birthday = birthday;
			storeUser.phoneNumber = phoneNumber;

			Assert.AreEqual(id, storeUser.ID);
			Assert.AreEqual(name, storeUser.name);
			Assert.AreEqual(address.AddressLine1, storeUser.address.AddressLine1);
			Assert.AreEqual(phoneNumber, storeUser.phoneNumber);
			Assert.AreEqual(birthday, storeUser.birthday);
		}
	}
}
