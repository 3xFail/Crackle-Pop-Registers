using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace AccessLibraryTest.MockConnections
{
	/// ****************************
	/// This classes purpose is to fake a connection to the database without making any actual connection.
	/// Instead this class accesses the test solutions app.config and stores/loads the data from there.
	/// This database makes no attempt to encrypt or store information in a protected manner.
	/// ****************************
	public class MockLogin
	{
		public static PointOfSales.Users.Employee Login(string UserName, string Password)
		{
			string[] UserData = AllUsers[UserName].Split(',');

			if (Password != UserData[1])
				throw new InvalidOperationException("Incorrect Password");

			PointOfSales.Users.Employee loggingInEmployee = new PointOfSales.Users.Employee(int.Parse(UserData[0]),
																							UserData[2],
																							new System.Device.Location.CivicAddress(UserData[3], UserData[4], UserData[5], UserData[6], UserData[7], UserData[8], UserData[9], UserData[10]),
                                                                                            UserData[11],
																							new DateTime(int.Parse(UserData[12]), int.Parse(UserData[13]), int.Parse(UserData[14]))
																							);

			loggingInEmployee.EmployeePermissions = int.Parse(UserData[15]);

			return loggingInEmployee;
		}

		private static System.Collections.Specialized.NameValueCollection AllUsers = ConfigurationManager.GetSection("Users") as System.Collections.Specialized.NameValueCollection;
	}
}
