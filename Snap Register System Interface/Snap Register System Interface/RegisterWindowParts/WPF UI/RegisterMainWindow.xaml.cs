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
using System.Windows.Threading;
using System.Text.RegularExpressions;
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

        private ConnectionSession m_connection = null;
        private Transaction m_transaction = null;
        public Employee m_employee { get; private set; } = null;
        public DateTime m_start { get; private set; } = DateTime.Now;
        public int m_itemssold { get; set; } = 0;
        public decimal m_totalsales { get; set; } = 0M;
        private List<ItemAndDiscountOutputObject> m_listOfOutputObjects;
        public decimal m_costTotal { get; private set; } = 0;
        private decimal m_savingsTotal = 0;
        private decimal m_totalTotal = 0;
        public static KeyboardHook kh;


        //private PaymentWindow m_pay_window;

        public RegisterMainWindow(Employee currentEmployee, ConnectionSession session)
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

            //m_pay_window = new PaymentWindow();
            //m_pay_window.Hide();

            m_employee = currentEmployee;
            m_connection = session;
            m_transaction = new Transaction(m_employee, AddItemToOutputPanels, ShowApplicationOfCouponToSale, m_connection);
            m_listOfOutputObjects = new List<ItemAndDiscountOutputObject>();

            FocusManager.SetFocusedElement(this, UPCField);

            //Sets the username to the employee that logged in
            LoggedInAs.Text = currentEmployee.name;

            //Experimental - Not working yet
            //This code is supposed to lock the keyboard to this application
            //kh = new KeyboardHook(KeyboardHook.Parameters.AllowWindowsKey);
        }
        private void AddItemToOutputPanels(Item item)
        {
            // Update the documentation once you have this finished.
            ItemAndDiscountOutputObject itemOutput = new ItemAndDiscountOutputObject(item, m_transaction, 60, ItemsList, CouponList, UpdateTotals);
            m_listOfOutputObjects.Add(itemOutput);

            ItemScroll.ScrollToBottom();
            CouponScroll.ScrollToBottom();
            UpdateTotals();
        }
        private void ShowApplicationOfCouponToSale(Coupon couponToAdd)
        {

            foreach (ItemAndDiscountOutputObject output in m_listOfOutputObjects)
                output.AddDiscount(couponToAdd);

            ItemScroll.ScrollToBottom();
            CouponScroll.ScrollToBottom();
            UpdateTotals();
        }
        private void UpdateTotals()
        {

            m_costTotal = 0;
            m_savingsTotal = 0;
            m_totalTotal = 0;

            foreach (Item item in m_transaction.m_Items)
            {
                m_costTotal += item.OriginalPrice;
                m_savingsTotal += item.OriginalPrice - item.Price;
                m_totalTotal += item.Price;
            }

            CostTotal.Text = m_costTotal.ToString("C");
            SavingsTotal.Text = m_savingsTotal.ToString("C");
            Total.Text = m_totalTotal.ToString("C");
        }

        private void ShortcutKeyPressed(object sender, KeyEventArgs keyPressed)
        {
            // Ctrl-B: Special key combo the scanner inserts before the bar-code.
            if (keyPressed.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                UPCField.Clear();

            // F8: Opens the manager override options menu.
            if (keyPressed.Key == Key.F8)
            {
                ManagerOverrideMenu overrideMenu = new ManagerOverrideMenu(ChangeEmployeePermissions);
                overrideMenu.Show();
            }

            // Enter: Enter a bar-code from UPCField
            if (keyPressed.Key == Key.Enter)
            {
                try
                {
                    if (UPCField.Text != string.Empty)
                    {
                        m_transaction.AddItem(UPCField.Text); //try constructing an item
                        UPCField.Clear();
                    }
                }
                catch (InvalidOperationException ex )
                {
                    MessageBox.Show( ex.Message );
                }
                catch (Exception ) //if that fails
                {
                    try { m_transaction.AddCoupon(UPCField.Text); } //try constructing a coupon
                    catch (Exception _ex) { MessageBox.Show(_ex.Message); } //if both of those fail show the error message
                    UPCField.Clear();
                }
            }

            if (keyPressed.Key == Key.Escape)
                FocusManager.SetFocusedElement(this, UPCField);

            // F9: Opens the cash payment window
            if (keyPressed.Key == Key.F9)
            {
                try
                {
                    // showPayByCashWindow();
                    Main_Frame.Navigate(new PaymentMenuPage(this));

                    PaymentMenuPage.m_payment_menu_frame = Main_Frame;

                }
                catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); } //Catches exception if no items to pay for
                catch (Exception ex) { MessageBox.Show(ex.Message); } //catches any other exceptions

            }
        }

        private void ShortcutKeyPressedPayByCash(object sender, KeyEventArgs keyPressed)
        {
            // Enter: Enter the cash paid by customer
            if (keyPressed.Key == Key.Enter)
            {
                if (AmountPaidInCashBox.Text != string.Empty && decimal.Parse(AmountPaidInCashBox.Text) >= m_totalTotal )
                {
                    ChangeAmount.Text = (m_totalTotal - decimal.Parse(AmountPaidInCashBox.Text)).ToString("C");

                    cashPaymentPopup.IsOpen = false;
                    cashPaidPopup.IsOpen = true;

                    CashPaidResetRegister.Focus();

                    AmountPaidInCashBox.Clear();
                }
            }
        }



        public void ResetRegister()
        {
            m_itemssold += m_transaction.m_Items.Count;
            m_totalsales += m_totalTotal;
            m_transaction.Checkout();
            m_transaction = new Transaction(m_employee, AddItemToOutputPanels, ShowApplicationOfCouponToSale, m_connection);
            ItemsList.Children.Clear();
            CouponList.Children.Clear();
            m_listOfOutputObjects.Clear();

            UpdateTotals();
            Main_Frame.Navigate(string.Empty);
        }

        //Cash payment popup functions
        //////////////////////////////////////////////////
        //Added command binding extension barebones even though they're not being used
        //for possible future implementation
        private void PayByCash_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void PayByCash_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            cashPaymentShowPopup_Click(sender, e);
        }
        private void cashPaymentClosePopup_Click(object sender, RoutedEventArgs e)
        {
            cashPaymentPopup.IsOpen = false;
        }

        private void cashPaymentShowPopup_Click(object sender, RoutedEventArgs e)
        {
            showPayByCashWindow();
        }


        private void showPayByCashWindow()
        {
            if (m_listOfOutputObjects.Count > 0)
            {
                cashPaymentPopup.IsOpen = true;
                AmountPaidInCashBox.Focus();
            }
            else
            {
                throw new InvalidOperationException("No items to pay for.");
            }
        }
        //////////////////////////////////////////////////////////////////

        private void hidePayByCashWindow()
        {
            cashPaidPopup.IsOpen = false;
            UPCField.Focus();
        }

        private void WindowClicked(object sender, MouseButtonEventArgs e)
        {
            FocusManager.SetFocusedElement(this, UPCField);
        }

        public void Logout()
        {
            SnapRegisters.LoginMainWindow loginWindow = new SnapRegisters.LoginMainWindow();
            loginWindow.Show();
            m_connection.WriteNoResponse( "AddEmployeeSession @0, @1, @2, @3, @4", m_employee.ID, m_start, DateTime.Now, m_itemssold, m_totalsales );
            this.Close();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            //Logout();
            Main_Frame.Navigate(new OptionsPage(this));
            OptionsPage.Options_Frame = Main_Frame;
        }

        private void ChangeEmployeePermissions(ulong newPermissions)
        {
            m_employee = new Employee( m_employee.ID, m_employee.name, m_employee.address, m_employee.phoneNumber, m_employee.birthday, newPermissions, m_employee.PermissionGroup );
        }

        public void CashPaidResetRegister_Clicked(object sender, RoutedEventArgs e)
        {
            //TODO TESTING, REMOVE COMMENT AFTER DONE
            m_transaction.Checkout();
            ResetRegister();
            hidePayByCashWindow();
        }

        //Prevents user from typing anything other than numbers into the cash paid box
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

}

