using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device.Location;
using System.Device;
using CSharpClient;

using System.Security.Cryptography;

namespace SnapRegisters
{
	// Document me.
	public partial class LoginMainWindow : Window
	{
        private bool isLoggedIn= false;

		public LoginMainWindow()
		{
			InitializeComponent();

            FocusManager.SetFocusedElement(this, usernameField);

		}

		private void Login_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !(string.IsNullOrEmpty(usernameField.Text) || string.IsNullOrEmpty(passwordField.Password));
		}


		private void Login_Executed(object sender, ExecutedRoutedEventArgs e)
		{
            Employee loggedIn = null;

            if (!isLoggedIn)
            {
                LoginDetails attempt = new LoginDetails();

                attempt.Password = passwordField.Password;
                attempt.Username = usernameField.Text;
#if DEBUG
                loggedIn = ConnectToMockServer(attempt);
#else
			    ConnectToServer(attempt);
#endif
            }
            else
            {
                OpenInterfaceWindow(loggedIn);
                this.Close();
            }
        }






        private void OpenInterfaceWindow(Employee employeeLoggedIn)
		{




#if ADMIN
			SnapRegisters.AdminMainWindow MainAdminWindow = new SnapRegisters.AdminMainWindow(employeeLoggedIn);
			MainAdminWindow.Show();
#elif REGISTER
			SnapRegisters.RegisterMainWindow MainRegisterWindow = new SnapRegisters.RegisterMainWindow(employeeLoggedIn);
			MainRegisterWindow.Show();
#else
			MessageBox.Show("Success: Interface now on screen.");
#endif
		}
#if DEBUG

		private Employee ConnectToMockServer(LoginDetails attempt)
		{
            if (attempt.Username == "admin" && attempt.Password == "password")
            {
                Employee loggedIn = new Employee(10, "admin", null, "987654321", new DateTime(1, 1, 1), 31);
                //OpenInterfaceWindow(loggedIn);
                //this.Close();
                MessageBox.Show("Success!");
                isLoggedIn = true;
                return loggedIn;
            }
            else
            {
                MessageBox.Show("Failure");
                isLoggedIn = false;
                return null;
            }
        }
#else
		private void ConnectToServer(LoginDetails attempt)
		{
			connection_session connection;
			try
			{
				connection = new connection_session("172.20.10.10", 500, attempt.Username, attempt.Password);

				Employee loggedIn = new Employee(10, attempt.Username, null, "987654321", new DateTime(1, 1, 1), 31);

				OpenInterfaceWindow(loggedIn);

				this.Close();
			}
			catch (InvalidOperationException ee)
			{
				MessageBox.Show("Invalid username or password");

			}
			catch (Exception eee)
			{
				MessageBox.Show("Server not found");
			}
		}
#endif

		private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

        private void Management_Operations_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TOCODE
            MessageBox.Show("Manager functions pop up now");
        }

        private void Management_Operations_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Manager_Function_Button_Click(object sender, RoutedEventArgs e)
        {
            0
        }
    }
}