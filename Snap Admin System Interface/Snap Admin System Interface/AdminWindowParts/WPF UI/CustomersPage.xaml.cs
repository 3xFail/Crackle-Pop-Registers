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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for CustomersPage.xaml
    /// </summary>
    public partial class CustomersPage : Page
    {
        // frame to reference the frame in the AdminMainWindow
        public static Frame CustFrame { get; set; }

        public CustomersPage()
        {
            InitializeComponent();
        }
        // navigate to add employee page
        private void btn_AddCust(object sender, RoutedEventArgs e)
        {
            try
            {
                // navigate to the add employee page
                //CustFrame.Navigate( new AddCustomerPage() );
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(new AddCustomerPage());
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void CustCatalogBtn_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                //CustFrame.Navigate( new CatalogCustomerPage() );
                NavigationService nav = NavigationService.GetNavigationService(this);
                nav.Navigate(new CatalogCustomerPage());
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
    }
}
