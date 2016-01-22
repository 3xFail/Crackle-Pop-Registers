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
	//			This class represents an item in the sale. It contains an ItemDisplayBox and a list of
	//			DiscountDisplayBoxs that apply to the item.
	//		MEMBERS:
	//			
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
			discounts = new List<DiscountDisplayBox>();
		}
		public Item(Item source)
		{
			id = source.id;
			itemName = source.itemName;
			price = source.price;
			discounts = new List<DiscountDisplayBox>(source.discounts);
		}

		public ItemDisplayBox itemDisplayBox { get; set; }
		public List<DiscountDisplayBox> discounts { get; set; }
	}
	//*************************************************************************************************************
	// public class ItemDisplayBox
	//		SUMMARY:
	//			This is a displayable item showing the name of the item and the price of the item. The entire
	//			object is a Grid with a TextBlock for name and price.
	//		MEMBERS:
	//			public Grid displayItem
	//				This is the object to display. Contains the name of the item and its price.
	//			private TextBlock itemName
	//				The TextBlock that will display the name of the item. This will be left, top aligned to the
	//				Grid.
	//			private TextBlock itemPrice
	//				The TextBlock that will display the price of the item. This will be right, top aligned to the
	//				Grid.
	//			private double rawItemPrice
	//				The price of the item stored as a double.
	//		FUNCTIONS:
	//			public ItemDisplayBox()
	//				Basic constructor. Creates a blank item.
	//			public ItemDisplayBox(string name, double price)
	//				Overloaded constructor. Takes a name and a price and creates an item from those parameters.
	//			public string GetItemNameAsString()
	//				Returns the name of the item in itemName as a string.
	//			public SetItemName(string name)
	//				Sets the name of this item to the name given.
	//			public string GetItemPriceAsString()
	//				Returns the price of the item as a string.
	//			public double GetItemPriceAsDouble()
	//				Returns the price of the item as a double.
	//			public void SetItemPrice(double price)
	//				Sets the price of the item to the price specified.
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class ItemDisplayBox
	{
		public ItemDisplayBox()
		{
			itemName = new TextBlock();
			itemPrice = new TextBlock();
			displayItem = new Grid();

			itemName.TextAlignment = TextAlignment.Left;
			itemName.HorizontalAlignment = HorizontalAlignment.Left;
			itemPrice.TextAlignment = TextAlignment.Right;
			itemPrice.HorizontalAlignment = HorizontalAlignment.Right;

			displayItem.Children.Add(itemName);
			displayItem.Children.Add(itemPrice);
		}

		public ItemDisplayBox(string name, double price)
		{
			itemName = new TextBlock();
			itemPrice = new TextBlock();
			displayItem = new Grid();

			itemName.TextAlignment = TextAlignment.Left;
			itemName.HorizontalAlignment = HorizontalAlignment.Left;
			itemPrice.TextAlignment = TextAlignment.Right;
			itemPrice.HorizontalAlignment = HorizontalAlignment.Right;

			displayItem.Children.Add(itemName);
			displayItem.Children.Add(itemPrice);

			itemName.Text = name;

			rawItemPrice = price;
			itemPrice.Text = price.ToString();
		}

		public string GetItemNameAsString()
		{
			return itemName.Text;
		}
		public void SetItemName(string name)
		{
			itemName.Text = name;
		}
		public string GetItemPriceAsString()
		{
			return itemPrice.Text;
		}
		public double GetItemPriceAsDouble()
		{
			return rawItemPrice;
		}
		public void SetItemPrice(double price)
		{
			rawItemPrice = price;
			itemPrice.Text = price.ToString();
		}

		public Grid displayItem { get; set; }
		private TextBlock itemName { get; set; }
		private TextBlock itemPrice {get; set; }

		private double rawItemPrice = 0;
	}

	//*************************************************************************************************************
	// public class DiscountDisplayBox
	//		SUMMARY:
	//			This is a a displayable box that contains a single discount for a single item. The entire object
	//			is a grid that contains a TextBlock for the name of the discount and a TextBlock that contains the
	//			amount.
	//		MEMBERS:
	//			public Grid displayDiscount
	//				This is the object to display. Contains the name of the discount and the amount.
	//			private TextBlock discountName
	//				The TextBlock displaying the name of the discount. This will be left, top aligned to the Grid.
	//			private TextBlock discountAmount
	//				The TextBlock displaying the discount amount. This will be right, top aligned to the Grid.
	//			private double rawDiscountAmount
	//				The discount stored as a double.
	//		FUNCTIONS:
	//			public DiscountDisplayBox()
	//				Basic constructor. Creates a blank discount.
	//			public DiscountDisplayBox(string name, double amount)
	//				Overloaded constructor. Takes the name of the discount and the amount and constructs a 
	//				discount item from that.
	//			public string GetDiscountNameAsString()
	//				Returns the name of the item in itemName as a string.
	//			public void SetDiscountName(string name)
	//				Sets the discount name to the name specified.
	//			public string GetDiscountAmountAsString()
	//				Returns the discount amount as a string.
	//			public double GetDiscountAmountAsDouble()
	//				Returns the discount amount as a double.
	//			public void SetDiscountAmount(double amount)
	//				Sets the discount amount to the amount specified.
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class DiscountDisplayBox
	{
		public DiscountDisplayBox()
		{
			discountName = new TextBlock();
			discountAmount = new TextBlock();
			displayItem = new Grid();

			discountName.TextAlignment = TextAlignment.Left;
			discountName.HorizontalAlignment = HorizontalAlignment.Left;
			discountAmount.TextAlignment = TextAlignment.Right;
			discountAmount.HorizontalAlignment = HorizontalAlignment.Right;

			displayItem.Children.Add(discountName);
			displayItem.Children.Add(discountAmount);
		}
		public DiscountDisplayBox(string name, double amount)
		{
			discountName = new TextBlock();
			discountAmount = new TextBlock();
			displayItem = new Grid();

			discountName.TextAlignment = TextAlignment.Left;
			discountName.HorizontalAlignment = HorizontalAlignment.Left;
			discountAmount.TextAlignment = TextAlignment.Right;
			discountAmount.HorizontalAlignment = HorizontalAlignment.Right;

			displayItem.Children.Add(discountName);
			displayItem.Children.Add(discountAmount);

			discountName.Text = name;
			discountAmount.Text = amount.ToString();
		}

		public string GetDiscountNameAsString()
		{
			return discountName.Text;
		}

		public void SetDiscountName(string name)
		{
			discountName.Text = name;
		}

		public string GetDiscountAmountAsString()
		{
			return discountAmount.Text;
		}

		public double GetDiscountAmountAsDouble()
		{
			return rawDiscountAmount;
		}

		public void SetDiscountAmount(double amount)
		{
			rawDiscountAmount = amount;
			discountAmount.Text = amount.ToString();
		}

		public Grid displayItem { get; set; }
		private TextBlock discountName { get; set; }
		private TextBlock discountAmount { get; set; }
		private double rawDiscountAmount = 0;


	}
}

