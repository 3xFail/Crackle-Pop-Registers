using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSales.Permissions
{
	// Static class allows the updating and defining of users' permissions.
	public static class Permissions
	{

        public static readonly string RegisterLogIn = "RegisterLogIn";
        public static readonly string RegisterOverride = "RegisterOverride";
        public static readonly string CanVoidItem = "CanVoidItem";
        public static readonly string CanVoidCoupon = "CanVoidCoupon";
        public static readonly string CanVoidSale = "CanVoidSale";
        public static readonly string CanDiscountItems = "CanDiscountItems";
        public static readonly string CanProcessRefunds = "CanProcessRefunds";
        public static readonly string CanGenerateInvoice = "CanGenerateInvoice";
        public static readonly string CanExitInterface = "CanExitInterface";
        public static readonly string CanRestartStation = "CanRestartStation";
        public static readonly string AfterHoursLogin = "AfterHoursLogin";
        public static readonly string AdminLogIn = "AdminLogIn";
        public static readonly string ViewEmployeeCatalog = "ViewEmployeeCatalog";
        public static readonly string ChangeEmployeeCatalog = "ChangeEmployeeCatalog";
        public enum SystemPermissions
		{

            // Register Permissions (NEW)

            // Allows the employee to log into the register system
            LOG_IN_REGISTER = 0,

            // Allows employee to put a cashier's station into 'override mode' 
            /// *for the duration of the current transaction?*
            CAN_OVERRIDE_REGISTER = 1, 

            // Allows the employee to void items from the sale
            CAN_VOID_ITEM = 2, 

            // Allows the employee to void coupons from the sale
            CAN_VOID_COUPON = 3, 

            // Allows the employee to void the entire sale
            CAN_VOID_SALE = 4,

            // Allows the employee to add discounts to items.
            CAN_DISCOUNT_ITEMS = 5, 

            // Allows the employee to process refunds.
            CAN_PROCESS_REFUNDS = 6,

            // Allows the employee to generate invoices.
            ///Don't know if needed yet 
            CAN_GENERATE_INVOICE = 7,

            // Allows the employee to close either the register or admin programs on the station they are physically at.
            CAN_EXIT_CRACKLE_POP_INTERFACE = 8,

            // Allows the employee to restart the entire station, including the OS. 
            CAN_RESTART_STATION = 9,

            // Allows the employee to use the register after normal business hours (configured separately).
            AFTER_HOURS_REGISTER_ACCESS = 10, 


            // Admin Console Permission (NEW)

            // Allows the employee to log into the admin console.
            LOG_IN_ADMIN_CONSOLE = 11, 

            // Allows the employee to view the list of employees and their information.
            VIEW_EMPLOYEE_DATABASE = 12, 

            // Allows the employee to change the information of other employees. 
            ///Who's information can be changed is based on management level, can only change those below you. 
            CHANGE_EMPLOYEE_DATABASE = 13,


            // Temporary permission to test manager functions, will be removed later
            ///TO REMOVE
            IS_OWNER = 14,

		}

        public static readonly List<string> _Permissions = new List<string>()
        {
            RegisterLogIn,
            RegisterOverride,
            CanVoidItem,
            CanVoidCoupon,
            CanVoidSale,
            CanDiscountItems,
            CanProcessRefunds,
            CanGenerateInvoice,
            CanExitInterface,
            CanRestartStation,
            AfterHoursLogin,
            AdminLogIn,
            ViewEmployeeCatalog,
            ChangeEmployeeCatalog

        };

		// Adds a permission from the employee EmployeeToModify. If the user User does not have permissions to do this, instead throws an exception.
		public static void AddPermission( Users.Employee User, Users.Employee EmployeeToModify, string PermissionToAdd)
		{
            if( !CheckPermissions( User, ChangeEmployeeCatalog ) ) 
				throw new InvalidOperationException("User " + User.name + " Does not have permissions for this operation.");

			// TODO: Connect to the database, change permissions and rebuild the employee class.
		}

		// Removes a permission from the employee EmployeeToModify. If the user User does not have permissions to do this, instead throws an exception.
		public static void RemovePermissions( Users.Employee User, Users.Employee EmployeeToModify, string PermissionToRemove)
		{
			if (!CheckPermissions( User, ChangeEmployeeCatalog ) )
				throw new InvalidOperationException("User " + User.name + " Does not have permissions for this operation.");

			// TODO: Connect to the database, change permissions and rebuild the employee class.
		}

		// Checks if the user EmployeeToCheck has the required permissions to perform a given operation.
		public static bool CheckPermissions( Users.Employee EmployeeToCheck, string PermissionToCheck )
		{
            return GetBit( _Permissions.IndexOf( PermissionToCheck ), EmployeeToCheck.Permissions );
		}
        
        public static bool CheckPermissions( ulong PermissionsFlags, string PermissionToCheck )
        {
            return GetBit( _Permissions.IndexOf( PermissionToCheck ), PermissionsFlags );
        }

        public static void SetPermission( ref ulong PermissionsFlags, bool value, string PermissionToSet )
        {
            SetBit( _Permissions.IndexOf( PermissionToSet ), value, ref PermissionsFlags );
        }

        public static void TogglePermission( ref ulong PermissionsFlags, string PermissionToToggle )
        {
            ToggleBit( _Permissions.IndexOf( PermissionToToggle ), ref PermissionsFlags );
        }

        private static bool GetBit( int idx, ulong flags )
        {
            return ( ( flags >> idx ) & 1 ) != 0;
        }

        private static void SetBit( int idx, bool value, ref ulong flags )
        {
            if( value ) //if true set the bit to 1
                flags |= (ulong)1 << idx;
            else //if false set the bit to 0
                flags &= ~( (ulong)1 << idx );
        }

        private static void ToggleBit( int idx, ref ulong flags )
        {
            flags ^= (ulong)1 << idx; //flip the bit. If 1 set to 0, if 0 set to 1
        }

    }
}
