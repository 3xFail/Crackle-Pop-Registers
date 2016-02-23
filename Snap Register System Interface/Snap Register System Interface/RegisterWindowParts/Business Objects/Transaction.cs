using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device;
using CSharpClient;
using System.Xml;

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public class Transaction
	//		SUMMARY: 
	//			This class contains all information about the current sale. A new class should be created for each
	//			Transaction at a register. This class allows the managing of a sale from scanning to payment.
	//		MEMBERS:
	//			public delegate void AddItemToOutput(ref Item);
	//				After an item is created this delegate will be called allowing the item to be output to some
	//				sort of display.
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
	//			public void OverrideCost(string itemID, double newPrice, string reason = "No description")
	//				Overrides the cost of the item specified with the new price specified with "newPrice".
	//				"reason" is the reason the employee chose to override the price.
	//			public void ApplyCoupon(int couponID)
	//				Applies a coupon to the sale.
	//			public void Checkout()
	//				Finishes the transaction and begins processing payment.
	//			public List<Item> GetItems()
	//				Returns a copy of all the items in this sale. The list of items cannot be changed without
	//				proper permissions but can be read with this.
	//			private Item ConstructItem(string itemID)
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

		// Delegate for output function
		public delegate void ItemOutputDelegate(Item itemToAdd);

		// TODO: Make it so that multiple of the same item can be added without breaking functions.
		public Transaction(Employee employee, ItemOutputDelegate itemToAdd, connection_session session)
		{
			if (employee == null)
				throw new InvalidOperationException("Invalid Employee Credentials.");
			if (!Permissions.CheckPermissions(employee, Permissions.SystemPermissions.UseRegister))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            m_connection = session;
			m_Employee = employee;
			m_Items = new List<Item>();
			OutputDelegate = itemToAdd;
		}

		public Item AddItem(string itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.UseRegister))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before adding it to the list.
			try
			{
				// Construct a new item from the given itemID and add it to the list.
				Item newItem = ConstructItem(itemID);

				// Fire whatever Output method has been assigned for this item.
				OutputDelegate(newItem);

				m_Items.Add(newItem);
				return newItem;

				// TODO: Make this box's height equal to the combined discount's height.
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void RemoveItem(string itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.UseRegister))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before removing it from the list.
			try
			{
				m_Items.RemoveAll(x => x.ID == itemID);
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void OverrideCost(string itemID, double newPrice, string reason = "No description")
		{
			// Find the item to change the price of in the list assign changedItem these values.
			Item changedItem = m_Items.Find(x => x.ID == itemID);

			if (changedItem == null)
				throw new InvalidOperationException("Item specified is not in sale.");

			if (reason == "No description")
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.PriceOverrideNoReason))
					throw new InvalidOperationException("A reason must be specified for this action.");
				else
					changedItem.Price = newPrice;
			}
			else
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.PriceOverride))
					throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");
				else
					changedItem.Price = newPrice;
			}
		}
		public void ApplyCoupon(int couponID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.ApplyCoupon))
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

		private Item ConstructItem(string itemID)
		{
			// TODO: Connect to database and construct item from given item ID.
			

            m_connection.write("EXEC [dbo].[GetItem] \"" + itemID + "\"");

            XmlNode it = m_connection.Response[0];

            float price = float.Parse(it.Attributes["Price"].Value);
            string name = it.Attributes["Name"].Value;
            bool active = bool.Parse(it.Attributes["Active_Use"].Value);
            int product_id = int.Parse(it.Attributes["ProductID"].Value);


            // if the active use is false a error needs to be thrown
            Item newItem = new Item(name, price, itemID);
            

            return newItem;
		}

		public ItemOutputDelegate OutputDelegate { get; set; }
		private List<Item> m_Items = null;
		private Employee m_Employee = null;
        private connection_session m_connection = null;
	}
}
