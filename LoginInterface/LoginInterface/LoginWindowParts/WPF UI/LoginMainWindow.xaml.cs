using System;
using System.IO;
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
using System.Xml;

namespace SnapRegisters
{
	// Document me.
	public partial class LoginMainWindow : Window
	{
        private bool isLoggedIn = false;
        private connection_session connection;
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

            if (!isLoggedIn)
            {
                LoginDetails attempt = new LoginDetails();

                attempt.Password = passwordField.Password;
                attempt.Username = usernameField.Text;

			    ConnectToServer(attempt);
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

		private void ConnectToServer(LoginDetails attempt)
		{
			try
			{
				connection = new connection_session(File.ReadAllText("sv_ip.txt"), 6119, attempt.Username, attempt.Password);

                connection.write("EXEC	[dbo].[GetEmployee_Username] \"" + attempt.Username + "\"");

                XmlNode item = connection.Response[0];

                int permissions = Int32.Parse(item.Attributes["PermissionsID"].Value);
                string phone = item.Attributes["EmployeePhone"].Value;
                int id = Int32.Parse(item.Attributes["UserID"].Value);

				Employee loggedIn = new Employee(id, attempt.Username, null, phone, new DateTime(1, 1, 1), permissions);

                isLoggedIn = true;
				OpenInterfaceWindow(loggedIn);

				this.Close();
			}
			catch (InvalidOperationException )
			{
				MessageBox.Show( "Invalid username or password" );

			}
			catch (Exception ee)
			{
				MessageBox.Show(ee.ToString());
			}
		}

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