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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        public static Frame InvFrame;
        public InventoryPage()
        {
            InitializeComponent();
        }

        private void CatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InvFrame.Navigate( new CatalogInventoryPage() );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void InvBackBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
