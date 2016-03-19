using SnapRegisters;
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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.EmployeePages
{
    /// <summary>
    /// Interaction logic for RemoveEmployeePage.xaml
    /// </summary>
    public partial class RemoveEmployeePage :Page
    {
        public RemoveEmployeePage()
        {
            InitializeComponent();
        }

        private void SubmitButton_Click( object sender, RoutedEventArgs e )
        {
            DBInterface.RemoveEmployee(EmployeeIDBox.Text); 
        }

        private void ResetButton_Click( object sender, RoutedEventArgs e )
        {
            EmployeeIDBox.Clear();
        }
    }
}
