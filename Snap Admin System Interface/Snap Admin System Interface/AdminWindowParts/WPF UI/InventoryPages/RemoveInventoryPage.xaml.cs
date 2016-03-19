using CSharpClient;
using SnapRegisters;
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
    /// Interaction logic for RemoveInventoryPage.xaml
    /// </summary>
    public partial class RemoveInventoryPage :Page
    {
        public RemoveInventoryPage()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click( object sender, RoutedEventArgs e )
        {
            DBInterface.RemoveProducts(ItemIDBox.Text);
            
            if( DBInterface.Response[0].Get( "ProductID" ) == ItemIDBox.Text )
                System.Windows.Forms.MessageBox.Show( "Product ID " + ItemIDBox.Text + " has been disabled" );
            /*    
                  there are two errors that can be thrown back by the proc. 
                  -1 means that the UserID entered is not in the system
                  -2 means that the active bit is already false for this UserID's Account: R
            */
            else
                System.Windows.Forms.MessageBox.Show( DBInterface.Response[0].Get("ProductID" ) == "-1" ?
                    "ProductID (" + ItemIDBox.Text + ") does not exist in the database" : "This item (" + ItemIDBox.Text + ") is already disabled" );

            ItemIDBox.Clear();
        }

        private void ResetButton_Click( object sender, RoutedEventArgs e )
        {
            ItemIDBox.Clear();
        }
    }
}
