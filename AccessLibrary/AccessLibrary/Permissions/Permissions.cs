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
		public enum PointOfSalesPermissions
		{
			// Allows the user to use a register to scan items.
			UseRegister = 1,
			// Allows the user to process a payment for the transaction.
			ProcessPayment = 2,
			// Allows the user to apply a coupon to the sale.
			ApplyCoupon = 4,
			// Allows the user to override predefined prices with an entered value.
			PriceOverride = 8,
			// Allows the user to change the stored amount of an item.
			ChangeAStockQuantity = 16,
			// Allows the addition or removal of store products.
			AddRemoveAProduct = 32,
			// Allows the user to change the price/brand/etc. of a product.
			ChangeProductDetails = 64,
			// Allows the user to change the value of transactions that have happened in the past.
			ModifyTransaction = 128,
			// Allows the user to change other users' permissions.
			ChangePermissions = 256
		}

		public static void AddPermission(PointOfSales.Users.Employee User, PointOfSales.Users.Employee EmployeeToModify, PointOfSalesPermissions PermissionToAdd)
		{
			if ((User.EmployeePermissions & (int)PointOfSalesPermissions.ChangePermissions) >= 1)
				throw new InvalidOperationException("User " + User.name + " Does not have permissions for this operation.");

			EmployeeToModify.EmployeePermissions = EmployeeToModify.EmployeePermissions | (int)PermissionToAdd;
		}

		public static void RemovePermissions(PointOfSales.Users.Employee User, PointOfSales.Users.Employee EmployeeToModify, PointOfSalesPermissions PermissionToRemove)
		{
			if ((User.EmployeePermissions & (int)PointOfSalesPermissions.ChangePermissions) >= 1)
				throw new InvalidOperationException("User " + User.name + " Does not have permissions for this operation.");

			EmployeeToModify.EmployeePermissions = EmployeeToModify.EmployeePermissions | ~(int)PermissionToRemove;
		}
    }
}
