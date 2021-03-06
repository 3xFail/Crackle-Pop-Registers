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
using SnapRegisters;

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for GiftCardPaymentPage.xaml
    /// </summary>
    public partial class GiftCardPaymentPage : Page
    {
        private RegisterMainWindow m_win;
        public GiftCardPaymentPage(RegisterMainWindow win)
        {
            m_win = win;
            InitializeComponent();
        }

        private void Gift_Card_Entry_Next_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Gift_Card_Entry_Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(string.Empty);
        }
    }
}
