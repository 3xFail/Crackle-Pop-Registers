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
//using Snap_Admin_System_Interface.AdminWindowParts.WPF_UI;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        public InventoryPage()
        {
            InitializeComponent();
        }

        
        // opens the inventory page
        private void btn_AddEmp(object sender, RoutedEventArgs e)
        {
            //NavigationFrame.Navigate(new InventoryPage());
            //this.NavigationFrame.Navigate(new Uri("InventoryPage.xaml", UriKind.Relative));

            //AddEmployeePage add_emp = new AddEmployeePage();
            //NavigationService.Navigate = (new System.Uri("/AdminWindowParts/WPF UI/AddEmployeePage.xaml", UriKind.Relative));
            //add_emp.ShowsNavigationUI;
            //NavigationService navService = NavigationService.GetNavigationService(this);
            //NavigationService.Navigate = (new System.Uri("/AdminWindowParts/WPF UI/AddEmployeePage.xaml", UriKind.AbsoluteOrRelative);

            //NavigationFrame.Navigate(new InventoryPage());
            //this.NavigationFrame.Navigate(new Uri("/AdminWindowParts/WPF UI/InventoryPage.xaml", UriKind.Relative));
        }
    }
}
