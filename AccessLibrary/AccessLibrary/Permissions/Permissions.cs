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

		public enum SystemPermissions
		{

            ///***********************
            /// Deprecated Permissions
            ///***********************

            //// Allows the user to use a register to scan items.
            //UseRegister = 1,

            //// Allows the user to process a payment for the transaction.
            //ProcessPayment = 2,

            //// Allows the user to apply a coupon to the sale.
            //ApplyCoupon = 4,

            //// Allows the user to override predefined prices with an entered value, A reason must be entered as to why the price was changed.
            //PriceOverride = 8,

            //// Allows the user to change the stored amount of an item.
            //ChangeAStockQuantity = 16,

            //// Allows the addition or removal of store products.
            //AddRemoveAProduct = 32,

            //// Allows the user to change the price/brand/etc. of a product.
            //ChangeProductDetails = 64,

            //// Allows the user to change the value of transactions that have happened in the past.
            //ModifyTransaction = 128,

            //// Allows the user to change other users' permissions.
            //ChangePermissions = 256,

            //// Allows the user to override predefined prices with an entered value.
            //PriceOverrideNoReason = 512,

            //// Allows the employee to use Manager functions at the login window
            //UseLoginWindowManagerFunctions = 1024,




            // Register Permissions (NEW)

            // Allows the employee to log into the register system
            LOG_IN_REGISTER = 1,

            // Allows employee to put a cashier's station into 'override mode' 
            /// *for the duration of the current transaction?*
            CAN_OVERRIDE_REGISTER = 2, 

            // Allows the employee to void items from the sale
            CAN_VOID_ITEM = 4, 

            // Allows the employee to void coupons from the sale
            CAN_VOID_COUPON = 8, 

            // Allows the employee to void the entire sale
            CAN_VOID_SALE = 16,

            // Allows the employee to add discounts to items.
            CAN_DISCOUNT_ITEMS = 32, 

            // Allows the employee to process refunds.
            CAN_PROCESS_REFUNDS = 64, 

            // Allows the employee to generate invoices.
            ///Don't know if needed yet 
            CAN_GENERATE_INVOICE = 128,

            // Allows the employee to close either the register or admin programs on the station they are physically at.
            CAN_EXIT_CRACKLE_POP_INTERFACE = 256,

            // Allows the employee to restart the entire station, including the OS. 
            CAN_RESTART_STATION = 512,

            // Allows the employee to use the register after normal business hours (configured separately).
            AFTER_HOURS_REGISTER_ACCESS = 1024, 



            // Admin Console Permission (NEW)

            // Allows the employee to log into the admin console.
            LOG_IN_ADMIN_CONSOLE = 2048, 

            // Allows the employee to view the list of employees and their information.
            VIEW_EMPLOYEE_DATABASE = 4096, 

            // Allows the employee to change the information of other employees. 
            ///Who's information can be changed is based on management level, can only change those below you. 
            CHANGE_EMPLOYEE_DATABASE = 8192,


            // Temporary permission to test manager functions, will be removed later
            ///TO REMOVE
            IS_OWNER = 16384,



            






		}

		// Adds a permission from the employee EmployeeToModify. If the user User does not have permissions to do this, instead throws an exception.
		public static void AddPermission(PointOfSales.Users.Employee User, PointOfSales.Users.Employee EmployeeToModify, SystemPermissions PermissionToAdd)
		{
			if (!CheckPermissions(User, SystemPermissions.CHANGE_EMPLOYEE_DATABASE))
				throw new InvalidOperationException("User " + User.name + " Does not have permissions for this operation.");

			// TODO: Connect to the database, change permissions and rebuild the employee class.
		}

		// Removes a permission from the employee EmployeeToModify. If the user User does not have permissions to do this, instead throws an exception.
		public static void RemovePermissions(PointOfSales.Users.Employee User, PointOfSales.Users.Employee EmployeeToModify, SystemPermissions PermissionToRemove)
		{
			if (!CheckPermissions(User, SystemPermissions.CHANGE_EMPLOYEE_DATABASE))
				throw new InvalidOperationException("User " + User.name + " Does not have permissions for this operation.");

			// TODO: Connect to the database, change permissions and rebuild the employee class.
		}

		// Checks if the user EmployeeToCheck has the required permissions to perform a given operation.
		public static bool CheckPermissions(PointOfSales.Users.Employee EmployeeToCheck, SystemPermissions PermissionToCheck)
		{
			return ((EmployeeToCheck.GetEmployeePermissions() & (int)PermissionToCheck) != 0);
		}




    }
}
