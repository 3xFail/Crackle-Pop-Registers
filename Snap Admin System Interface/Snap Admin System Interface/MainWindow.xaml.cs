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

namespace Snap_Admin_System_Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Inv(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Visible;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        private void btn_Anal(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Visible;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        private void btn_Emp(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Visible;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        private void btn_Cust(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Visible;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        private void btn_Sales(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Visible;
            SalesTabs.Visibility = Visibility.Visible;
            OptionsTabs.Visibility = Visibility.Collapsed;
        }
        private void btn_Opt(object sender, RoutedEventArgs e)
        {
            InventoryTabs.Visibility = Visibility.Collapsed;
            AnalysisTabs.Visibility = Visibility.Collapsed;
            EmployeesTabs.Visibility = Visibility.Collapsed;
            CustomersTabs.Visibility = Visibility.Collapsed;
            SalesTabs.Visibility = Visibility.Collapsed;
            OptionsTabs.Visibility = Visibility.Visible;
        }

        private void EmployeesTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }   
}
