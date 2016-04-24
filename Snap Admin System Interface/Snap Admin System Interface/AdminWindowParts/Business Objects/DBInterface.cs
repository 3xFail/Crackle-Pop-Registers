
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpClient;
using System.Xml;
using PointOfSales.Users;
using PointOfSales.Permissions;

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
            if( m_employee.HasPermisison( Permissions.ViewAdminLogs ) )
            {
                object dbnull = DBNull.Value;
                m_connection.Write( "GetLogs_Username @0, @1, @2", username ?? dbnull, start ?? dbnull, end ?? dbnull );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewAdminLogs ) );
        }

        public static void GetAllSales()
        {
            //if( m_employee.HasPermisison( Permissions.ViewSaleCatalog ) )
            {
                m_connection.Write( "GetAllSales" );
            }
            //else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewSaleCatalog ) );
        }

        public static void GetAllCoupons()
        {
            if( m_employee.HasPermisison( Permissions.ViewCouponCatalog ) )
            {
                m_connection.Write( "GetAllCoupons" );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewCouponCatalog ) );
        }

        public static void GetAllEmployees()
        {
            if( m_employee.HasPermisison( Permissions.ViewEmployeeCatalog ) )
            {
                m_connection.Write( "GetAllEmployees" );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewEmployeeCatalog ) );
        }

        public static void GetAllPermissions()
        {
            if( m_employee.HasPermisison( Permissions.ViewPermissions ) )
            {
                m_connection.Write( "GetAllPermissions" );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewPermissions ) );
        }

        public static void AddPermission( string name )
        {
            if( m_employee.HasPermisison( Permissions.ModifyPermissions ) )
            {
                Log( $"Added group \"{name}\"." );
                m_connection.WriteNoResponse( "AddPermission @0", name );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ModifyPermissions ) );
        }

        public static void RemovePermission( string name )
        {
            if( m_employee.HasPermisison( Permissions.ModifyPermissions ) )
            {
                Log( $"Removed group \"{name}\"." );
                m_connection.Write( "RemovePermissionGroup @0", name );

                if( Response[0].Get( "Return" ) == "-1" )
                    throw new ArgumentException( "There are users currently in this group, it cannot be removed." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ModifyPermissions ) );
        }

        public static void ModifyPermissionValue( string permission_group, long value, string permission_name, bool new_value )
        {
            if( m_employee.HasPermisison( Permissions.ModifyPermissions ) )
            {
                m_connection.WriteNoResponse( "ModifyPermissionValue @0, @1", permission_group, value );
                if( new_value )
                    Log( $"Gave \"{permission_name}\" to group \"{permission_group}\"." );
                else
                    Log( $"Removed \"{permission_name}\" from group \"{permission_group}\"." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ModifyPermissions ) );
        }

        public static void RenamePermissionlevel( string new_name, string old_name, long value )
        {
            if( m_employee.HasPermisison( Permissions.ModifyPermissions ) )
            {
                Log( $"Renamed group \"{old_name}\" to \"{new_name}\"." );
                m_connection.WriteNoResponse( "RenamePermissionLevel @0, @1, @2", new_name, old_name, value );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ModifyPermissions ) );
        }

        public static bool UserExists( string username )
        {
            m_connection.Write( "GetPass_Username @0", username );
            return Response.Count != 0;
        }

        public static void SetUserPassword( int ID, string newpass )
        {
            if( m_employee.HasPermisison( Permissions.ResetEmployeePassword ) )
            {
                m_connection.WriteNoResponse( "UpdatePassword_Username @0, @1", ID, PasswordHash.HashPassword( newpass ) );
                Log( $"Changed the password of UserID=\"{ID }\" to \"{new string( '*', newpass.Length )}\"." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ResetEmployeePassword ) );
        }

        public static void SetUserActivity( int ID, bool active )
        {
            if( m_employee.HasPermisison( Permissions.ChangeEmployeeCatalog ) )
            {
                m_connection.WriteNoResponse( "SetUserActivity @0, @1", ID, active ? '1' : '0' );
                Log( $"Set the account of UserID=\"{ID}\" to \"{( active ? "Active" : "Inactive" )}\"." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeEmployeeCatalog ) );
        }

        public static void ModifyCustomer( string firstName, string lastName, string address_1,
            string address_2, string city, string state, string country, string zip, string phoneNumber, string email,
            DateTime DOB, bool active )
        {
            if( m_employee.HasPermisison( Permissions.ChangeCustomerCatalog ) )
            {
                m_connection.Write( "ModifyCust @0, @1, @2, @3, @4, @5, @6, @7, @8, @10, @11, @12",
                    firstName, lastName, address_1, address_2, city, state, country, zip, phoneNumber, email, active, DOB );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeCustomerCatalog ) );
        }

        public static void GetAllCustomers()
        {
            if( m_employee.HasPermisison( Permissions.ViewCustomerCatalog ) )
            {
                m_connection.Write( "GetAllCustomers" );
            }
            else throw new UnauthorizedAccessException( "You do not have permission to view the customer catalog" );
        }

        public static void ChangePermissions( string name, int id, string permission )
        {
            if( m_employee.HasPermisison( Permissions.ChangeEmployeeCatalog ) )
            {
                Log( $"Set the permissions of username=\"{name}\" to \"{permission}\"" );
                m_connection.WriteNoResponse( "ChangePermission_ID_Permission @0, @1", id, permission );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeEmployeeCatalog ) );
        }

        public static void AddEmployee( string firstName, string lastName, string username,
            string email, string password, string authorizationLevel, DateTime DOB,
            string phoneNumber, string address_1, string address_2, string city, string state, string country,
            string zip)
        {
            if( m_employee.HasPermisison( Permissions.ChangeEmployeeCatalog ) )
            {
                string dob_string = DOB == null ? null : DOB.ToString();

                password = PasswordHash.HashPassword( password );

                m_connection.Write( "AddUser @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14",
                    firstName, lastName, username, password, phoneNumber, authorizationLevel, "1", dob_string, address_1, address_2, city, state, country, zip, email );

                if( Response[0].Get( "UserID" ) == "-1" ) //otherwise the UserID returned is the ID of the account just created
                    throw new InvalidOperationException( $"Username \"{username}\" already exists." );
                else if( Response[0].Get( "UserID" ) == "-2" )
                    throw new InvalidOperationException( $"User with phone number \"{phoneNumber}\" already exists." );
                else
                    Log( $"Added user \"{username}\" with authorization level \"{authorizationLevel}\"." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeEmployeeCatalog ) );
        }

        public static void AddItem( string name, decimal price, string barcode, int quantity )
        {
            if( m_employee.HasPermisison( Permissions.ChangeItemCatalog ) )
            {
                m_connection.Write( "AddItem @0, @1, @2, @3, @4", name, price, barcode, "1", quantity );

                if( Response[0].Get( "ProductID" ) == "-1" )
                    throw new InvalidOperationException( $"Item with barcode \"{barcode}\" already exists." );
                else
                    Log( $"Added item \"{name}\" for {price.ToString( "C2" )}." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeItemCatalog ) );
        }

        public static void AddCust( string firstName, string lastName, string address_1, 
            string address_2, string city, string state, string country, string zip, string phoneNumber, string email,
            DateTime DOB)
        {
            if( m_employee.HasPermisison( Permissions.ChangeCustomerCatalog ) )
            {
                string dob_string = DOB == null ? string.Empty : DOB.ToString();

                m_connection.Write( "AddCust @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12",
                    firstName, lastName, address_1, address_2, city, state, country, zip, phoneNumber, email, DBNull.Value, "1", dob_string );

                if( Response[0].Get( "CustID" ) == "-1" )
                    throw new InvalidOperationException( $"User with phone number {phoneNumber} already exists." );
                else
                    Log( $"Added customer \"{firstName}  {lastName}\" with phone number \"{phoneNumber}\"." );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeCustomerCatalog ) );
        }

        public static void GetAllProducts()
        {
            if( m_employee.HasPermisison( Permissions.ViewItemCatalog ) )
            {
                m_connection.Write( "GetAllItemsInProducts" );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewItemCatalog ) );
        }

        public static void ModifyItem( int ID, string name, string barcode, decimal price, bool active, int quantity )
        {
            m_connection.Write( "GetItem_ID @0", ID );

            if( Response.Count != 1 )
                throw new ArgumentException( $"Item with ID= {ID} does not exist" );

            string old_name = Response[0].Get( "Name" );
            string old_barcode = Response[0].Get( "Barcode" );
            decimal old_price = decimal.Parse( Response[0].Get( "Price" ) );
            bool old_active = Response[0].Get( "Active" )[0] == '1';
            int old_quantity = int.Parse( Response[0].Get( "Quantity" ) );

            if( quantity != old_quantity && !m_employee.HasPermisison( Permissions.ChangeItemQuantity ) )
                throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeItemQuantity ) );

            if( old_active != active || old_barcode != barcode || old_price != price || old_name != name )
                if( !m_employee.HasPermisison( Permissions.ChangeItemCatalog ) )
                    throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeItemCatalog ) );

            m_connection.Write( "Modify_Item @0, @1, @2, @3, @4, @5", ID, name, barcode, price, active, quantity );

            if( Response[0].Get( "ProductID" ) == "-1" )
                throw new InvalidOperationException( $"Item ({m_connection.Response[0].Get( "Name" )}) with barcode \"{barcode}\" already exists." );

            if( old_name != name )
                Log( $"Modified item ID=\"{ID}\"s name from \"{old_name}\" to \"{name}\"" );

            if( old_price != price )
                Log( $"Modified item ID=\"{ID}\"s price from {old_price.ToString( "C" )} to {price.ToString( "C" )}" );

            if( old_barcode != barcode )
                Log( $"Modified item ID=\"{ID}\"s barcode from \"{old_barcode}\" to \"{barcode}\"" );

            if( old_active != active )
                Log( $"Modified item ID=\"{ID}\"s active state from {( old_active ? "Active" : "Inactive" )} to {( active ? "Active" : "Inactive" )}" );

            if( old_quantity != quantity )
                Log( $"Modified item ID=\"{ID}\"s quantity from {old_quantity} to {quantity}" );
        }

        public static void RemoveItem( int ID )
        {
            m_connection.Write( "RemoveItem_ProductID @0", ID );
            if( Response[0].Get( "Return" ) == "-1" )
                throw new InvalidOperationException( $"Item with ID {ID} does not exist." );
            if( Response[0].Get( "Return" ) == "-2" )
                throw new InvalidOperationException( $"Item with ID {ID} has been sold and cannot be removed. Set it inactive instead." );
            else
                Log( $"Removed item with ID=\"{ID}\"." );
        }
        public static void GetItemID( string barcode )
        {
            if( m_employee.HasPermisison( Permissions.ChangeItemCatalog ) )
            {
                m_connection.Write( "GetItemID_Barcode @0", barcode );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ChangeItemCatalog ) );
        }

        public static void GetUsageStatistics( DateTime? start, DateTime? end )
        {
            if( m_employee.HasPermisison( Permissions.ViewRegisterLogs ) )
            {
                //If start is null, we get the statistics from one month ago to one day from now
                m_connection.Write( "GetUsageStatistics @0, @1", start ?? DateTime.Now.AddMonths( -1 ), end ?? DateTime.Now.AddDays( 1 ) );
            }
            else throw new UnauthorizedAccessException( Permissions.ErrorMessage( Permissions.ViewRegisterLogs ) );
        }

        public static void GetAllLogos()
        {
            m_connection.Write("GetAllLogos");
        }

        public static void AddLogo (string logoString)
        {
            m_connection.Write("AddLogo @0", logoString);
        }

        public static void ChangeLogo(int ID, string logoString)
        {
            m_connection.WriteNoResponse("ChangeLogo @0, @1", ID, logoString);
        }

        public static void GetOrdersCatalog()
        {
            m_connection.Write("GetOrdersInfo ");
        }

        public static XmlNodeList Response { get { return m_connection.Response; } }
    }
}
