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
using Snap_Admin_System_Interface.AdminWindowParts.WPF_UI;



namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        // frame to reference the frame in the AdminMainWindow
        public static Frame EmpFrame;
        public EmployeesPage()
        {
            InitializeComponent();
        }
        // navigate to add employee page
        private void btn_AddEmp(object sender, RoutedEventArgs e)
        {
            // navigate to the add employee page
            EmpFrame.Navigate(new AddEmployeePage());
            EmpFrame.Navigate(new Uri("/AdminWindowParts/WPF UI/AddEmployeePage.xaml", UriKind.Relative));
        }
        // navigate to search employee page
        private void btn_SearchEmp(object sender, RoutedEventArgs e)
        {
            // navigate to the search employee page
            EmpFrame.Navigate(new SearchEmployeePage());
            EmpFrame.Navigate(new Uri("/AdminWindowParts/WPF UI/SearchEmployeePage.xaml", UriKind.Relative));
        }
    }
}
