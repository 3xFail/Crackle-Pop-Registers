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

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public partial class MainWindow : Window
	//		SUMMARY:
	//			This is the main class that drives the main window of the user interface. It contains functions
	//			used to move the sale forward and display the information to the screen.
	//		MEMBERS:
	//			private Transaction m_transaction
	//				Contains the data of the sale as well as manipulation of that data.
	//			private Employee m_employee
	//				The employee who is currently using the register. This data is pulled from the login page.
	//		FUNCTIONS:
	//			public MainWindow(Employee currentEmployee)
	//				Constructs the main sales window. An employee must be specified in order to check permissions.
	//			private UpdateItemDisplay
	//		PERMISSIONS:
	//			Permissions are handled by the Transaction class.
	//*************************************************************************************************************
	public partial class RegisterMainWindow : Window
	{
        // temp constructor for testing.
        //public RegisterMainWindow()
        //{
        //	InitializeComponent();

        //	Employee currentEmployee = new Employee(1, "Joe", null, "5", new DateTime(1,2,3), 255);
        //	m_employee = currentEmployee;
        //	m_transaction = new Transaction(m_employee, AddItemToOutputPanels);

        //	m_itemPanel = ItemsList;
        //	m_discountList = CouponList;
        //}

		public RegisterMainWindow(Employee currentEmployee, connection_session session)
		{
            //Updates the clock constantly
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.dateText.Text = DateTime.Now.ToString("hh:mm tt");
            }, this.Dispatcher);

            //Delays showing the window until the clock is guaranteed to have already ticked once (ticks once per second)
            System.Threading.Thread.Sleep(1100);


            //Initialize window after the clock is ticked
            InitializeComponent();




            m_employee = currentEmployee;
            m_connection = session;
			m_transaction = new Transaction(m_employee, AddItemToOutputPanels, m_connection);

			FocusManager.SetFocusedElement(this, UPCField);



            

            //Sets the username to the employee that logged in
            LoggedInAs.Text = currentEmployee.name;

		}

		private void AddItemToOutputPanels(Item itemToAdd)
		{
			//MessageBox.Show(itemToAdd.ItemName);


			ItemDisplayBox itemDescription = new ItemDisplayBox(itemToAdd);
			itemDescription.Height = 100;
			//// Bind the item's price. type's don't make sense here.
			//Binding ItemPriceBinding = new Binding();
			//ItemPriceBinding.Source = itemToAdd;
			//ItemPriceBinding.Path = new PropertyPath(itemToAdd.Price);
			//itemDescription.itemPrice.SetBinding(TextBlock.TextProperty, ItemPriceBinding);
			ItemsList.Children.Add(itemDescription);

			m_costTotal += itemToAdd.Price;
			m_totalTotal += itemToAdd.Price;

			UpdateTotals();
		}

		private void UpdateTotals()
		{
			CostTotal.Text = m_costTotal.ToString( "C" );
			SavingsTotal.Text = m_savingsTotal.ToString( "C" );
			Total.Text = m_totalTotal.ToString( "C" );
		}


        private connection_session m_connection = null;
		private Transaction m_transaction = null;
		private Employee m_employee = null;
		private double m_costTotal = 0;
		private double m_savingsTotal = 0;
		private double m_totalTotal = 0;
		private void ShortcutKeyPressed(object sender, KeyEventArgs e)
		{
			if (UPCField.Text == string.Empty)
				return;

			if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				UPCField.Clear();

            if( e.Key == Key.Enter )
            {
                try
                {
                    m_transaction.AddItem( UPCField.Text );
                    UPCField.Clear();
                }
                catch( Exception ex )
                {
                    MessageBox.Show( ex.Message );
                }
            }
			else if (e.Key == Key.Escape)
				FocusManager.SetFocusedElement(this, UPCField);
		}
		private void WindowClicked(object sender, MouseButtonEventArgs e)
		{
			FocusManager.SetFocusedElement(this, UPCField);
		}
	}
}

