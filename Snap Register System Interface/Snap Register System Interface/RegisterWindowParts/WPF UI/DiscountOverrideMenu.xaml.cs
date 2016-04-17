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
	/// Interaction logic for DiscountOverrideMenu.xaml
	/// </summary>
	public partial class DiscountOverrideMenu : UserControl
	{
		public delegate void ClosePriceOverrideMenu();
		public DiscountOverrideMenu(DiscountDisplayBox discountToModify, Transaction transaction, ClosePriceOverrideMenu closeFunction)
		{
			InitializeComponent();

			m_discountBox = discountToModify;
			m_transaction = transaction;
			m_closeFunction = closeFunction;

			OriginalAmountField.Text = "Original Amount: " + m_discountBox.Discount;
		}

		private void ChangeDiscountButtonClick(object sender, RoutedEventArgs e)
		{
			m_transaction.OverrideDiscount(m_discountBox.PossessingItem, m_discountBox.SourceDiscount, NewAmountField.Number);
			m_closeFunction();
		}

		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			m_closeFunction();
		}

		private DiscountDisplayBox m_discountBox;
		private Transaction m_transaction;
		private ClosePriceOverrideMenu m_closeFunction;
	}
}
