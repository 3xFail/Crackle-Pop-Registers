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
    
    public class Item: INotifyPropertyChanged, ICloneable
    {
        private int productid;
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

        public object Clone()
        {
            return new Item()
            {
                ProductID = this.ProductID
                , Name = this.Name
                , Price = this.Price
                , Barcode = this.Barcode
                , Active = this.Active
            };
        }

        public int ProductID
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

        public bool Changed { get; set; } = false;
        public bool WasChanged { get; set; } = false;

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
                    ProductID = int.Parse( node.Get( "ProductID" ) )
                    , Name = node.Get( "Name" )
                    , Price = decimal.Parse( node.Get( "Price" ) )
                    , Barcode = node.Get( "Barcode" )
                    //, int.Parse( node.Get( "Stock" ) ) need to add eventually: R
                    , Active = node.Get( "Active" )[0] == '1'
                } );
            }
            LoadItems();
        }

        public bool ItemFilter( object o )
        {
            Item item = o as Item;

            if( item == null )
                return false;

            if( !item.Name.ToLower().Contains( NameSearchBox.Text.ToLower() ) )
                return false;
            if( !item.Barcode.Contains( BarcodeSearchBox.Text ) )
                return false;
            if( ActiveComboBox.SelectedIndex == 1 && !item.Active ) //Index 1 = Show active only
                return false;
            if( ActiveComboBox.SelectedIndex == 2 && item.Active ) //Index 2 = Show inactive only
                return false; 

            return true;
        }

        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
            {
                Catalog.ItemsSource = data_cv;
                try
                {
                    data_cv.Filter = ItemFilter; //can't add a filter when they're editing a column, but that's OK.
                }
                catch( Exception ) { }
            }
        }


        private void Commit( object sender, RoutedEventArgs e )
        {
            
            Item item = ( (FrameworkElement)sender ).DataContext as Item;
            item.Changed = false;
            item.WasChanged = true;

            LoadItems();

            try
            {
                DBInterface.ModifyItem( item.ProductID, item.Name, item.Barcode, item.Price, item.Active );
                MessageBox.Show( "Changes saved to database." );
            }
            catch( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        Item _EditItem;

        private void Catalog_BeginningEdit( object sender, DataGridBeginningEditEventArgs e )
        {
            Item item = e.Row.Item as Item;

            _EditItem = new Item();
            _EditItem.Name = item.Name;
            _EditItem.Barcode = item.Barcode;
            _EditItem.Active = item.Active;
            _EditItem.Price = item.Price;
        }

        private void Catalog_CellEditEnding( object sender, DataGridCellEditEndingEventArgs e )
        {
            Item item = e.Row.Item as Item;
            if( item.Price != _EditItem.Price || item.Name != _EditItem.Name || item.Barcode != _EditItem.Barcode || item.Active != _EditItem.Active )
            {
                e.Row.Background = new SolidColorBrush( Color.FromArgb( 128, 255, 0, 0 ) );
                item.Changed = true;
            }
        }

        private void Catalog_LoadingRow( object sender, DataGridRowEventArgs e )
        {
            Item item = e.Row.Item as Item;
            if( item.Changed )
                e.Row.Background = new SolidColorBrush( Color.FromArgb( 128, 255, 0, 0 ) );
            else if( item.WasChanged )
                e.Row.Background = new SolidColorBrush( Color.FromArgb( 128, 0, 255, 0 ) );
            else
                e.Row.Background = new SolidColorBrush( Color.FromArgb( 128, 200, 200, 200 ) );
        }

        private void SearchBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            LoadItems();
        }

        private void ActiveCheckBox_Checked( object sender, RoutedEventArgs e )
        {
            LoadItems();
        }

        private void ActiveCheckBox_Unchecked( object sender, RoutedEventArgs e )
        {
            LoadItems();
        }

        private void comboBox_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            LoadItems();
        }
    }
}
