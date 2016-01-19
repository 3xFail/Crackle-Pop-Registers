using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device;

namespace Snap_Register_System_Interface
{
	//*************************************************************************************************************
	// public partial class MainWindow : Window
	//		SUMMARY:
	//			This is the main class that drives the main window of the user interface. It contains functions
	//			used to move the sale forward and display the information to the screen.
	//		MEMBERS:
	//			private Transaction m_transaction
	//				Contains the data of the sale as well as manipulation of that data.
	//			private Employee m_employee
	//				The employee who is currently using the register. This data is pulled from the login page.
	//		FUNCTIONS:
	//			public MainWindow(Employee currentEmployee)
	//				Constructs the main sales window. An employee must be specified in order to check permissions.
	//			private UpdateItemDisplay
	//		PERMISSIONS:
	//			Permissions are handled by the Transaction class.
	//*************************************************************************************************************
	public partial class MainWindow : Window
	{
		// temp constructor for testing.
		public MainWindow()
		{
			Employee currentEmployee = new Employee(1, "Joe", null, "5", new DateTime(1,2,3));

			InitializeComponent();
			m_employee = currentEmployee;
			m_transaction = new Transaction(m_employee);
		}

		public MainWindow(Employee currentEmployee)
		{
			InitializeComponent();

			m_employee = currentEmployee;
			m_transaction = new Transaction(m_employee);
		}


		private Transaction m_transaction = null;
		private Employee m_employee = null;
	}


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
	//			public Transaction(Employee employee)
	//				Constructor for a new transaction opened by the specified employee.
	//			public void AddItem(int itemID)
	//				Adds an item to the current transaction.
	//			public void RemoveItem(int itemID)
	//				Removes the item with the ID matching "itemID" from the transaction.
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
	//			GetItems					- UseRegister
	//*************************************************************************************************************
	public class Transaction
	{
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
				m_Items.Add(new Item(ConstructItem(itemID)));

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

			// Checks to make sure the item was valid before adding it to the list.
			try
			{
				// Construct a new item from the given itemID and add it to the list.
				m_Items.Remove(new Item(ConstructItem(itemID)));

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

			if(reason == "No description")
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.PriceOverrideNoReason))
					throw new InvalidOperationException("A reason must be specified for this action.");
				else
					changedItem.price = newPrice;
			}
			else
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.PriceOverride))
					throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");
				else
					changedItem.price = newPrice;
			}

			// Reassign the item's price.
			m_Items[m_Items.FindIndex(x => x.id == itemID)].price = changedItem.price;
		}
		public void ApplyCoupon(int couponID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.PointOfSalesPermissions.ApplyCoupon))
				throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");

			// Connect to the database and check if the couponID is correct. If not throw an exception.

			// For each item in m_Items, look at the database and see if the coupon should be applied to it. If so, add
			// a new discount to m_Items[that item].discounts.
		}
		public void Checkout()
		{
			// Some sort of credit card magic goes here.
		}

		public List<Item> GetItems()
		{
			return m_Items;
		}

		private Item ConstructItem(int itemID)
		{
			// Connect to database and create a new item from information in the database.
			return new Item();
		}

		private List<Item> m_Items;
		private Employee m_Employee;
	}

	//*************************************************************************************************************
	// public class Item
	//		SUMMARY: 
	//			This class represents an item in the sale. It contains 2 TextBlocks and data about the item for
	//			sale. The first TextBlock displays the name and cost of the product while the second displays all
	//			the discounts that item has.
	//		MEMBERS:
	//			public DisplayBox itemDetails
	//				A DisplayBox displaying the item's name and the item's price. The item's name is left aligned
	//				and the item's price is right aligned. This box will always be the height of the itemDiscounts
	//				text block. If it is short, blank lines will be added to ensure it is the same height.
	//			public Grid itemDiscounts
	//				A TextBlock displaying each discount applied to the item. The discount's name is left aligned
	//				while the discount's amount is right aligned. Each discount gets its own line.
	//			public int idD
	//				The ID of the item. Updating this updates the display boxes.
	//			public string itemName
	//				The name of the item. Updating this updates the display boxes.
	//			public double price
	//				The price of the item. Updating this updates the display boxes.
	//			public List<string> discountName
	//				A list of the names of all the discounts this item has. Updating this updates the display boxes.
	//			public List<double> discountAmounts
	//				A list of all discount quantities this item has. Updating this updates the display boxes.
	//		FUNCTIONS:
	//			public Item()
	//				Basic constructor.
	//			public Item(Item source)
	//				Copy Constructor.
	//			private void UpdateDisplayBoxes
	//				Reassigns text to the display box with the current info in the item.
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class Item
	{
		public Item()
		{
			discountNames = new List<string>();
			discountAmounts = new List<double>();

			UpdateDisplayBoxes();
		}
		public Item(Item source)
		{
			id = source.id;
			itemName = source.itemName;
			price = source.price;
			discountNames = new List<string>(source.discountNames);
			discountAmounts = new List<double>(source.discountAmounts);

			UpdateDisplayBoxes();
		}

		private void UpdateDisplayBoxes()
		{
			itemDetails.Text
		}


		public TextBlock itemDetails { get; set; }
		public TextBlock itemDiscounts { get; set; }
		public int id { get; set; }
		public string itemName { get; set; }
		public double price { get; set; }
		public List<string> discountNames { get; set; }
		public List<double> discountAmounts { get; set; }
	}


}
