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

		public LoginMainWindow()
		{
			InitializeComponent();

		}

		private void Login_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !(string.IsNullOrEmpty(usernameField.Text) || string.IsNullOrEmpty(passwordField.Password));
		}


		private void Login_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Employee loggedIn = null;

			LoginDetails attempt = new LoginDetails();

			attempt.Password = passwordField.Password;
			attempt.Username = usernameField.Text;
#if DEBUG
			loggedIn = ConnectToMockServer(attempt);
#else
			loggedIn = ConnectToServer(attempt);
#endif
			if (loggedIn != null)
			{
				OpenInterfaceWindow(loggedIn);
				this.Close();
			}
			else
				MessageBox.Show("Failed to connect.");
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
				return loggedIn;
			}
			else
			{
				return null;
			}
		}
#else
		private Employee ConnectToServer(LoginDetails attempt)
		{
			connection_session connection;
			try
			{
				connection = new connection_session("172.20.10.10", 500, attempt.Username, attempt.Password);

				Employee loggedIn = new Employee(10, attempt.Username, null, "987654321", new DateTime(1, 1, 1), 31);

				return loggedIn;
			}
			catch (InvalidOperationException ee)
			{
				MessageBox.Show("Invalid username or password");
				return null;

			}
			catch (Exception eee)
			{
				MessageBox.Show("Server not found");
				return null;
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

		}

		private void Management_Operations_Can_Execute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = false;
		}
	}
}