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

namespace Snap_Register_System_Interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainMenuButtonClick(object sender, RoutedEventArgs e)
		{
			if (m_mainMenu == null)
			{
				m_mainMenu = new MainMenu();
				m_mainMenu.Show();
			}
			else
			{
				m_mainMenu.Hide();
				m_mainMenu.Close();
				m_mainMenu = null;
			}
		}

		private Snap_Register_System_Interface.MainMenu m_mainMenu = null;
	}


	//*************************************************************************************************************
	// public class Transaction
	//		SUMMARY: 
	//			This class contains all information about the current sale. A new class should be created for each
	//			Transaction at a register. This class allows the managing of a sale from scanning to payment.
	//		MEMBERS:
	//			private List<items> m_Items
	//				A list of all items in the transaction.
	//		FUNCTIONS:
	//			public void RemoveItem(int itemID)
	//				Removes the item with the ID matching "itemID" from the transaction.
	//			public void OverRideCost(int itemID, double newPrice, string reason = "No description")
	//				Overrides the cost of the item specified with the new price specified with "newPrice".
	//				"reason" is the reason the employee chose to override the price.
	//			public void ApplyCoupon(int CouponID)
	//		PERMISSIONS:
	//			RemoveItem - 
	//			
	// 
	//*************************************************************************************************************
	public class Transaction
	{

	}
}
