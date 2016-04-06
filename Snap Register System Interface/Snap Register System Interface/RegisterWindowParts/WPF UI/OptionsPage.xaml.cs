﻿using SnapRegisters;
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
        public OptionsPage()
        {
            InitializeComponent();

            //create the string that makes the employee stats area
        }
        public OptionsPage(RegisterMainWindow win)
        {
            InitializeComponent();
            TimeSpan shift = ( DateTime.Now - win.m_start );
           
            statsBlock.Text = string.Format(m_stats_template, win.m_employee.name, shift.TotalMinutes, win.m_totalsales, win.m_itemssold, (win.m_itemssold / shift.TotalMinutes));
        }

        private void Logout_Button_Click( object sender, RoutedEventArgs e )
        {
           // Logout();
        }

        private void Shutdown_Button_Click( object sender, RoutedEventArgs e )
        {
            //Shutdown();
        }

        private void Payment_Button_Click( object sender, RoutedEventArgs e )
        {
            Options_Frame.Navigate(new PaymentMenuPage());
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
