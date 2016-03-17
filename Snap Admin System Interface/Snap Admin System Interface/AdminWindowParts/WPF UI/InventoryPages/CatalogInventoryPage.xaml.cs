using System;
using System.Collections.Generic;
using System.Data;
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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for CatalogInventoryPage.xaml
    /// </summary>
    public partial class CatalogInventoryPage : Page
    {
        
        public CatalogInventoryPage()
        {
            InitializeComponent();
        }

        

        private DataTable m_datatable { get; set; }
    }
}
