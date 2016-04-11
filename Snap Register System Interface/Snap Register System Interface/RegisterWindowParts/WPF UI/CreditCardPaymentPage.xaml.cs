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
using SnapRegisters;
namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for CreditCardPaymentPage.xaml
    /// </summary>
    public partial class CreditCardPaymentPage : Page
    {
        private RegisterMainWindow m_win;
        public CreditCardPaymentPage(RegisterMainWindow win)
        {
            m_win = win;
            InitializeComponent();
        }

        private void Manual_Entry_CC_Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CC_Manual_Entry_Page(m_win));
        }

        private void Swipe_Entry_CC_Button_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
