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

namespace SnapRegisters.RegisterWindowParts.WPF_UI
{
	/// <summary>
	/// Interaction logic for ItemDisplayBox.xaml
	/// </summary>
	public partial class ItemDisplayBox : UserControl
	{
		// Construct with an item.
		public ItemDisplayBox(Item sourceItem)
		{
			InitializeComponent();

			m_sourceItem = sourceItem;

			NameField.Text = m_sourceItem.ItemName;
			rawItemPrice = m_sourceItem.Price;
			AmountField.Text = rawItemPrice.ToString( "C" );

			PreviewMouseLeftButtonDown += DisplayItemClickedEvent;
		}

		private void DisplayItemClickedEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			EditItemMenu editMenu = new EditItemMenu(m_sourceItem);
			editMenu.Show();
		}

		private double rawItemPrice = 0;
		private Item m_sourceItem;
	}
}
