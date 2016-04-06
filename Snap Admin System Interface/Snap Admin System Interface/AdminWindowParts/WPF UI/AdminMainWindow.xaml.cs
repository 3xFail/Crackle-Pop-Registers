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
//using SnapRegisters.AdminWindowParts.WPF_UI;
using System.Windows.Threading;
using Snap_Admin_System_Interface.AdminWindowParts.WPF_UI;
using CSharpClient;
using Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages;

namespace SnapRegisters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        private Employee m_employee = null;
        //private DockPanel m_itemPanel = null;
        //private DockPanel m_discountList = null;

        public AdminMainWindow(Employee currentEmployee, ConnectionSession connection)
        {
            DBInterface.m_connection = connection;
            DBInterface.m_employee = currentEmployee;

            DBInterface.Log( "Logged in" );
            // update the clock manually
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.dateText.Text = DateTime.Now.ToString("hh:mm tt");
            }, this.Dispatcher);

            InitializeComponent();
            m_employee = currentEmployee;

            // set the username to the employee that logged in
            LoggedInAs.Text = currentEmployee.name;

        }
        // opens the inventory page
        private void btn_Inv(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationFrame.Navigate( new CatalogInventoryPage() );

                InventoryPage.InvFrame = NavigationFrame;
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
        // opens the analysis page
        private void btn_Anal(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationFrame.Navigate( new AnalysisPage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
        // opens the employees page
        private void btn_Emp(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationFrame.Navigate( new EmployeesPage() );

                // reference the AdminMainWindow frame
                EmployeesPage.EmpFrame = NavigationFrame;
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
        // opens the customers page
        private void btn_Cust(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationFrame.Navigate( new CustomersPage() );

                // reference the AdminMainWindow frame
                CustomersPage.CustFrame = NavigationFrame;
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
        // opens the sales page
        private void btn_Sales(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationFrame.Navigate( new SalesPage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
        // opens the options page
        private void btn_Opt(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationFrame.Navigate( new OptionsPage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
        private void Window_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            DBInterface.Log( "Logged out" );
        }
        private void Logout()
        {
            SnapRegisters.LoginMainWindow loginWindow = new SnapRegisters.LoginMainWindow();
            loginWindow.Show();
            this.Close();
        }
        // logs out of the system
        private void btn_Logout(object sender, RoutedEventArgs e)
        {
            Logout();
        }
    }
}

