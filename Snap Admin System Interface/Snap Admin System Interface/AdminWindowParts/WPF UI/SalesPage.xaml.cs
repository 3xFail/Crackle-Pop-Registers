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
    /// Interaction logic for SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        public static Frame SalesFrame;
        public SalesPage()
        {
            InitializeComponent();
        }

        private void CouponsBtn_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                SalesFrame.Navigate( new CouponCatalog() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void SalesBtn_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                SalesFrame.Navigate( new SaleCatalog() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
    }
}
