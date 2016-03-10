
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpClient;

namespace SnapRegisters
{
    static class DBInterface
    {
        public static connection_session m_connection;
        public static void AddEmployee( string firstName, string lastName, string username,
            string email, string password, string authorizationLevel, DateTime DOB,
            string phoneNumber, string address_1, string address_2, string city, string state, string country,
            string zip)
        {
            string dob_string = DOB == null ? string.Empty : DOB.ToString();

            password = PasswordHash.Hash(username, password);

            m_connection.write(string.Format("AddUser \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\" \"{10}\" \"{11}\" \"{12}\" \"{13}\" \"{14}\" ",
                firstName, lastName, username, password, phoneNumber, authorizationLevel, "1", dob_string, address_1, address_2, city, state, country, zip, email));

            if (m_connection.Response[0].Get("UserID") == "-1") //otherwise the UserID returned is the ID of the account just created
                throw new InvalidOperationException("Username already exists. Please choose another.");    
        }

        public static void AddItem( string name, string price, string barcode)
        {

            m_connection.write(string.Format("AddItem \"{0}\" \"{1}\" \"{2}\" \"{3}\" ", name, price, barcode, "1"));

            if (m_connection.Response[0].Get("ProductID") == "-1")
                throw new InvalidOperationException("Item with barcode \"" + barcode + "\" already exists.");
        }

        public static void AddCust( string firstName, string lastName, string address_1, 
            string address_2, string city, string state, string country, string zip, string phoneNumber, string email,
            DateTime DOB)
        {
            string dob_string = DOB == null ? string.Empty : DOB.ToString();

            m_connection.write(string.Format("AddCust \"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\" \"{5}\" \"{6}\" \"{7}\" \"{8}\" \"{9}\" \"{10}\" \"{11}\" \"{12}\" ",
                firstName, lastName, address_1, address_2, city, state, country, zip, phoneNumber, email, "NULL", "1", dob_string));

            if (m_connection.Response[0].Get("UserID") == "-1")
                throw new InvalidOperationException("User with phone number \"" + phoneNumber + "\" already exists.");
        }

        //internal static void AddEmployee(object connection, object firstName, object lastName, object username, object email, object password, object )
        //{
        //    throw new NotImplementedException();
        //}
    }
}
