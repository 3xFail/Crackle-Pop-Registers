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
using System.Windows.Shapes;
using PointOfSales.Users;
using PointOfSales.Permissions;
namespace SnapRegisters
{
	//*************************************************************************************************************
	// public partial class ManagerOverrideMenu : Window
	//		SUMMARY:
	//			This window allows the temporary changing of an employee's permissions for a sale. The permission
	//			level of the employee will be changed to the permission level of the scanners.
	//		MEMBERS:
	//			private Employee m_loggedInEmployee
	//				The employee logged into the register. This employee might have it's permissions changed.
	//			private SystemPermissions m_scannedPermissions
	//				The new permission level of the employee.
	//		WPF NAMES:
	//			TextBox UPCField
	//				This is the field the bar-code is entered in. this field has 0 width, height and is hidden so
	//				the bar-code is never shown.
	//			Button Override
	//				This button becomes available once valid employee credentials are scanned. Closes this window
	//				and sets the logged in employee's permissions to the permission level of the scanned employee.
	//			TextBlock OverrideText
	//				This is the text on the button. It changes if valid employee credentials were scanned.
	//		FUNCTIONS:
	//			public ManagerOverrideMenu(ref Employee loggedInEmployee)
	//				Initializes a new ManagerOverrideMenu. loggedInEmployee's permissions will be potentially
	//				changed so it is passed by reference.
	//			private GetPermissions(string employeeIDCode)
	//				Takes the employeeIDCode and returns the permission level of that employee retrieved from the
	//				database.
	//		EVENTS:
	//			KeyPressed
	//				Sender - MainWindow
	//				Event - KeyDown
	//				Action
	//					Manages the targeting of the invisible UPC box so that it is difficult to type into without
	//					the scanner.
	//			WindowDeactivated
	//				Sender - MainWindow
	//				Event - Deactivated
	//				Action
	//					If this window is clicked out of, close it.
	//			Override_CanExecute
	//				Sender - MainWindow
	//				Event - CanExecute
	//				Action
	//					Changes the text on Override and lights it up if valid credentials were scanned.
	//			Override_Execute
	//				Sender - MainWindow
	//				Event - Execute
	//				Action
	//					Changes the permissions of the logged in employee and closes this window.
	//		PERMISSIONS:
	//*************************************************************************************************************
	public delegate void AssignPermissionsDelegate(ulong permissionsToAssign);
	public partial class ManagerOverrideMenu : Page
	{
        RegisterMainWindow m_win;
		public ManagerOverrideMenu(AssignPermissionsDelegate permissionsOutput)
		{
			InitializeComponent();
			m_permissionsOutput = permissionsOutput;
			m_scannedPermissions = 0;
		}

        public ManagerOverrideMenu(RegisterMainWindow win,AssignPermissionsDelegate permissionsOutput)
        {
            InitializeComponent();
            m_permissionsOutput = permissionsOutput;
            m_scannedPermissions = 0;
            m_win = win;
        }
        public void GetPermissions(string employeeIDCode)
		{
            //TODO: Replace this with getting actual permissions.
            m_scannedPermissions = ulong.MaxValue;
		}


		private AssignPermissionsDelegate m_permissionsOutput;
		private ulong m_scannedPermissions;
		private void KeyPressed(object sender, KeyEventArgs keyPressed)
		{
			if (keyPressed.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
			{
				FocusManager.SetFocusedElement(this, UPCField);
				UPCField.Clear();
			}

			if (keyPressed.Key == Key.Enter)
				if (UPCField.Text != string.Empty)
				{
					GetPermissions(UPCField.Text);
					UPCField.Clear();
				}
		}
        private void Override_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = m_scannedPermissions != 0;
            if (e.CanExecute)
                UPCField.Text = "Override Login";
            else
                UPCField.Text = "Scan Override Code";
        }
        private void Override_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            m_permissionsOutput(m_scannedPermissions);
            m_win.Main_Frame.Navigate(string.Empty);
        }

        private void Override_Click(object sender, RoutedEventArgs e)
        {
            //prompt for manager override code
            this.NavigationService.Navigate(new ManagerOverrideBox());
        }
    }
}
