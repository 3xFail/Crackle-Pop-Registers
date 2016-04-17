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
	/// Interaction logic for DiscountEditMenu.xaml
	/// </summary>
	public partial class DiscountEditMenu : UserControl
	{
		public delegate void CloseEditMenu();

		public DiscountEditMenu(DiscountDisplayBox discountToModify, Transaction transaction, CloseEditMenu closefunction)
		{
			InitializeComponent();

			m_discountBox = discountToModify;
			m_transaction = transaction;
			m_closeFunction = closefunction;

			RemoveDiscount = false;
			AmountOverride = false;
		}

		private DiscountDisplayBox m_discountBox;
		private Transaction m_transaction;
		private CloseEditMenu m_closeFunction;
		public bool RemoveDiscount { get; set; }
		public bool AmountOverride { get; set; }
		private void RemoveDiscountButtonClicked(object sender, RoutedEventArgs e)
		{
			RemoveDiscount = true;
			m_transaction.RemoveDiscount(m_discountBox.PossessingItem, m_discountBox.SourceDiscount);
			m_closeFunction();
		}

		private void ChangeDiscountButtonClicked(object sender, RoutedEventArgs e)
		{
			AmountOverride = true;
			m_closeFunction();
		}
	}
}
