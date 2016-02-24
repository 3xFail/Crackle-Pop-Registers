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
            if (!isLoggedIn || usernameField.Text != lastAttempt.Username)
            {
                lastAttempt = new LoginDetails();

                lastAttempt.Password = passwordField.Password;
                lastAttempt.Username = usernameField.Text;

                ConnectToServer(lastAttempt);
                if (isLoggedIn && !Permissions.CheckPermissions(loggedIn, Permissions.SystemPermissions.IS_OWNER))
                {
                    OpenInterfaceWindow(loggedIn);
                    this.Close();
                }
                if (loggedIn != null && Permissions.CheckPermissions(loggedIn, Permissions.SystemPermissions.IS_OWNER))
                    MessageBox.Show("Welcome " + loggedIn.name + ". You now have access to Manager Functions.");
            }
            else
            {
                if (usernameField.Text == lastAttempt.Username && passwordField.Password == lastAttempt.Password)
                {
                    OpenInterfaceWindow(loggedIn);
                    this.Close();
                }
                else
                {
                    isLoggedIn = false;
                    MessageBox.Show("Mismatched password: ERROR");

                }
            }
        }




        private void OpenInterfaceWindow(Employee employeeLoggedIn)
        {
#if ADMIN
			SnapRegisters.AdminMainWindow MainAdminWindow = new SnapRegisters.AdminMainWindow(employeeLoggedIn);
			MainAdminWindow.Show();
#elif REGISTER
            SnapRegisters.RegisterMainWindow MainRegisterWindow = new SnapRegisters.RegisterMainWindow(employeeLoggedIn, connection);
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
                loggedIn = null;
                connection = new connection_session(File.ReadAllText("sv_ip.txt"), 6119, attempt.Username, attempt.Password);

                connection.write(string.Format("GetEmployee_Username \"{0}\"", attempt.Username));
                //I know this will get SQL injected. Will fix ASAP.

                XmlNode item = connection.Response[0];

                long permissions = long.Parse(item.Attributes["PermissionsID"].Value);
                string phone = item.Attributes["EmployeePhone"].Value;
                int id = Int32.Parse(item.Attributes["UserID"].Value);
                string username = item.Attributes["Name"].Value;

                loggedIn = new Employee(id, username, null, phone, new DateTime(1, 1, 1), permissions);

                isLoggedIn = true;
            }
            catch (InvalidOperationException)
            {
                isLoggedIn = false;
                MessageBox.Show("Invalid username or password");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
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

        private void Management_Operations_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TOCODE
            //MessageBox.Show("Manager functions pop up now");
            //SnapRegisters.ManagerFunctionsPopup managerFunctionsPopup = new ManagerFunctionsPopup(this);
            //this.
            //managerFunctionsPopup.Show();
            if (usernameField.Text == lastAttempt.Username && passwordField.Password == lastAttempt.Password && Permissions.CheckPermissions(loggedIn, Permissions.SystemPermissions.IS_OWNER))
            {
                btnShowPopup_Click(sender, e);
            }
            else
            {
                isLoggedIn = false;
                MessageBox.Show("Mismatched password: Access to Manager Functions Denied");
            }



        }

        private void Management_Operations_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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