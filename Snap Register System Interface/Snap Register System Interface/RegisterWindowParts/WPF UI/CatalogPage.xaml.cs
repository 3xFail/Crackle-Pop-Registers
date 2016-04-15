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
    /// Interaction logic for CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        RegisterMainWindow m_win;
        public CatalogPage()
        {
            InitializeComponent();
        }
        public CatalogPage(RegisterMainWindow win)
        {
            InitializeComponent();
            m_win = win;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
