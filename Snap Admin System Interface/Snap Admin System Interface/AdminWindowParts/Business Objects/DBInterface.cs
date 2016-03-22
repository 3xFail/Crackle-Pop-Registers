
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpClient;
using System.Xml;
using PointOfSales.Users;

namespace SnapRegisters
{
    static class DBInterface
    {
        public static ConnectionSession m_connection { get; set; }
        public static Employee m_employee { get; set; }

        public static void Log( string message )
        {
            m_connection.WriteNoResponse( "AddLog @0, @1", m_employee.ID, message );
        }

        public static void GetLogs( string username, DateTime? start, DateTime? end )
        {
            object dbnull = DBNull.Value;
            m_connection.Write( "GetLogs_Username @0, @1, @2", username ?? dbnull, start ?? dbnull, end ?? dbnull );
        }

        public static void GetAllEmployees()
        {
            m_connection.Write( "GetAllEmployees" );
        }

        public static void GetAllPermissions()
        {
            m_connection.Write( "GetAllPermissions" );
        }

        public static void SetUserPassword( int ID, string newpass )
        {
            m_connection.WriteNoResponse( "UpdatePassword_Username @0, @1", ID, PasswordHash.HashPassword( newpass ) );
            Log( "Changed the password of UserID=\"" + ID + "\" to \"" + new string( '*', newpass.Length ) + "\"." );
        }

        public static void SetUserActivity( int ID, bool active )
        {
            m_connection.WriteNoResponse( "SetUserActivity @0, @1", ID, active ? '1' : '0' );
            Log( "Set the account of UserID=\"" + ID + "\" to \"" + ( active ? "Active" : "Inactive" ) + "\"." );
        }

        public static void ModifyCustomer( string firstName, string lastName, string address_1,
            string address_2, string city, string state, string country, string zip, string phoneNumber, string email,
            DateTime DOB, bool active )
        {
            m_connection.Write( "ModifyCust @0, @1, @2, @3, @4, @5, @6, @7, @8, @10, @11, @12",
                firstName, lastName, address_1, address_2, city, state, country, zip, phoneNumber, email, active, DOB );
        }

        public static void GetAllCustomers()
        {
            m_connection.Write( "GetAllCustomers" );
        }

        public static void ChangePermissions( int ID, string permission )
        {
            m_connection.WriteNoResponse( "ChangePermission_ID_Permission @0, @1", ID, permission );
            Log( "Set the permissions of UserID=\"" + ID + "\" to \"" + permission + "\"." );
        }

        public static void AddEmployee( string firstName, string lastName, string username,
            string email, string password, string authorizationLevel, DateTime DOB,
            string phoneNumber, string address_1, string address_2, string city, string state, string country,
            string zip)
        {
            string dob_string = DOB == null ? null : DOB.ToString();

            password = PasswordHash.HashPassword( password );

            m_connection.Write("AddUser @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14",
                firstName, lastName, username, password, phoneNumber, authorizationLevel, "1", dob_string, address_1, address_2, city, state, country, zip, email);

            if( Response[0].Get( "UserID" ) == "-1" ) //otherwise the UserID returned is the ID of the account just created
                throw new InvalidOperationException( "Username \"" + username + "\" already exists." );
            else if( Response[0].Get( "UserID" ) == "-2" )
                throw new InvalidOperationException( "User with phone number \"" + phoneNumber + "\" already exists." );
            else
                Log( "Added user \"" + username + "\" with authorization level \"" + authorizationLevel + "\"." );
        }

        public static void AddItem( string name, decimal price, string barcode, int quantity )
        {

            m_connection.Write( "AddItem @0, @1, @2, @3, @4", name, price, barcode, "1", quantity );

            if( Response[0].Get( "ProductID" ) == "-1" )
                throw new InvalidOperationException( "Item with barcode \"" + barcode + "\" already exists." );
            else
                Log( "Added item \"" + name + "\" for \"" + price.ToString( "C2" ) + "\"." );
        }

        public static void AddCust( string firstName, string lastName, string address_1, 
            string address_2, string city, string state, string country, string zip, string phoneNumber, string email,
            DateTime DOB)
        {
            string dob_string = DOB == null ? string.Empty : DOB.ToString();

            m_connection.Write("AddCust @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12",
                firstName, lastName, address_1, address_2, city, state, country, zip, phoneNumber, email, DBNull.Value, "1", dob_string);

            if( Response[0].Get( "CustID" ) == "-1" )
                throw new InvalidOperationException( "User with phone number \"" + phoneNumber + "\" already exists." );
            else
                Log( "Added customer \"" + firstName + ' ' + lastName + "\" with phone number \"" + phoneNumber + "\"." );
        }

        public static void GetAllProducts()
        {
            // no passins on this one
            m_connection.Write("GetAllItemsInProducts");
        }

        public static void ModifyItem( int ID, string name, string barcode, decimal price, bool active, int quantity )
        {
            m_connection.Write( "Modify_Item @0, @1, @2, @3, @4, @5", ID, name, barcode, price, active, quantity );

            if( Response[0].Get( "ProductID" ) == "-1" )
                throw new InvalidOperationException( "Item (" + m_connection.Response[0].Get( "Name" ) + ") with barcode \"" + barcode + "\" already exists." );
            else
                Log( "Modified item ID \"" + ID + "\" to have name=\"" + name + "\", price=\"" + price.ToString( "C2" ) + "\", barcode=\"" + barcode + "\", active=\"" + ( active ? "true" : "false" ) + "\", and quantity=\"" + quantity + "\"." );
        }

        public static void RemoveItem( int ID )
        {
            m_connection.Write( "RemoveItem_ProductID @0", ID );
            if( Response[0].Get( "Return" ) == "-1" )
                throw new InvalidOperationException( "Item with ID \"" + ID + "\" does not exist." );
            if( Response[0].Get( "Return" ) == "-2" )
                throw new InvalidOperationException( "Item with ID \"" + ID + "\" has been sold and cannot be removed. Set it inactive instead." );
            else
                Log( "Removed item with ID=\"" + ID + "\"." );
        }
        public static void GetItemID( string barcode )
        {
            m_connection.Write( "GetItemID_Barcode @0", barcode );
        }

        public static void GetUsageStatistics( DateTime? start, DateTime? end )
        {
            //If start is null, we get the statistics from one month ago to one day from now
            m_connection.Write( "GetUsageStatistics @0, @1", start ?? DateTime.Now.AddMonths( -1 ), end ?? DateTime.Now.AddDays( 1 ) );
        }

        public static XmlNodeList Response { get { return m_connection.Response; } }

    }
}
