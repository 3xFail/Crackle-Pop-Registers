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
using Snap_Admin_System_Interface.AdminWindowParts;
using SnapRegisters;
using System.Xml;
using CSharpClient;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for CatalogInventoryPage.xaml
    /// </summary>
    public partial class CatalogInventoryPage : Page
    {
        
        enum Row
        {
            ProductID,
            Name,
            Price,
            Barcode,
            Active
        }

        public CatalogInventoryPage()
        {
            InitializeComponent();

            //creating a table based on the passed values
            DataTable Table = new DataTable("ItemsCatalog");
            Table.Columns.Add( "ProductID", typeof( int ) );
            Table.Columns.Add( "Name", typeof( string ) );
            Table.Columns.Add( "Price", typeof( double ) );
            Table.Columns.Add( "Barcode", typeof( string ) );
            Table.Columns.Add( "Active", typeof( bool ) );

            //populates the response with the list of item nodes
            DBInterface.GetAllProducts();

            //loop though the list adding each row to the list
            foreach( XmlNode node in DBInterface.Response )
            {
                var row = Table.Rows.Add( new Object[] {
                                            int.Parse( node.Get( "ProductID" ) )
                                            , node.Get( "Name" )
                                            , double.Parse( node.Get( "Price" ) )
                                            , node.Get( "Barcode" )
                                            , node.Get( "Active" ) != "1"
                } );
            }
            //assigning the data from the table to displayed in the grid view
            Catalog.ItemsSource = Table.DefaultView;
        }

        private void Commit( object sender, RoutedEventArgs e )
        {
            //Todo: Send all changes to the item in the line that the button belonged to, to the datebase
        }

        

        
    }
}
