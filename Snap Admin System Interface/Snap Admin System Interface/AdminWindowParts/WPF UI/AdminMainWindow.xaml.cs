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


namespace SnapRegisters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow(Employee currentEmployee)
        {
            InitializeComponent();
			//m_employee = currentEmployee;
        }

        //public AddPage(Employee currentEmployee)
        //{
        //    //InitializeComponent();
        //}

        // opens the inventory page
        private void btn_Inv(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Visible;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        // opens the analytics page
        private void btn_Anal(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Visible;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        // opens the employees page
        private void btn_Emp(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Visible;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        // opens the cutomers page
        private void btn_Cust(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Visible;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        // opens the sale page
        private void btn_Sales(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Visible;
            SalesTabs.Visibility = Visibility.Visible;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        // opens the options page
        private void btn_Opt(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Visible;
        }

        
		
		//private Employee m_employee = null;

		//private DockPanel m_itemPanel = null;
		//private DockPanel m_discountList = null;
	}   
}
