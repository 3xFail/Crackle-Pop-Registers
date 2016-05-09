//#define NO_USERS_IN_DATABASE
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
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
//using System.Windows.Forms;
using System.Drawing;


namespace SnapRegisters
{
    // Document me.
    public partial class LoginMainWindow : Window
    {
        private bool _isLoggedIn { get; set; } = false;
        private ConnectionSession _connection;
        private Employee _loggedIn = null;
        private LoginDetails _lastAttempt;
        public static bool firstStartup = true;
        public LoginMainWindow()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(this, usernameField);


            if (firstStartup)
            {
                try
                {

                    _connection = new ConnectionSession("s", "1234");

                    XmlNode employee = _connection.Response[0];
                    if (employee.Get("Active")[0] == '0')
                        MessageBox.Show("This account is inactive.");
                    else
                    {





                        DBInterface.m_connection = _connection;


                        DBInterface.GetAllLogos();
                        string theNewImageString = DBInterface.Response[0].Get("LogoImage");

                        System.Drawing.Image newImage = LogoOperations.StringToImage(theNewImageString);

                        newImage.Save("..\\..\\..\\..\\SharedResources\\Images\\Emblem.png");


                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
                firstStartup = false;
            }



            System.Drawing.Image leEmblem = System.Drawing.Image.FromFile("..\\..\\..\\..\\SharedResources\\Images\\Emblem.png");



            System.Windows.Forms.PictureBox lePictureBox = new System.Windows.Forms.PictureBox();
            lePictureBox.Width = 200;
            lePictureBox.Image = leEmblem;



            windowsFormsHost1.Child = lePictureBox;







        }



        private void Login_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(string.IsNullOrEmpty(usernameField.Text) || string.IsNullOrEmpty(passwordField.Password));
        }

        private void Login_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _lastAttempt = new LoginDetails()
            {
                Password = passwordField.Password,
                Username = usernameField.Text
            };


#if NO_USERS_IN_DATABASE //For use if Ryan deleted all the employees lol
            loggedIn = new Employee(99, lastAttempt.Username, null, null, new DateTime(), 31);
#else
            ConnectToServer(_lastAttempt);
#endif
            try
            {
                if (_loggedIn != null)
                {
                    OpenInterfaceWindow(_loggedIn);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            passwordField.Clear();
        }



        private void OpenInterfaceWindow(Employee employeeLoggedIn)
        {

#if ADMIN
            if (!employeeLoggedIn.HasPermisison(Permissions.AdminLogIn))
                throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.AdminLogIn)); //("User does not have sufficient permissions to use this machine.");
            SnapRegisters.AdminMainWindow MainAdminWindow = new SnapRegisters.AdminMainWindow(employeeLoggedIn, _connection);
            MainAdminWindow.Show();
#elif REGISTER
            if (!Permissions.CheckPermissions(employeeLoggedIn, Permissions.RegisterLogIn))
                throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.RegisterLogIn)); //InvalidOperationException("User does not have sufficient permissions to use this machine.");
            SnapRegisters.RegisterMainWindow MainRegisterWindow = new SnapRegisters.RegisterMainWindow(employeeLoggedIn, _connection);
            MainRegisterWindow.Show();

#else
                MessageBox.Show("Success: Interface now on screen.");
#endif
        }


        private void ConnectToServer(LoginDetails attempt)
        {
            _loggedIn = null;
            _isLoggedIn = false;
            try
            {
                _connection = new ConnectionSession(attempt.Username, attempt.Password);

                XmlNode employee = _connection.Response[0];

                if (employee.Get("Active")[0] == '0')
                    MessageBox.Show("This account is inactive.");
                else
                {

                    string name = employee.Get("FName") + ' ' + employee.Get("LName");
                    ulong permissions = ulong.Parse(employee.Get("PermissionsID"));
                    string group = employee.Get("PermissionsGroup");
                    string phone = employee.Get("PhoneNumber");
                    int id = int.Parse(employee.Get("UserID"));

                    _loggedIn = new Employee(id, name, null, phone, new DateTime(1, 1, 1), permissions, group);

                    _isLoggedIn = true;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (_isLoggedIn)
            {
                if (_loggedIn.HasPermisison(Permissions.CanExitInterface))
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
            try
            {
                _lastAttempt = new LoginDetails()
                {
                    Password = passwordField.Password,
                    Username = usernameField.Text
                };

                ConnectToServer(_lastAttempt);

                if (!_isLoggedIn || !Permissions.CheckPermissions(_loggedIn, Permissions.CanAccessManagerFunctions))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.CanAccessManagerFunctions));

                btnShowPopup_Click(sender, e);
                passwordField.Clear();
            }
            catch (InvalidOperationException ex)
            {
                _isLoggedIn = false;
                _loggedIn = null;
                MessageBox.Show(ex.Message);
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
            try
            {
                if (!_isLoggedIn || !Permissions.CheckPermissions(_loggedIn, Permissions.CanRestartStation))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.CanRestartStation));

                System.Diagnostics.Process.Start("Shutdown", "-r -t 00");
                btnClosePopup_Click(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Shutdown_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_isLoggedIn || !Permissions.CheckPermissions(_loggedIn, Permissions.CanShutdownStation))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.CanShutdownStation));

                System.Diagnostics.Process.Start("Shutdown", "-s -t 00");
                btnClosePopup_Click(sender, e);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Username_Field_Got_Focus(object sender, RoutedEventArgs e)
        {
            usernameField.SelectAll();
        }

        private void Password_Field_Got_Focus(object sender, RoutedEventArgs e)
        {
            passwordField.SelectAll();
        }
    }
}