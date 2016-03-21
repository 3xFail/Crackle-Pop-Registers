using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using CSharpClient;
using SnapRegisters;
using System.ComponentModel;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    public class LogData
    {
        public string Username { get; set; }
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public string Event { get; set; }
    }
    /// <summary>
    /// Interaction logic for LogEmployeePage.xaml
    /// </summary>
    public partial class LogEmployeePage :Page
    {
        private ObservableCollection<LogData> data = new ObservableCollection<LogData>();
        public LogEmployeePage()
        {
            InitializeComponent();
            StartDatePicker.SelectedDate = DateTime.Now.AddMonths( -1 );
            EndDatePicker.SelectedDate = DateTime.Now.AddDays( 1 );
            PopulateList();
        }

        private void PopulateList()
        {
            data.Clear();

            DBInterface.GetLogs( UsernameTextBox.Text == string.Empty ? null : UsernameTextBox.Text, StartDatePicker.SelectedDate, EndDatePicker.SelectedDate);

            foreach( XmlNode node in DBInterface.Response )
            {
                data.Add( new LogData()
                {
                    Username = node.Get( "Username" )
                    , ID = int.Parse( node.Get( "UserID") )
                    , Time = DateTime.Parse( node.Get( "Time" ) )
                    , Event = node.Get( "Event" )
                } );
            }
            LoadItems();
        }

        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
                LogGrid.ItemsSource = data_cv;
        }

        private void SearchButton_Click( object sender, RoutedEventArgs e )
        {
            PopulateList();
        }
    }
}
