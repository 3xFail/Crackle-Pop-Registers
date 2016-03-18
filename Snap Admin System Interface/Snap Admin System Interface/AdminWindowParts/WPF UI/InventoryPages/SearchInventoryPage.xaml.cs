using CSharpClient;
using SnapRegisters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Xml;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for SearchInventoryPage.xaml
    /// </summary>
    public partial class SearchInventoryPage :Page
    {
        public SearchInventoryPage()
        {
            InitializeComponent();

            m_table = new DataTable( "SearchCatalog" );
            m_table.Columns.Add( "ProductID", typeof( int ) );
            m_table.Columns.Add( "Name", typeof( string ) );
            m_table.Columns.Add( "Price", typeof( double ) );
            m_table.Columns.Add( "Barcode", typeof( string ) );
            //Table.Columns.Add( "Stock", typeof(Int32) );
            m_table.Columns.Add( "Active", typeof( bool ) );

            //populates the response with the list of item nodes
            DBInterface.GetAllProducts();

            //loop though the list adding each row to the list
            foreach( XmlNode node in DBInterface.Response )
            {
                var row = m_table.Rows.Add( new Object[] {
                                            int.Parse( node.Get( "ProductID" ) )
                                            , node.Get( "Name" )
                                            , double.Parse( node.Get( "Price" ) )
                                            , node.Get( "Barcode" )
                                            //, int.Parse( node.Get( "Stock" ) ) need to add eventually: R
                                            , node.Get( "Active" ) != "1"
                } );
            }
            SearchCatalog.ItemsSource = m_table.DefaultView;
        }

        private void Commit( object sender, RoutedEventArgs e )
        {
            //Todo: Send all changes to the item in the line that the button belonged to, to the datebase

            //does this associate with the individual row or with the whole file?: R
        }

        private void Search_TextChanged( object sender, TextChangedEventArgs e )
        {
            //m_table.DefaultView.RowFilter = string.Format( "Name LIKE '%{0}%'", SearchBox.Text );
        }

        private DataTable m_table = new DataTable();
    }
}
