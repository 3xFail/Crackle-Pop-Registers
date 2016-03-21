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
        private int productid;
        private string name;
        private decimal price;
        private int quantity;
        private string barcode;
        private bool active;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged( string propertyName )
        {
            if( PropertyChanged != null )
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
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
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; OnPropertyChanged( "Quantity" ); }
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
            PopulateList();
        }

        private void PopulateList()
        {
            //populates the response with the list of item nodes
            DBInterface.GetAllProducts();

            decimal maxprice = 0;
            int maxquantity = 0;

            //loop though the list adding each row to the list
            foreach( XmlNode node in DBInterface.Response )
            {
                decimal price = decimal.Parse( node.Get( "Price" ) );
                int quantity = int.Parse( node.Get( "Quantity" ) );

                maxprice = Math.Max( price, maxprice );
                maxquantity = Math.Max( quantity, maxquantity );

                data.Add( new Item()
                {
                    ProductID = int.Parse( node.Get( "ProductID" ) )
                    , Name = node.Get( "Name" )
                    , Price = price
                    , Barcode = node.Get( "Barcode" )
                    , Quantity = quantity
                    , Active = node.Get( "Active" )[0] == '1'
                } );
            }
            LoadItems();


            MaxPriceSlider.Maximum = (int)maxprice + 1;
            MinPriceSlider.Maximum = (int)maxprice + 1;
            MaxPriceTextBox.Text = MinPriceTextBox.Text = "$0.00";

            MaxQuantitySlider.Maximum = maxquantity + 1;
            MinQuantitySlider.Maximum = maxquantity + 1;
            MaxQuantityTextBox.Text = MinQuantityTextBox.Text = "0";
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
            if( item.Price < (decimal)MinPriceSlider.Value ) //If price is greater than the minimum
                return false;
            if( MaxPriceSlider.Value > 0.01D && item.Price > (decimal)MaxPriceSlider.Value ) //If price is less than the max
                return false;
            if( item.Quantity < MinQuantitySlider.Value )
                return false;
            if( (int)MaxQuantitySlider.Value != 0 && item.Quantity > MaxQuantitySlider.Value )
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

        private void Commit_ButtonClick( object sender, RoutedEventArgs e )
        {
            
            Item item = ( ( FrameworkElement )sender ).DataContext as Item;
            item.Changed = false;
            item.WasChanged = true;

            LoadItems();

            try
            {
                DBInterface.ModifyItem( item.ProductID, item.Name, item.Barcode, item.Price, item.Active, item.Quantity );
            }
            catch( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message );
            }
        }

        Item _EditItem = new Item();

        private void Catalog_BeginningEdit( object sender, DataGridBeginningEditEventArgs e )
        {
            Item item = e.Row.Item as Item;

            _EditItem.Name = item.Name;
            _EditItem.Barcode = item.Barcode;
            _EditItem.Price = item.Price;
            _EditItem.Active = item.Active;
            _EditItem.Quantity = item.Quantity;
        }

        private void Catalog_CellEditEnding( object sender, DataGridCellEditEndingEventArgs e )
        {
            Item item = e.Row.Item as Item; //Known bug: Can't get cell to highlight when price changes since it doesn't count as a cell edit event.
            if( item.Price != _EditItem.Price || item.Name != _EditItem.Name || item.Barcode != _EditItem.Barcode || item.Active != _EditItem.Active || item.Quantity != _EditItem.Quantity )
            {
                e.Row.Background = new SolidColorBrush( Color.FromArgb( 128, 255, 0, 0 ) );
                item.Changed = true;


                //Update Maximums if the item changed changed the new max for either price or quantity
                int newmax = (int)Math.Max( MaxPriceSlider.Maximum, (double)item.Price ) + 1;

                MaxPriceSlider.Maximum = MinPriceSlider.Maximum = newmax;
                MaxPriceSlider.Value = Math.Min( MaxPriceSlider.Value, MaxPriceSlider.Maximum );
                MinPriceSlider.Value = Math.Min( MinPriceSlider.Value, MinPriceSlider.Maximum );

                MaxPriceTextBox.Number = (decimal)MaxPriceSlider.Value;
                MinPriceTextBox.Number = (decimal)MinPriceSlider.Value;

                newmax = (int)Math.Max( MaxQuantitySlider.Maximum, item.Quantity ) + 1;
                MaxQuantitySlider.Maximum = MinQuantitySlider.Maximum = newmax;
                MaxQuantitySlider.Value = Math.Min( MaxQuantitySlider.Value, MaxQuantitySlider.Maximum );
                MinQuantitySlider.Value = Math.Min( MinQuantitySlider.Value, MinQuantitySlider.Maximum );

                MaxQuantityTextBox.Text = ( (int)MaxQuantitySlider.Value ).ToString();
                MinQuantityTextBox.Text = ( (int)MinQuantitySlider.Value ).ToString();
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

        private void Remove_ButtonClick( object sender, RoutedEventArgs e )
        {
            Item item = ( ( FrameworkElement )sender ).DataContext as Item;
            try
            {
                DBInterface.RemoveItem( item.ProductID );
                data.Remove( item );
            }
            catch( Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void SearchButton_Click( object sender, RoutedEventArgs e )
        {
            LoadItems();
        }

        private void MinPriceSlider_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            MinPriceTextBox.Number = (decimal)e.NewValue;
            MaxPriceSlider.Value = Math.Max( e.NewValue, MaxPriceSlider.Value );
        }

        private void MaxPriceSlider_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            MaxPriceTextBox.Number = (decimal)e.NewValue;
            MinPriceSlider.Value = Math.Min( e.NewValue, MinPriceSlider.Value );
        }

        private void MinPriceTextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            MinPriceSlider.Value = (double)MinPriceTextBox.Number;
        }

        private void MaxPriceTextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            MaxPriceSlider.Value = (double)MaxPriceTextBox.Number;
        }
        private void MinQuantitySlider_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            MinQuantityTextBox.Text = ((int)e.NewValue).ToString();
            MaxQuantitySlider.Value = Math.Max( e.NewValue, MaxQuantitySlider.Value );
        }

        private void MaxQuantitySlider_ValueChanged( object sender, RoutedPropertyChangedEventArgs<double> e )
        {
            MaxQuantityTextBox.Text = ((int)e.NewValue).ToString();
            MinQuantitySlider.Value = Math.Min( e.NewValue, MinQuantitySlider.Value );
        }

        private void MinQuantityTextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            MinQuantitySlider.Value = int.Parse( MinQuantityTextBox.Text );
        }

        private void MaxQuantityTextBox_TextChanged( object sender, TextChangedEventArgs e )
        {
            MaxQuantitySlider.Value = int.Parse( MaxQuantityTextBox.Text );
        }

        private void AddItemButton_Click( object sender, RoutedEventArgs e )
        {
            int quantity;

            if( BarcodeAddBox.Text == string.Empty )
                BarcodeAddBox.Focus();
            else if( NameAddBox.Text == string.Empty )
                NameAddBox.Focus();
            else if( !int.TryParse( QuantityAddTextBox.Text, out quantity ) || quantity < 0 )
                QuantityAddTextBox.Focus();
            else
            {
                try
                {
                    DBInterface.AddItem( NameAddBox.Text, PriceAddBox.Number, BarcodeAddBox.Text, 1 );
                    MessageBox.Show( "\"" + NameAddBox.Text + "\" has been added!" );

                    //Get item info ID from database
                    DBInterface.GetItemID( BarcodeAddBox.Text );

                    //Add item to list
                    XmlNode node = DBInterface.Response[0];
                    data.Add( new Item()
                    {
                        ProductID = int.Parse( node.Get( "ProductID" ) )
                        , Name = NameAddBox.Text
                        , Price = PriceAddBox.Number
                        , Barcode = BarcodeAddBox.Text
                        , Quantity = quantity
                        , Active = true
                    } );
                    LoadItems();

                    //Clear entry fields
                    BarcodeAddBox.Clear();
                    NameAddBox.Clear();
                    PriceAddBox.Number = 0M;
                }
                catch( Exception ex )
                {
                    MessageBox.Show( ex.Message );
                }
            }
        }

        private void RefreshButton_Click( object sender, RoutedEventArgs e )
        {
            /*
            BarcodeSearchBox.Clear();
            NameSearchBox.Clear();
            ActiveComboBox.SelectedIndex = -1;

            MaxPriceSlider.Value = 0;
            MinPriceSlider.Value = 0;

            MaxQuantitySlider.Value = 0;
            MinQuantitySlider.Value = 0;
            */

            data.Clear();
            PopulateList();
        }
    }
}
