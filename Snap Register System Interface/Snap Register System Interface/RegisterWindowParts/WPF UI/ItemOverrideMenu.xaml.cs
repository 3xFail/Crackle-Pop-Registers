﻿using System;
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
	/// Interaction logic for PriceOverrideMenu.xaml
	/// </summary>
	public partial class ItemOverrideMenu : UserControl
	{
		public delegate void ClosePriceOverrideMenu();
		public ItemOverrideMenu(ItemDisplayBox itemToModify, Transaction transaction, ClosePriceOverrideMenu closeFunction)
		{
			InitializeComponent();

			m_itemBox = itemToModify;
			m_transaction = transaction;
			m_closeFunction = closeFunction;

			OriginalPriceField.Text = "Original Price: " + m_itemBox.SourceItem.OriginalPrice.ToString( "C" );
		}

		private void ChangePriceButtonClicked(object sender, RoutedEventArgs e)
		{
			ChangedPrice = new ManagerOverrideDiscount(m_itemBox.SourceItem.OriginalPrice - NewPriceField.Number);
			m_transaction.AddCustomCoupon(m_itemBox.SourceItem, NewPriceField.Number);
			Confirmed = true;
			m_closeFunction();
		}

		private ItemDisplayBox m_itemBox;
		private Transaction m_transaction;
		private ClosePriceOverrideMenu m_closeFunction;
		public bool Confirmed { get; set; }
		public IDiscount ChangedPrice { get; set; }
		private void CancelButtonClick(object sender, RoutedEventArgs e)
		{
			m_closeFunction();
		}
	}
}
