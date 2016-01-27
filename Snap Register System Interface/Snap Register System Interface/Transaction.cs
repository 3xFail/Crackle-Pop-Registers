using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device;

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public class Transaction
	//		SUMMARY: 
	//			This class contains all information about the current sale. A new class should be created for each
	//			Transaction at a register. This class allows the managing of a sale from scanning to payment.
	//		MEMBERS:
	//			private List<Item> m_Items
	//				A list of all items in the transaction.
	//			private Employee m_Employee
	//				The currently logged in employee.
	//		FUNCTIONS:
	//			public Transaction(Employee employee, ref DockPanel itemOutput, ref DockPanel discountOutput)
	//				Constructor for a new transaction opened by the specified employee. itemOutput is a DockPanel
	//				passed by reference where items should be displayed to while discountOutput is the same thing
	//				except for discounts instead.
	//			public void AddItem(int itemID)
	//				Adds an item to the current transaction. Displays this item to the outputs.
	//			public void RemoveItem(int itemID)
	//				Removes the item with the ID matching "itemID" from the transaction. removes it from the
	//				output.
	//			public void OverrideCost(int itemID, double newPrice, string reason = "No description")
	//				Overrides the cost of the item specified with the new price specified with "newPrice".
	//				"reason" is the reason the employee chose to override the price.
	//			public void ApplyCoupon(int couponID)
	//				Applies a coupon to the sale.
	//			public void Checkout()
	//				Finishes the transaction and begins processing payment.
	//			public List<Item> GetItems()
	//				Returns a copy of all the items in this sale. The list of items cannot be changed without
	//				proper permissions but can be read with this.
	//			private Item ConstructItem(int itemID)
	//				Contacts the database and constructs an item from the given item ID.
	//		PERMISSIONS:
	//			AddItem						- UseRegister
	//			RemoveItem					- UseRegister
	//			OverrideCost				- PriceOverride, PriceOverrideNoReason
	//			ApplyCoupon					- ApplyCoupon
	//			Checkout					- ProcessPayment
	//			GetItems					- None
	//*************************************************************************************************************
	public class Transaction
	{
		// TODO: Make it so that multiple of the same item can be added without breaking functions.
		public Transaction(Employee employee)
		{
			if (employee == null)
				throw new InvalidOperationException("Invalid Employee Credentials.");
			if (!Permissions.CheckPermissions(employee, Permissions.PointOfSalesPermissions.UseRegister))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			m_Employee = employee;
			m_Items = new List<Item>();
		}

		public void AddItem(int itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.UseRegister))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before adding it to the list.
			try
			{
				// Construct a new item from the given itemID and add it to the list.
				Item newItem = new Item(ConstructItem(itemID));
				m_Items.Add(newItem);

				// TODO: Make this box's height equal to the combined discount's height.
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void RemoveItem(int itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.UseRegister))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before removing it from the list.
			try
			{
				m_Items.RemoveAll(x => x.id == itemID);
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void OverrideCost(int itemID, double newPrice, string reason = "No description")
		{
			// Find the item to change the price of in the list assign changedItem these values.
			Item changedItem = new Item(m_Items.Find(x => x.id == itemID));

			if (changedItem == null)
				throw new InvalidOperationException("Item specified is not in sale.");

			if (reason == "No description")
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.PriceOverrideNoReason))
					throw new InvalidOperationException("A reason must be specified for this action.");
				else
					changedItem.itemDisplayBox.SetItemPrice(newPrice);
			}
			else
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.PriceOverride))
					throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");
				else
					changedItem.itemDisplayBox.SetItemPrice(newPrice);
			}

			// Reassign the item's price.
			m_Items[m_Items.FindIndex(x => x.id == itemID)].itemDisplayBox.SetItemPrice(changedItem.itemDisplayBox.GetItemPriceAsDouble());
		}
		public void ApplyCoupon(int couponID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.ApplyCoupon))
				throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");

			// TODO: Connect to database and attempt to apply coupon.

			// Connect to the database and check if the couponID is correct. If not throw an exception.

			// For each item in m_Items, look at the database and see if the coupon should be applied to it. If so, add
			// a new discount to m_Items[that item].discounts.
		}
		public void Checkout()
		{
			// TODO: Insert credit card magic here.
		}

		public List<Item> GetItems()
		{
			return m_Items;
		}

		private Item ConstructItem(int itemID)
		{
			// TODO: Connect to database and construct item from given item ID.
			Item newItem = new Item();

			return newItem;
		}

		private List<Item> m_Items = null;
		private Employee m_Employee = null;
	}
}
