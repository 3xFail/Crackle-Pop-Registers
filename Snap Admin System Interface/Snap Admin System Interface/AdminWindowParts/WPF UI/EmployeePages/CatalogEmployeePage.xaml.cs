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
using System.Collections.ObjectModel;
using System.Xml;
using CSharpClient;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{

    /// <summary>
    /// Interaction logic for SearchEmployeePage.xaml
    /// </summary>
    public partial class SearchEmployeePage : Page
    {
        public ObservableCollection<UsageData> data = new ObservableCollection<UsageData>();
        public SearchEmployeePage()
        {
            InitializeComponent();
            PopulateList();
        }

        private void PopulateList()
        {
            //DBInterface.GetAllUsers(); //TODO: IMPLIMENT

            foreach( XmlNode node in DBInterface.Response )
            {

            }

            
        }
    }
}
