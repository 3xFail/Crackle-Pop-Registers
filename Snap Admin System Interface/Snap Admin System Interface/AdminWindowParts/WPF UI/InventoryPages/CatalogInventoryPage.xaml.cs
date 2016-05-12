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
using PointOfSales.Permissions;
using System.Windows.Threading;
using Scale;

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
        private decimal weight;
        private bool weighable;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged( string propertyName )
        {
            if( PropertyChanged != null )
                PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
        }

        public decimal Weight
        {
            get { return weight; }
            set { weight = value; OnPropertyChanged( "Weight" ); }
        }

        public bool Weighable
        {
            get { return weighable; }
            set { weighable = value;  OnPropertyChanged( "PricePerWeight" ); }
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

            Scale.Scale theScale = new Scale.Scale();

            //Update the weight constantly
            DispatcherTimer weightUpdateTimer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, 100),
                                DispatcherPriority.Normal,
                                delegate
                                {
                                    string theWeight = theScale.GetWeightAsString();
                                    if (theWeight == "null" || theWeight == "neg" )
                                        this.ItemWeight.Text = theWeight;
                                    else
                                        this.ItemWeight.Text = Math.Round(Convert.ToDouble(theScale.GetWeightAsDecimal()), 2).ToString();
                                },
                                this.Dispatcher);
        }

        private void PopulateList()
        {
            //populates the response with the list of item nodes
            DBInterface.GetAllProducts();

            decimal maxprice = 0;
            int maxquantity = int.MinValue;
            int minquantity = int.MaxValue;

            //loop though the list adding each row to the list
            foreach( XmlNode node in DBInterface.Response )
            {
                decimal price = decimal.Parse( node.Get( "Price" ) );
                int quantity = int.Parse( node.Get( "Quantity" ) );

                maxprice = Math.Max( price, maxprice );
                maxquantity = Math.Max( quantity, maxquantity );
                minquantity = Math.Min( quantity, minquantity );

                data.Add( new Item()
                {
                    ProductID = int.Parse( node.Get( "ProductID" ) )
                    , Name = node.Get( "Name" )
                    , Price = price
                    , Barcode = node.Get( "Barcode" )
                    , Quantity = quantity
                    , Active = node.Get( "Active" )[0] == '1'
                    , Weight = decimal.Parse( node.Get( "Weight") )
                    , Weighable = node.Get( "Weighable" )[0] == '1'
                } );
            }

            //Price sliders
            MaxPriceSlider.Maximum = (double)maxprice;
            MinPriceSlider.Maximum = (double)maxprice;

            MaxPriceSlider.Value = (double)maxprice;
            MinPriceSlider.Value = 0;
            ////////////////////////////////////

            //Quantity sliders
            MaxQuantitySlider.Maximum = maxquantity;
            MinQuantitySlider.Maximum = maxquantity;

            MinQuantitySlider.Minimum = minquantity;
            MaxQuantitySlider.Minimum = minquantity;

            MaxQuantitySlider.Value = maxquantity;
            MinQuantitySlider.Value = minquantity;
            ////////////////////////////////////

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
            if( item.Price < (decimal)MinPriceSlider.Value ) //If price is greater than the minimum
                return false;
            if( item.Price > (decimal)MaxPriceSlider.Value ) //If price is less than the max
                return false;
            if( item.Quantity < MinQuantitySlider.Value )
                return false;
            if( item.Quantity > MaxQuantitySlider.Value )
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

            Catalog.CommitEdit();
            LoadItems();

            try
            {
                DBInterface.ModifyItem( item.ProductID, item.Name, item.Barcode, item.Price, item.Active, item.Quantity, item.Weight, item.Weighable );
            }
            catch( InvalidOperationException ex )
            {
                MessageBox.Show( ex.Message );
            }

            UpdateSliders();
        }

       private void UpdateSliders()
        {
            int newmaxquantity = int.MinValue;
            int newminquantity = int.MaxValue;
            double newmaxprice = double.MinValue;

            foreach( Item _item in data )
            {
                newmaxquantity = Math.Max( _item.Quantity, newmaxquantity );
                newminquantity = Math.Min( _item.Quantity, newminquantity );
                newmaxprice = Math.Max( (double)_item.Price, newmaxprice );
            }

            MaxPriceSlider.Maximum = MinPriceSlider.Maximum = newmaxprice;
            MaxQuantitySlider.Maximum = MinQuantitySlider.Maximum = newmaxquantity;
            MaxQuantitySlider.Minimum = MinQuantitySlider.Minimum = newminquantity;
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
            _EditItem.Weighable = item.Weighable;
            _EditItem.Weight = item.Weight;
        }

        private void Catalog_CellEditEnding( object sender, DataGridCellEditEndingEventArgs e )
        {
            Item item = e.Row.Item as Item; //Known bug: Can't get cell to highlight when price changes since it doesn't count as a cell edit event.
            if( item.Price != _EditItem.Price || item.Name != _EditItem.Name || item.Barcode != _EditItem.Barcode || item.Active != _EditItem.Active || item.Quantity != _EditItem.Quantity || item.Weight != _EditItem.Weight || item.Weighable != _EditItem.Weighable )
            {
                e.Row.Background = new SolidColorBrush( Color.FromArgb( 128, 255, 0, 0 ) );
                item.Changed = true;
                UpdateSliders();
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
                DBInterface.RemoveItem( item.ProductID, item.Name );
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
            decimal weightOfItem = 0;

            decimal.TryParse(ItemWeight.Text, out weightOfItem);

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
                    DBInterface.AddItem( NameAddBox.Text, PriceAddBox.Number, BarcodeAddBox.Text, quantity, weightOfItem, PriceIsPerLb.IsChecked == true );
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
                        , Weight = weightOfItem
                        , Weighable = PriceIsPerLb.IsChecked == true
                    } );
                    LoadItems();

                    //Clear entry fields
                    BarcodeAddBox.Clear();
                    NameAddBox.Clear();
                    PriceAddBox.Number = 0M;
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch( Exception ex )
                {
                    MessageBox.Show( ex.Message );
                }
            }
        }

        private void RefreshButton_Click( object sender, RoutedEventArgs e )
        {
            data.Clear();
            PopulateList();
        }

        private void PriceIsPerLb_Checked(object sender, RoutedEventArgs e)
        {
            WeightLabel.Visibility = Visibility.Hidden;
            LbLabel.Visibility = Visibility.Hidden;
            ItemWeight.Visibility = Visibility.Hidden;
        }

        private void PriceIsPerLb_Unchecked(object sender, RoutedEventArgs e)
        {
            WeightLabel.Visibility = Visibility.Visible;
            LbLabel.Visibility = Visibility.Visible;
            ItemWeight.Visibility = Visibility.Visible;
        }
    }
}
