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
			InitializeComponent();

			m_employee = currentEmployee;
            m_connection = session;
			m_transaction = new Transaction(m_employee, AddItemToOutputPanels, m_connection);
			FocusManager.SetFocusedElement(this, UPCField);

		}

		private void AddItemToOutputPanels(Item itemToAdd)
		{
			MessageBox.Show(itemToAdd.ItemName);


			//ItemDisplayBox itemDescription = new ItemDisplayBox(itemToAdd);

			//// Bind the item's price. type's don't make sense here.
			//Binding ItemPriceBinding = new Binding();
			//ItemPriceBinding.Source = itemToAdd;
			//ItemPriceBinding.Path = new PropertyPath(itemToAdd.Price);
			//itemDescription.itemPrice.SetBinding(TextBlock.TextProperty, ItemPriceBinding);
			TextBlock x = new TextBlock();
			x.Text = "Item";
			ItemsList.Children.Add(x);

		}
        private connection_session m_connection = null;
		private Transaction m_transaction = null;
		private Employee m_employee = null;

		private DockPanel m_itemPanel = null;
		private DockPanel m_discountList = null;

		private void ShortcutKeyPressed(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
				UPCField.Clear();

			if (e.Key == Key.Enter)
				try {
						m_transaction.AddItem(UPCField.Text);
						UPCField.Clear();
					}
				catch (Exception error) { }
			if (e.Key == Key.Escape)
				FocusManager.SetFocusedElement(this, UPCField);
		}
		private void WindowClicked(object sender, MouseButtonEventArgs e)
		{
			FocusManager.SetFocusedElement(this, UPCField);
		}
	}
}

