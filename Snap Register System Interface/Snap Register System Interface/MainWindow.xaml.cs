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

namespace Snap_Register_System_Interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainMenuButtonClick(object sender, RoutedEventArgs e)
		{
			if (m_mainMenu == null)
			{
				m_mainMenu = new MainMenu();
				m_mainMenu.Show();
			}
			else
			{
				m_mainMenu.Hide();
				m_mainMenu.Close();
				m_mainMenu = null;
			}
		}

		private Snap_Register_System_Interface.MainMenu m_mainMenu = null;
	}


	//*************************************************************************************************************
	// public class Transaction
	//		SUMMARY: 
	//			This class contains all information about the current sale. A new class should be created for each
	//			Transaction at a register. This class allows the managing of a sale from scanning to payment.
	//		MEMBERS:
	//			private List<Item> m_Items
	//				A list of all items in the transaction.'
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
	//			public void ApplyCoupon(int CouponID)
	//				Applies a coupon to the sale.
	//			public void Checkout()
	//				Finishes the transaction and begins processing payment.
	//			private Item ConstructItem(int itemID)
	//				Contacts the database and constructs an item from the given item ID.
	//		PERMISSIONS:
	//			AddItem						- UseRegister
	//			RemoveItem					- UseRegister
	//			OverrideCost				- PriceOverride, PriceOverrideNoReason
	//			ApplyCoupon					- ApplyCoupon
	//			Checkout					- ProcessPayment
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



		private Item ConstructItem(int itemID)
		{
			// REPLACE ME:
			// Connect to database and create a new item from information in the database.
			return new Item();
		}

		private List<Item> m_Items;
		private Employee m_Employee;
	}

	//*************************************************************************************************************
	// public class Item
	//		SUMMARY: 
	//			This class stores information about an item used in a sale. This information contains an ID for
	//			the item, a price, a list of discounts, etc.
	//		MEMBERS:
	//			public int idD
	//				The ID of the item.
	//			public string itemName
	//				The name of the item.
	//			public double price
	//				The price of the item.
	//			public List<double> discounts
	//				A list of all discounts this item has.
	//		FUNCTIONS:
	//			public Item()
	//				Basic constructor.
	//			public Item(Item source)
	//				Copy Constructor.
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class Item
	{
		public Item()
		{
			discounts = new List<double>();
		}
		public Item(Item source)
		{
			id = source.id;
			itemName = source.itemName;
			price = price;
			discounts = new List<double>(source.discounts);
		}
		public int id { get; set; }
		public string itemName { get; set; }
		public double price { get; set; }
		public List<double> discounts { get; set; }
	}
}
