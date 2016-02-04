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
			LoginDetails attempt = new LoginDetails();
			
			attempt.Password = passwordField.Password;
			attempt.Username = usernameField.Text;
#if DEBUG
			ConnectToMockServer(attempt);
#else
			ConnectToServer(attempt);
#endif
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
			MessageBox.Show("Success!");
#endif
		}
#if DEBUG
		public static string HashIt(string input)
		{

		    MD5 hasher = MD5.Create();
		    byte[] inputBytes = Encoding.ASCII.GetBytes(input);
		    byte[] hashBytes = hasher.ComputeHash(inputBytes);


		    StringBuilder stringBuilder = new StringBuilder();
		    for (int idx = 0; idx < hashBytes.Length; idx++)
		    {
		        stringBuilder.Append(hashBytes[idx].ToString("X2"));
		    }
		    return stringBuilder.ToString();
		}

		private void ConnectToMockServer(LoginDetails attempt)
		{
			if (attempt.Username == "admin" && attempt.Password == HashIt("password"))
			{
				Employee loggedIn = new Employee(10, "admin", null, "987654321", new DateTime(1, 1, 1), 31);
				OpenInterfaceWindow(loggedIn);
				this.Close();
			}
			else
				MessageBox.Show("Failure");    
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
	}
}