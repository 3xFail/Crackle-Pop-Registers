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

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage :Page
    {
        public static Frame Options_Frame;
        public OptionsPage()
        {
            InitializeComponent();

            //create the string that makes the employee stats area
        }

        private void Logout_Button_Click( object sender, RoutedEventArgs e )
        {
           // Logout();
        }

        private void Shutdown_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Payment_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Add_Discount_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Remove_Coupon_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Override_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Catalog_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Remove_Item_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Remove_Discount_Button_Click( object sender, RoutedEventArgs e )
        {

        }

        private void Close_Button_Click( object sender, RoutedEventArgs e )
        {
            Options_Frame.Navigate(string.Empty);
        }
    }
}
