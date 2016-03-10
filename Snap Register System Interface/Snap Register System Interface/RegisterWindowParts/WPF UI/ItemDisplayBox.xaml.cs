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
		// Construct with an item.
		public ItemDisplayBox(Item sourceItem)
		{
			InitializeComponent();

			m_sourceItem = sourceItem;

			NameField.Text = sourceItem.ItemName.Substring(0, Math.Min(sourceItem.ItemName.Length, 40));

			AmountField.Text = m_sourceItem.OriginalPrice.ToString( "C" );

			PreviewMouseLeftButtonDown += DisplayItemClickedEvent;
		}

		private void DisplayItemClickedEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			EditItemMenu editMenu = new EditItemMenu(m_sourceItem);
			editMenu.Show();
		}

		private Item m_sourceItem;
	}
}
