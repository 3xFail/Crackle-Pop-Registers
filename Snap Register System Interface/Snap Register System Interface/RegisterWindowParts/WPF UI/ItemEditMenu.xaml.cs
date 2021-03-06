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
using PointOfSales.Permissions;
using PointOfSales.Users;

namespace SnapRegisters
{
	/// <summary>
	/// Interaction logic for ItemEditMenu.xaml
	/// </summary>
	public partial class ItemEditMenu : UserControl
	{
		public delegate void CloseEditMenu();

		public ItemEditMenu(ItemDisplayBox itemToModify, Transaction transaction, CloseEditMenu closeFunction, Employee currentUser)
		{
			InitializeComponent();

			m_itemBox = itemToModify;
			m_transaction = transaction;
			m_closeFunction = closeFunction;
            m_currentUser = currentUser;
			ItemNameBox.Text = m_itemBox.SourceItem.ItemName.ToString();

			RemoveItem = false;
			PriceOverride = false;
		}

		private ItemDisplayBox m_itemBox;
		private Transaction m_transaction;
		private CloseEditMenu m_closeFunction;
        private Employee m_currentUser;
		public bool RemoveItem { get; set; }
		public bool PriceOverride { get; set; }
		private void RemoveItemButtonClicked(object sender, RoutedEventArgs e)
		{
            // check permission
            if (!Permissions.CheckPermissions(m_currentUser, Permissions.CanVoidItem))
                throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.CanVoidItem));
            RemoveItem = true;
			m_transaction.RemoveItem(m_itemBox.SourceItem);
			m_closeFunction();
		}

		private void ChangePriceButtonClicked(object sender, RoutedEventArgs e)
		{
			PriceOverride = true;
			m_closeFunction();
		}
	}
}
