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

namespace SnapRegisters
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
	public partial class RegisterMainWindow : Window
	{
		// temp constructor for testing.
		public RegisterMainWindow()
		{
			Employee currentEmployee = new Employee(1, "Joe", null, "5", new DateTime(1,2,3), 256);

			InitializeComponent();
			m_employee = currentEmployee;
			m_transaction = new Transaction(m_employee, ref ItemsList, ref CouponList);
		}

		public RegisterMainWindow(Employee currentEmployee)
		{
			InitializeComponent();

			m_employee = currentEmployee;
			m_transaction = new Transaction(m_employee, ref ItemsList, ref CouponList);
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
		public Transaction(Employee employee, ref DockPanel itemOutput, ref DockPanel discountOutput)
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
				itemOutput.Children.Add(newItem.itemDisplayBox.displayItem);

				foreach(DiscountDisplayBox discount in newItem.discounts)
					discountOutput.Children.Add(discount.displayItem);


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
				
				foreach (UIElement item in itemOutput.Children)
				{
					itemOutput.Children.Remove(item);
				}

				foreach (UIElement item in discountOutput.Children)
				{
					discountOutput.Children.Remove(item);
				}

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
			return new Item();
		}

		private List<Item> m_Items = null;
		private Employee m_Employee = null;
		private DockPanel itemOutput = null;
		private DockPanel discountOutput = null;
	}

	//*************************************************************************************************************
	// public class Item
	//		SUMMARY: 
	//			This class represents an item in the sale. It contains an ItemDisplayBox and a list of
	//			DiscountDisplayBoxs that apply to the item.
	//		MEMBERS:
	//			public int id;
	//				The id of this item.
	//			public ItemDisplayBox
	//				This box is responsible for displaying the name and the price of the item. Contains a grid
	//				with the name of the item left aligned and the price of the item right aligned. Can be
	//				manipulated with the SetItemName() and SetItemPrice() functions.
	//			public List<DiscountDisplayBox> discounts
	//				This is a list of box's responsible for displaying one discount item for this item. Each box
	//				contains a grid with the name of the discount left aligned and the amount of the discount right
	//				aligned. Can be manipulated with the SetDiscountName() and SetDiscountAmount() functions.
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
			id = 0;
			itemDisplayBox = new ItemDisplayBox();
			discounts = new List<DiscountDisplayBox>();
		}
		public Item(Item source)
		{
			id = source.id;
			itemDisplayBox = new ItemDisplayBox(source.itemDisplayBox.GetItemNameAsString(), source.itemDisplayBox.GetItemPriceAsDouble());
			discounts = new List<DiscountDisplayBox>(source.discounts);
		}

		public int id;
		public ItemDisplayBox itemDisplayBox { get; set; }
		public List<DiscountDisplayBox> discounts { get; set; }
	}
	//*************************************************************************************************************
	// public class ItemDisplayBox
	//		SUMMARY:
	//			This is a displayable item showing the name of the item and the price of the item. The entire
	//			object is a Grid with a TextBlock for name and price.
	//		MEMBERS:
	//			public int id
	//				The id of the item this box is associated with.
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

		public int id = 0;
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
	//			public int id
	//				The id of the item this discount is associated with.
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

		public int id = 0;
		public Grid displayItem { get; set; }
		private TextBlock discountName { get; set; }
		private TextBlock discountAmount { get; set; }
		private double rawDiscountAmount = 0;


	}
}

