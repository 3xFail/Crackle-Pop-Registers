using CSharpClient;
using Snap_Register_System_Interface.RegisterWindowParts.Business_Objects;
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
using System.Xml;

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for GetCustomerPage.xaml
    /// </summary>
    public partial class GetCustomerPage : Page
    {
        RegisterMainWindow m_win;
        public GetCustomerPage()
        {
            InitializeComponent();
        }

        public GetCustomerPage(RegisterMainWindow win)
        {
            InitializeComponent();
            m_win = win;
        }


        private void submit_button_Click(object sender, RoutedEventArgs e)
        {
            try {
                DBInterface.m_connection = m_win.m_connection;

                DBInterface.GetCustomer( Phone_Number_Box.Text );

                XmlNode it = m_win.m_connection.Response[0];

                m_win.m_customer = new Customer( it.Get( "FName" ), 
                    it.Get( "LName" ), 
                    it.Get( "PhoneNumber" ), 
                    it.Get( "Email" ), 
                    int.Parse( it.Get( "RewardsID" ) ), 
                    int.Parse( it.Get( "RewardsPoints" ) ), 
                    int.Parse( it.Get( "CustID" ) ) );

                m_win.m_transaction.m_customer = m_win.m_customer;

                System.Windows.Forms.MessageBox.Show( "Good to see you again " + m_win.m_customer.fname + "!" );
            }
            catch (NullReferenceException)
            {
                System.Windows.Forms.MessageBox.Show( "There is not a customer with that number" );
            }

        }

        private void cancel_button_Click(object sender, RoutedEventArgs e)
        {
            m_win.Main_Frame.Navigate(string.Empty);
        }
    }
}
