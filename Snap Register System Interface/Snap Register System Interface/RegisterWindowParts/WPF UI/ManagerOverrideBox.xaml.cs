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
using CSharpClient;

namespace SnapRegisters
{
    /// <summary>
    /// Interaction logic for ManagerOverrideBox.xaml
    /// </summary>
    public partial class ManagerOverrideBox : Page
    {
        public ManagerOverrideBox()
        {
            InitializeComponent();
        }

        private void Override_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter: Enter a code
            if (e.Key == Key.Enter)
            {
                try
                {
                    if (Override.Text != string.Empty)
                    {
                        //check if a valid manager code
                        DBInterface.GetEmp(Override.Text);
                        DBInterface.Response[0].Get("PermissionsID");
                        Override.Clear();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception _ex) //if that fails
                {
                    MessageBox.Show(_ex.Message);//if both of those fail show the error message
                    UPCField.Clear();
                }
            }

            if (e.Key == Key.Escape)
            { 
                FocusManager.SetFocusedElement(this, UPCField);
                //close box
            }
        }
    }
}
