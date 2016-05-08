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
using CurrencyTextBoxControl;
using Snap_Register_System_Interface.RegisterWindowParts.Business_Objects;
using SnapRegisters;
namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{

    /// <summary>
    /// Interaction logic for CashPaymentWindow.xaml
    /// </summary>
    public partial class CashPaymentPage : Page
    {
        private RegisterMainWindow m_win;
        private decimal _priceOfItems;

        CurrencyTextBox moneyAccepted = new CurrencyTextBox();
        public CashPaymentPage(RegisterMainWindow win)
        {
            m_win = win;
            _priceOfItems = win.m_totalTotal;
            InitializeComponent();
            initCurrencyTextBox();
            moneyAccepted.Focus();
        }


        private void initCurrencyTextBox()
        {
            moneyAccepted.HorizontalAlignment = HorizontalAlignment.Center;
            moneyAccepted.VerticalAlignment = VerticalAlignment.Center;
            moneyAccepted.FontSize = 20;
            mainGrid.Children.Add(moneyAccepted);
            Grid.SetRow(moneyAccepted, 1);

        }


        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            decimal diff = moneyAccepted.Number - _priceOfItems;
            if( diff >= 0 )
            {
                Change theChange = new Change( diff );
                NavigationService.Navigate( new CashPaymentFinished( theChange, m_win ) );
            }
        }
    }
}