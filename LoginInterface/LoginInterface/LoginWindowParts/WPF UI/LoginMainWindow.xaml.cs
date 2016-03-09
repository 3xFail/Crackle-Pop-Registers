#define SERVER_DOWN
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
using System.Management;
using SnapRegisters;

using System.Security.Cryptography;
using System.Xml;

namespace SnapRegisters
{
    // Document me.
    public partial class LoginMainWindow : Window
    {
        private bool isLoggedIn = false;
        private connection_session connection;
        private Employee loggedIn = null;
        private LoginDetails lastAttempt;

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
            lastAttempt = new LoginDetails()
            {
                Password = passwordField.Password,
                Username = usernameField.Text
            };


#if SERVER_DOWN
            loggedIn = new Employee(99, lastAttempt.Username, null, null, new DateTime(), 31);
#else
            ConnectToServer(lastAttempt);
#endif
            OpenInterfaceWindow(loggedIn);
            this.Close();
        }

        private void OpenInterfaceWindow(Employee employeeLoggedIn)
        {
#if ADMIN
			SnapRegisters.AdminMainWindow MainAdminWindow = new SnapRegisters.AdminMainWindow(employeeLoggedIn, connection);
			MainAdminWindow.Show();
#elif REGISTER
            SnapRegisters.RegisterMainWindow MainRegisterWindow = new SnapRegisters.RegisterMainWindow(employeeLoggedIn, connection);
            MainRegisterWindow.Show();

#else
			MessageBox.Show("Success: Interface now on screen.");
#endif
        }

        private void ConnectToServer(LoginDetails attempt)
        {
            loggedIn = null;
            isLoggedIn = false;
            try
            {
                connection = new connection_session( File.ReadAllText( "sv_ip.txt" ), 6119, attempt.Username, attempt.Password );

                connection.write( string.Format( "GetEmployee_Username \"{0}\"", attempt.Username ) );

                XmlNode item = connection.Response[0];

                long permissions = long.Parse( item.Get( "PermissionsID" ) );
                string phone = item.Get( "PhoneNumbers" );
                int id = Int32.Parse( item.Get( "UserID" ) );
                string username = item.Get( "FName" ) + ' ' + item.Get("LName") ;
                //string Address = item.Get( "Address" );

                loggedIn = new Employee( id, username, null, phone, new DateTime( 1, 1, 1 ), permissions );

                isLoggedIn = true;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show( "Invalid username or password" );
            }
            catch (Exception ee)
            {
                MessageBox.Show( ee.Message );
            }
        }

        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (isLoggedIn)
            {
                if (Permissions.CheckPermissions(loggedIn, Permissions.SystemPermissions.IS_OWNER))
                {
                    e.CanExecute = true;
                }
            }
        }

        private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Management_Operations_Executed( object sender, ExecutedRoutedEventArgs e )
        {
            lastAttempt = new LoginDetails()
            {
                Password = passwordField.Password,
                Username = usernameField.Text
            };

            ConnectToServer( lastAttempt );

            if (isLoggedIn && Permissions.CheckPermissions(loggedIn, Permissions.SystemPermissions.IS_OWNER))
            {
                btnShowPopup_Click(sender, e);
            }
            else
            {
                isLoggedIn = false;
                loggedIn = null;
                MessageBox.Show("Access to Manager Functions Denied");
            }
        }

        private void Management_Operations_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = usernameField.Name != string.Empty && passwordField.Password != string.Empty;
        }

        private void btnClosePopup_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }

        private void btnShowPopup_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = true;
        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("Shutdown", "-r -t 00");
            btnClosePopup_Click(sender, e);
        }

        private void Shutdown_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("Shutdown", "-s -t 00");
            btnClosePopup_Click(sender, e);
        }
    }
}