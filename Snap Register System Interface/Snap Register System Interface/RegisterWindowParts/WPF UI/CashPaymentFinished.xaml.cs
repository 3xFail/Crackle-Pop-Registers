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
using Snap_Register_System_Interface.RegisterWindowParts.Business_Objects;
using SnapRegisters;

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for CashPaymentFinished.xaml
    /// </summary>
    public partial class CashPaymentFinished : Page
    {
        private RegisterMainWindow m_win;
        public CashPaymentFinished(Change changeToGive, RegisterMainWindow win)
        {
            InitializeComponent();

            m_win = win;

            hundreds.Text = changeToGive.hundreds.ToString();
            twenties.Text = changeToGive.twenties.ToString();
            tens.Text = changeToGive.tens.ToString();
            fives.Text = changeToGive.fives.ToString();
            ones.Text = changeToGive.ones.ToString();
            halfdollars.Text = changeToGive.halfdollars.ToString();
            quarters.Text = changeToGive.quarters.ToString();
            dimes.Text = changeToGive.dimes.ToString();
            nickels.Text = changeToGive.nickels.ToString();
            pennies.Text = changeToGive.pennies.ToString();

            ChangeTotal.Text = "$" + changeToGive.total;

        }

        private void ResetRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            m_win.ResetRegister();
            
        }
    }
}
