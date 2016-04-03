﻿using System;
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
        public static Frame m_payment_frame; 
        public PaymentMenuPage()
        {
            InitializeComponent();
        }

        private void Cash_Button_Click(object sender, RoutedEventArgs e)
        {
            m_payment_frame.Navigate( new CashPaymentPage() );
        }

        private void Credit_Button_Click(object sender, RoutedEventArgs e)
        {
            //m_payment_frame.Navigate();
        }

        private void GiftCard_Button_Click(object sender, RoutedEventArgs e)
        {
            //m_payment_frame.Navigate();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            //needs to close the frame view... not sure how todo that
        }
    }
}
