using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public partial class ItemDisplayBox : UserControl
	//		SUMMARY:
	//			A GUI object that displays basic information about an item.
	//		MEMBERS:
	//			private Item m_sourceItem
	//				The item this GUI is displaying.
	//		WPF NAMES:
	//			TextBlock NameField
	//				The name of the item.
	//			TextBlock AmountField
	//				The price of the Item.
	//		FUNCTIONS:
	//			public ItemDisplayBox(Item sourceItem)
	//				Creates a new GUI element from the given item.
	//		EVENTS:
	//			DisplayItemClickedEvent
	//			Sender - this
	//			Event - Click
	//			Action 
	//				Open a menu allowing the edit of this object.
	//*************************************************************************************************************
	public partial class ItemDisplayBox : UserControl
	{
		public delegate void RemoveItemFromDisplay();
		public delegate void UpdateItemDisplay();
		// Construct with an item.
		public ItemDisplayBox(Item itemInTransaction, Transaction transaction, RemoveItemFromDisplay removeFunction, UpdateItemDisplay updateFunction)
		{
			InitializeComponent();

			SourceItem = itemInTransaction;
			m_transaction = transaction;
			m_removeFunction = removeFunction;
			m_updateFunction = updateFunction;
			NameField.Text = itemInTransaction.ItemName.Substring(0, Math.Min(itemInTransaction.ItemName.Length, 40));

			AmountField.Text = SourceItem.OriginalPrice.ToString( "C" );

			PreviewMouseLeftButtonDown += DisplayItemClickedEvent;
		}

		private void DisplayItemClickedEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			EditMenu = new Popup();
			EditMenu.Child = new ItemEditMenu(this, m_transaction, CloseEditMenu);
			EditMenu.IsOpen = true;
		}

		private void CloseEditMenu()
		{
			ItemEditMenu menu = (ItemEditMenu)EditMenu.Child;

            EditMenu.IsOpen = false;

			if (menu.RemoveItem)
				m_removeFunction();
			else if (menu.PriceOverride)
			{
				PriceMenu = new Popup();
				PriceMenu.Child = new ItemOverrideMenu(this, m_transaction, ClosePriceMenu);
				PriceMenu.IsOpen = true;
			}

			EditMenu = null;
		}

		private void ClosePriceMenu()
		{
			PriceMenu.IsOpen = false;
			PriceMenu = null;

			m_updateFunction();
		}

		public Item SourceItem { get; }
		public Popup EditMenu { get; set; }
		public Popup PriceMenu { get; set; }
		private Transaction m_transaction;
		private RemoveItemFromDisplay m_removeFunction;
		private UpdateItemDisplay m_updateFunction;
	}
}
