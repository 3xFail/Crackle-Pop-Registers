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

        public static readonly string RegisterLogIn = "Register Login";
        public static readonly string RegisterOverride = "Register Override";
        public static readonly string CanVoidItem = "Can Void Item";
        public static readonly string CanVoidCoupon = "Can Void Coupon";
        public static readonly string CanVoidSale = "Can Void Sale";
        public static readonly string CanDiscountItems = "Can Discount Items";
        public static readonly string CanProcessRefunds = "Can Process Refunds";
        public static readonly string CanGenerateInvoice = "Can Generate Invoice";
        public static readonly string CanExitInterface = "Can Exit Interface";
        public static readonly string CanRestartStation = "Can Restart Station";
        public static readonly string CanShutdownStation = "Can Shut Down Station";
        public static readonly string AfterHoursLogin = "After Hours Login";
        public static readonly string AdminLogIn = "Admin Program Login";
        public static readonly string ViewEmployeeCatalog = "View Employee Catalog";
        public static readonly string ChangeEmployeeCatalog = "Change Employee Information/Add Employees";
        public static readonly string ResetEmployeePassword = "Reset Employee Password";
        public static readonly string ViewItemCatalog = "View Item Catalog";
        public static readonly string ChangeItemCatalog = "Change Item Information";
        public static readonly string ChangeItemQuantity = "Change Item Quantity";
        public static readonly string ViewRegisterLogs = "View Register Logs";
        public static readonly string ViewAdminLogs = "View Admin Logs";
        public static readonly string ViewPermissions = "View Permissions";
        public static readonly string ModifyPermissions = "Modify Permissions";
        public static readonly string ViewCustomerCatalog = "View Customer Catalog";
        public static readonly string ChangeCustomerCatalog = "Modify Customer Information";
        public static readonly string ViewCouponCatalog = "View Coupon Catalog";
        public static readonly string ChangeCouponCatalog = "Change Coupon Catalog";
        public static readonly string CanAccessManagerFunctions = "Can Access Manager Functions";
        public static readonly string CanAddNewItem = "Can Add New Item";

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
            CanShutdownStation,
            AfterHoursLogin,
            AdminLogIn,
            ViewEmployeeCatalog,
            ChangeEmployeeCatalog,
            ResetEmployeePassword,
            ViewItemCatalog,
            ChangeItemCatalog,
            ChangeItemQuantity,
            ViewRegisterLogs,
            ViewAdminLogs,
            ViewPermissions,
            ModifyPermissions,
            ViewCustomerCatalog,
            ChangeCustomerCatalog,
            ViewCouponCatalog,
            ChangeCouponCatalog,
            CanAccessManagerFunctions,
            CanAddNewItem

        };

        private static readonly Dictionary<string, string> _ErrorStrings = new Dictionary<string, string>()
        {
            { RegisterLogIn, "You do not have permission to log into the register" },
            { RegisterOverride, "You do not have permission to perform a register override" },
            { CanVoidItem, "You do not have permission to void items" },
            { CanVoidCoupon, "You do not have permission to void coupons" },
            { CanVoidSale, "You do not have permission to void sales" },
            { CanDiscountItems, "You do not have permission to discount items" },
            { CanProcessRefunds, "You do not have permission to process refunds" },
            { CanGenerateInvoice, "You do not have permission to generate an invoice" },
            { CanExitInterface, "You do not have permission to exit the interface" },
            { CanRestartStation, "You do not have permission to restart the station" },
            { CanShutdownStation, "You do not have permission to shut down the station" },
            { AfterHoursLogin, "You do not have permission to log in after store hours" },
            { AdminLogIn, "You do not have permission to log into the admin program" },
            { ViewEmployeeCatalog, "You do not have permission to view the employee catalog" },
            { ChangeEmployeeCatalog, "You do not have permission to modify employee accounts" },
            { ResetEmployeePassword, "You do not have permission to reset employee accounts" },
            { ViewItemCatalog, "You do not have permssion to view the item catalog" },
            { ChangeItemCatalog, "You do not have permission to modify item information" },
            { ChangeItemQuantity, "You do not have permission to modify item quantity" },
            { ViewAdminLogs, "You do not have permission to view the admin logs" },
            { ViewRegisterLogs, "You do not have permission to view the register logs" },
            { ViewPermissions, "You do not have permission to view permissions" },
            { ModifyPermissions, "You do not have permission to modify permissions" },
            { ViewCustomerCatalog, "You do not have permission to view the customer catalog" },
            { ChangeCustomerCatalog, "You do not have permission to modify customer information" },
            { ViewCouponCatalog, "You do not have permission to view the coupon catalog" },
            { ChangeCouponCatalog, "You do not have permission to change the coupon catalog" },
            { CanAccessManagerFunctions, "You do not have permission to access the manager functions" },
            { CanAddNewItem, "You do not have permission to add a new item to the catalog" }
        };

        public static string ErrorMessage( string permission )
        {
            return _ErrorStrings[permission];
        }

        public static void AddPermission( Users.Employee User, string PermissionToAdd)
		{
            SetPermission( ref User.Permissions, true, PermissionToAdd );
		}

		public static void RemovePermissions( Users.Employee User, string PermissionToRemove )
		{
            SetPermission( ref User.Permissions, false, PermissionToRemove );
        }

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
