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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for SearchInventoryPage.xaml
    /// </summary>
    public partial class SearchInventoryPage :Page
    {
        public SearchInventoryPage()
        {
            InitializeComponent();
        }

        private void Commit( object sender, RoutedEventArgs e )
        {
            //Todo: Send all changes to the item in the line that the button belonged to, to the datebase

            //does this associate with the individual row or with the whole file?: R
        }

        private void Search_TextChanged( object sender, TextChangedEventArgs e )
        {

        }
    }
}
