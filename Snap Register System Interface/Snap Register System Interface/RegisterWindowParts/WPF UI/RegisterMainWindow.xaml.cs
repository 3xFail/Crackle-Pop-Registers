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
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device;
using CSharpClient;
using SnapRegisters.RegisterWindowParts.WPF_UI;
using System.Windows.Threading;
using Snap_Register_System_Interface.RegisterWindowParts.Business_Objects;
using Snap_Register_System_Interface.RegisterWindowParts.WPF_UI;

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public partial class MainWindow : Window
	//		SUMMARY:
	//			This is the main class that drives the main window of the user interface. It contains functions
	//			used to move the sale forward and display the information to the screen.
	//		MEMBERS:
	//			private connection_session m_connection
	//				The connection to the server. Used to update and retrieve items from the database.
	//			private Transaction m_transaction
	//				Contains the data of the sale as well as manipulation of that data.
	//			private Employee m_employee
	//				The employee who is currently using the register. This data is pulled from the login page.
	//			private double m_costTotal
	//				The roll-up for the cost column display. This displays the total cost of items before coupons
	//				are applied.
	//			private double m_savingsTotal
	//				The roll-up for the savings column. This displays the total amount of all the combined
	//				coupons.
	//			private double totalTotal
	//				This is how much would actually be charged for the sale. this is m_costTotal - m_savingsTotal.
	//			public static KeyBoardHook
	//				Restricts the keyboard from interacting with applications other than this one.
	//		WPF NAMES:
	//			ScrollViewer ItemScroll
	//				Allows the ItemsList panel to scroll up and down if more items exist than screen space.
	//			StackPanel ItemsList
	//				A panel for displaying ItemDisplayBoxs. items added to the transaction are displayed here.
	//			TextBlock CostTotal
	//				Displays the total cost for the sale. See m_costTotal.
	//			ScrollViewer CouponScroll
	//				Allows the CouponLiest panel to scroll up and down if more items exist than screen space.
	//			StackPanel CouponList
	//				A panel for displaying CouponDisplayBoxs. Coupons added to the transaction are displayed here.
	//			TextBlock SavingsTotal
	//				Displays the total savings for the sale. See m_savingsTotal.
	//			TextBlock dateText
	//				Displays current system time and date.
	//			TextBlock LoggedInAs
	//				Displays the full name of the employee logged in.
	//			Button OptionsButton
	//				Button for displaying the options menu.
	//			TextBox UPCField
	//				Field for entering standard bar-codes. The only valid bar-codes here are items and coupons.
	//			TextBlock Total
	//				Displays the total for the sale. See m_totalTotal.
	//		FUNCTIONS:
	//			public MainWindow(Employee currentEmployee)
	//				Constructs the main sales window. An employee must be specified in order to check permissions.
	//			private void AddItemToOutputPanel(Item itemToAdd)
	//				Takes the Item itemToAdd and adds it to the ItemsList.
	//			private void AddCouponToOutputPanel(Coupon couponToAdd)
	//				Takes the Coupon couponToAdd and adds it to the CouponList.
	//			private void UpdateTotals
	//				Updates the 3 totals boxes to display the current amount.
	//		EVENTS:
	//			ShorcutKeyPressed
	//				Sender - MainWindow
	//				Event - KeyDown
	//				Action
	//					Handles various actions depending on what shortcut key is pressed. Also intercepts
	//					key functions removed such as alt+f4.
	//			WindowClicked
	//				Sender - MainWindow
	//				Event -	MouseDown
	//				Action
	//					Sets the focus to the UPCField if nothing else is specified.
	//			OptionsButton_Click
	//				Sender - OptionsButton
	//				Event - Click
	//					Opens the options menu, fired when the button is clicked.
	//		PERMISSIONS:
	//			Permissions are handled by the Transaction class.
	//*************************************************************************************************************
	public partial class RegisterMainWindow : Window
	{
		public RegisterMainWindow(Employee currentEmployee, connection_session session)
		{

			
			//Updates the clock constantly
			DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1),
														DispatcherPriority.Normal,
														delegate { this.dateText.Text = DateTime.Now.ToString("hh:mm tt"); }, 
														this.Dispatcher);


			//Delays showing the window until the clock is guaranteed to have already ticked once (ticks once per second).
			//Doesn't delay the in debug mode for quicker development
#if !DEBUG
			System.Threading.Thread.Sleep(1100);
#endif

			//Initialize window after the clock is ticked
			InitializeComponent();

			m_employee = currentEmployee;
			m_connection = session;
			m_transaction = new Transaction(m_employee, AddItemToOutputPanels, AddCouponToOutputPanels, m_connection);

			FocusManager.SetFocusedElement(this, UPCField);

			//Sets the username to the employee that logged in
			LoggedInAs.Text = currentEmployee.name;

			//Experimental - Not working yet
			//This code is supposed to lock the keyboard to this application
			//kh = new KeyboardHook(KeyboardHook.Parameters.AllowWindowsKey);
		}
		private void AddItemToOutputPanels(Item itemToAdd)
		{
			// Update the documentation once you have this finished.
			ItemDisplayBox itemDescription = new ItemDisplayBox(itemToAdd);
			itemDescription.Height = 60;
			double height_t = itemDescription.Height;
			int count = 0;

			ItemsList.Children.Add(itemDescription);

			m_costTotal += itemToAdd.Price;
			m_totalTotal += itemToAdd.Price;

			foreach(Coupon coupon in itemToAdd.Discounts)
			{
				//need a Discounts Display box
				CouponDisplayBox couponDescription = new CouponDisplayBox(coupon);
				couponDescription.Height = 60;
				count++;

				if (height_t != (count * couponDescription.Height) && couponDescription.Height != 0 )
				{
					  height_t += couponDescription.Height;
					  itemDescription.Height =  height_t;
				}

				m_savingsTotal += coupon.m_discount;
				m_totalTotal -= coupon.m_discount;

				CouponList.Children.Add(couponDescription);
			}

			ItemScroll.ScrollToBottom();
			CouponScroll.ScrollToBottom();
			UpdateTotals();
		}
		private void AddCouponToOutputPanels(Coupon couponToAdd)
		{
			// Update the documentation once you have this finished.
			CouponDisplayBox CouponDescription = new CouponDisplayBox(couponToAdd);

			//not any current xaml object todo this
			CouponDescription.Height = 60;

			//breaking because of not being able to convert to a UI Element
			//CouponList.Children.Add(CouponDescription);

			m_savingsTotal += couponToAdd.m_discount;
			m_totalTotal -= couponToAdd.m_discount;


			ItemScroll.ScrollToBottom();
			CouponScroll.ScrollToBottom();
			UpdateTotals();
		}
		private void UpdateTotals()
		{
			CostTotal.Text = m_costTotal.ToString("C");
			SavingsTotal.Text = m_savingsTotal.ToString("C");
			Total.Text = m_totalTotal.ToString("C");
		}

		private connection_session m_connection = null;
		private Transaction m_transaction = null;
		private Employee m_employee = null;
		private double m_costTotal = 0;
		private double m_savingsTotal = 0;
		private double m_totalTotal = 0;
		public static KeyboardHook kh;

		private void ShortcutKeyPressed(object sender, KeyEventArgs e)
		{
			if (UPCField.Text == string.Empty)
				return;

			if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				UPCField.Clear();


			
			if (e.Key == Key.Enter)
			{
				try
				{
					m_transaction.AddItem(UPCField.Text); //try constructing an item
					UPCField.Clear();
				}
				catch (Exception ) //if that fails
				{
					try { m_transaction.AddCoupon(UPCField.Text); } //try constructing a coupon
					catch (Exception _ex) { System.Windows.Forms.MessageBox.Show(_ex.Message); }//if both of those fail show the error message
				}
			}

			if (e.Key == Key.Escape)
				FocusManager.SetFocusedElement(this, UPCField);
		}
		private void WindowClicked(object sender, MouseButtonEventArgs e)
		{
			FocusManager.SetFocusedElement(this, UPCField);
		}

		private void OptionsButton_Click(object sender, RoutedEventArgs e)
		{
			SnapRegisters.LoginMainWindow loginWindow = new SnapRegisters.LoginMainWindow();
			loginWindow.Show();
			this.Close();
		}

	}
}

