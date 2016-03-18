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
using System.Text.RegularExpressions;
using CurrencyTextBoxControl;
using System.ComponentModel;
using SnapRegisters;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for AddItemPage.xaml
    /// </summary>
    public partial class AddItemPage :Page
    {
        public AddItemPage()
        {
            InitializeComponent();
        }

        private void ResetButton_Click( object sender, RoutedEventArgs e )
        {
            BarcodeTextBox.Text = string.Empty;
            NameTextBox.Text = string.Empty;
            PriceTextBox.Number = 0M;
        }

        private void SubmitButton_Click( object sender, RoutedEventArgs e )
        {
            if( NameTextBox.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "An item name is required. Please enter it." );
            else if( BarcodeTextBox.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "An item barcode is required. Please enter it." );
            else if( PriceTextBox.Number == 0M )
                System.Windows.Forms.MessageBox.Show( "Items must have a non-zero price. Please enter one." );
            else
            {
                try
                {
                    DBInterface.AddItem( NameTextBox.Text, PriceTextBox.Number, BarcodeTextBox.Text );
                    System.Windows.Forms.MessageBox.Show( "\"" + NameTextBox.Text + "\" has been added!" );
                    ResetButton_Click( null, null );
                }
                catch( Exception ex )
                {
                    System.Windows.Forms.MessageBox.Show( ex.Message );
                }
            }
        }
    }
}
