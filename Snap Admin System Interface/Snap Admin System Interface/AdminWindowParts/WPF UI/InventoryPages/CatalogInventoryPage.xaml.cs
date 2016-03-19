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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.InventoryPages
{
    /// <summary>
    /// Interaction logic for CatalogInventoryPage.xaml
    /// </summary>
    
    public class Item: INotifyPropertyChanged
    {
        private string productid;
        private string name;
        private decimal price;
        private string barcode;
        private bool active;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged( string propertyName )
        {
            if( PropertyChanged != null )
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public string ProductID
        {
            get { return productid; }
            set { productid = value; OnPropertyChanged( "ProductID" ); }
        }
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged( "Name" ); }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; OnPropertyChanged( "Price" ); }
        }
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; OnPropertyChanged( "Barcode" ); }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; OnPropertyChanged( "Active" ); }
        }

    }

    public partial class CatalogInventoryPage : Page
    {
        public ObservableCollection<Item> data = new ObservableCollection<Item>();
        public CatalogInventoryPage()
        {
            InitializeComponent();

            //populates the response with the list of item nodes
            DBInterface.GetAllProducts();

            //loop though the list adding each row to the list
            foreach( XmlNode node in DBInterface.Response )
            {
                data.Add( new Item() {
                    ProductID = node.Get( "ProductID" )
                    , Name = node.Get( "Name" )
                    , Price = decimal.Parse( node.Get( "Price" ) )
                    , Barcode = node.Get( "Barcode" )
                    //, int.Parse( node.Get( "Stock" ) ) need to add eventually: R
                    , Active = node.Get( "Active_Use" )[0] == '1'
                } );
            }
            Catalog.ItemsSource = data;
        }

        private void Commit( object sender, RoutedEventArgs e )
        {
            Item item = ( (FrameworkElement)sender ).DataContext as Item;

            try
            {
                DBInterface.ModifyItem( int.Parse( item.ProductID ), item.Name, item.Barcode, item.Price, item.Active );
                MessageBox.Show( "Changes saved to database." );
            }
            catch( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

    }
}
