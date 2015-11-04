using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
namespace PointOfSales
{
	public class StoreUser
	{
		//Default constructor.
		public StoreUser()
		{
			ID = -1;
			name = "";
			address = new CivicAddress();
			phoneNumber = "";
			birthday = new DateTime();
		}

		// Overloaded constructor for all parameters.
		public StoreUser(int id, string Name, CivicAddress Address, string PhoneNumber, DateTime Birthday)
		{
			ID = id;
			name = Name;
			address = Address;
			phoneNumber = PhoneNumber;
			birthday = Birthday;
		}

		// Property to hold customer ID number.
		public int ID { get { return m_iD; } set { m_iD = value; } }
		private int m_iD;


		// Property to hold the customer's name.
		public string name { get { return m_name; } set { m_name = value; } }
		private string m_name;

		// Property to hold the customer's address.
		public CivicAddress address { get { return m_address; } set { m_address = value; } }
		private CivicAddress m_address;

		// Property to hold the customer's phone number.
		public string phoneNumber { get { return m_phoneNumber; } set { m_phoneNumber = value; } }
		private string m_phoneNumber;

		// Property to hold the customer's birthday.
		public DateTime birthday { get { return m_birthday; } set { m_birthday = value; } }
		private DateTime m_birthday;
	}
}
