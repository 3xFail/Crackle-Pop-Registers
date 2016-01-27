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
		private TextBlock itemPrice { get; set; }

		private double rawItemPrice = 0;
	}
}
