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
using SnapRegisters;
using PointOfSales.Permissions;

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
            try
            {
                EmpFrame.Navigate( new AddEmployeePage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void EmpCatalog_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                if (!Permissions.CheckPermissions(DBInterface.m_employee, Permissions.ViewEmployeeCatalog))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.ViewEmployeeCatalog));

                EmpFrame.Navigate( new CatalogEmployeePage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void EmpUsageBtn_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                EmpFrame.Navigate( new UsageEmployeePage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void EmpLogBtn_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                EmpFrame.Navigate( new LogEmployeePage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void EmpPermissions_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                EmpFrame.Navigate( new PermissionsPage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
    }
}
