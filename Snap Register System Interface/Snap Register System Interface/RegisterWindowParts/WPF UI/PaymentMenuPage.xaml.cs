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
    /// Interaction logic for PaymentMenuPage.xaml
    /// </summary>
    public partial class PaymentMenuPage : Page
    {
        public static Frame m_payment_menu_frame;
        private RegisterMainWindow m_win;

        public PaymentMenuPage(RegisterMainWindow win)
        {
            InitializeComponent();
            m_win = win;
            Cash_Button.Focus();
        }


        private void Cash_Button_Click(object sender, RoutedEventArgs e)
        {
            m_payment_menu_frame.Navigate( new CashPaymentPage(m_win));
            
        }

        private void Credit_Button_Click(object sender, RoutedEventArgs e)
        {
            m_payment_menu_frame.Navigate(new CreditCardPaymentPage(m_win));
        }

        private void GiftCard_Button_Click(object sender, RoutedEventArgs e)
        {
            m_payment_menu_frame.Navigate(new GiftCardPaymentPage(m_win));
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            //used to close the frame view
            m_payment_menu_frame.Navigate(string.Empty);
            m_win.UPCField.Focus();
            
        }
    }
}
