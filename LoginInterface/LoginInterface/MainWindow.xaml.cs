using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device.Location;
using System.Device;


using System.Security.Cryptography;

namespace WpfApplication2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class LoginDetails
        {
            public string Username { get; set; }
            public string Password { get; set; }

        }


        public MainWindow()
        {
            InitializeComponent();

        }


        private void Login_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(string.IsNullOrEmpty(usernameField.Text) || string.IsNullOrEmpty(passwordField.Password));
        }

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

        //Login code placeholder
        private void Login_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            LoginDetails attempt = new LoginDetails();


            attempt.Password = HashIt(passwordField.Password);
            attempt.Username = usernameField.Text;

            if (attempt.Username == "admin" && attempt.Password == HashIt("password"))
            {
                MessageBox.Show("Success!");
                Employee loggedIn = new Employee(10, "admin", null, "987654321" , new DateTime(1, 1, 1));
            }
            else
                MessageBox.Show("Failure");
           
        }

        
        


        private void Cancel_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cancel_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }





    }
    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
        (
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );


    }
}