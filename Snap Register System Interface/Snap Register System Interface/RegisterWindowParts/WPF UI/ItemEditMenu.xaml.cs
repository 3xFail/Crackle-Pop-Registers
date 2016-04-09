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
	/// <summary>
	/// Interaction logic for ItemEditMenu.xaml
	/// </summary>
	public partial class ItemEditMenu : UserControl
	{
		public ItemEditMenu(Item itemToModify)
		{
			InitializeComponent();

			m_item = itemToModify;

			ItemNameBox.Text = m_item.ItemName.ToString();
		}

		private Item m_item;
	}
}
