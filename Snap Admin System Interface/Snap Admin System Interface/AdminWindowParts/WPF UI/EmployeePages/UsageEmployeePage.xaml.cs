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
using System.Xml;
using System.Collections.ObjectModel;
using CSharpClient;
using System.ComponentModel;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    public class UsageData
    {
        public string Username { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public string Duration { get; set; }
        public TimeSpan Duration_TimeSpan { get; set; } //for efficient filtering, so we don't need to convert every time we compare
        public DateTime EndTime { get; set; }
        public int ItemsSold { get; set; }
        public string ItemsMin { get; set; }
        public string TotalValue { get; set; }
        public string user { get; set; }
    }
    /// <summary>
    /// Interaction logic for UsageEmployeePage.xaml
    /// </summary>
    public partial class UsageEmployeePage :Page
    {
        public ObservableCollection<UsageData> data = new ObservableCollection<UsageData>();
        public UsageEmployeePage()
        {
            InitializeComponent();
            StartDatePicker.SelectedDate = DateTime.Now.AddMonths( -1 );
            EndDatePicker.SelectedDate = DateTime.Now.AddDays( 1 );
            PopulateList();

            //Sort the grid by the time of the event
            var column = UsageGrid.Columns[3];
            UsageGrid.Items.SortDescriptions.Clear();
            UsageGrid.Items.SortDescriptions.Add( new SortDescription( column.SortMemberPath, ListSortDirection.Descending ) );
            foreach( var col in UsageGrid.Columns )
                col.SortDirection = null;
            column.SortDirection = ListSortDirection.Ascending;
            UsageGrid.Items.Refresh();
        }

        private void PopulateList()
        {
            data.Clear();
            DBInterface.GetUsageStatistics( StartDatePicker.SelectedDate, EndDatePicker.SelectedDate );

            foreach( XmlNode node in DBInterface.Response )
            {
                DateTime start = DateTime.Parse( node.Get( "LoginTime" ) );
                DateTime stop = DateTime.Parse( node.Get( "LogoutTime" ) );
                TimeSpan duration = stop - start;
                int items_sold = int.Parse( node.Get( "ItemsSold" ) );

                data.Add( new UsageData()
                {
                    Username = node.Get( "Username" ) + " (" + node.Get( "FName" ) + ' ' + node.Get( "LName" ) + ')'
                    , user = node.Get( "Username" )
                    , StartTime = start
                    , Duration = duration.ToString( @"hh\:mm\:ss" )
                    , Duration_TimeSpan = duration
                    , EndTime = stop
                    , ItemsSold = items_sold
                    , ItemsMin = ( items_sold / duration.TotalMinutes ).ToString( "F2" )
                    , TotalValue = decimal.Parse( node.Get( "TotalSales" ) ).ToString( "C2" )
                } );
            }
            LoadItems();
        }

        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
            {
                UsageGrid.ItemsSource = data_cv;
                data_cv.Filter = UsageFilter;
            }
        }

        private bool UsageFilter( object o )
        {
            UsageData data = o as UsageData;

            if( !data.user.Contains( UsernameSearchTextBox.Text ) )
                return false;
            if( DurationTextBox.Text != string.Empty && data.Duration_TimeSpan.TotalMinutes < int.Parse( DurationTextBox.Text ) )
                return false;
            return true;
        }

        private void SearchButton_Click( object sender, RoutedEventArgs e )
        {
            PopulateList();
        }

        private void RefreshButton_Click( object sender, RoutedEventArgs e )
        {
            PopulateList();
        }
    }
}
