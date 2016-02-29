using Snap_Register_System_Interface.RegisterWindowParts.Business_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SnapRegisters
{
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
