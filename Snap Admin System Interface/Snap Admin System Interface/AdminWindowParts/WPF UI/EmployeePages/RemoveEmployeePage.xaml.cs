using CSharpClient;
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

            if(DBInterface.Response[0].Get("UserID") == EmployeeIDBox.Text)
                System.Windows.Forms.MessageBox.Show( "User account " + EmployeeIDBox.Text + " has been disabled" );
            else
            {
                /*there are two errors that can be thrown back by the proc. 
                  -1 means that the UserID entered is not in the system
                  -2 means that the active bit is already false for this UserID's Account: R
                */
                System.Windows.Forms.MessageBox.Show( DBInterface.Response[0].Get( 
                    "UserID" ) == "-1" ? "UserID (" + EmployeeIDBox.Text + ") does not exist in the database" : "This account (" + EmployeeIDBox.Text + ") is already disabled" );
            }

        }

        private void ResetButton_Click( object sender, RoutedEventArgs e )
        {
            EmployeeIDBox.Clear();
        }
    }
}
