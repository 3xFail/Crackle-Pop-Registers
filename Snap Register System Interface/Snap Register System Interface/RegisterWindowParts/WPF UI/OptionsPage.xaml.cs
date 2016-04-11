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

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage :Page
    {

        public static Frame Options_Frame;
        public string m_stats_template = "{0} current stats\n\nTime logged in: {1}\n\nTotal Sales this login: {2}\n\nTotal number of items sold: {3}\n\nCurrent items per minute:{4}\n";
        private RegisterMainWindow m_win;
        public OptionsPage()
        {
            InitializeComponent();

            //create the string that makes the employee stats area
        }
        public OptionsPage(RegisterMainWindow win)
        {
            InitializeComponent();
            m_win = win;
            TimeSpan shift = ( DateTime.Now - m_win.m_start );
            statsBlock.Text = string.Format(m_stats_template, m_win.m_employee.name, shift.ToString( @"hh\:mm" ), m_win.m_totalsales.ToString( "C" ), m_win.m_itemssold, (m_win.m_itemssold / shift.TotalMinutes));
        }

        private void Logout_Button_Click( object sender, RoutedEventArgs e )
        {
            Options_Frame.Navigate(string.Empty);
            m_win.Logout();
        }

        private void Payment_Button_Click( object sender, RoutedEventArgs e )
        {
            Options_Frame.Navigate(new PaymentMenuPage(m_win));
            PaymentMenuPage.m_payment_menu_frame = Options_Frame;
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

        private void Add_Cust_Button_Click_1( object sender, RoutedEventArgs e )
        {

        }
    }
}
