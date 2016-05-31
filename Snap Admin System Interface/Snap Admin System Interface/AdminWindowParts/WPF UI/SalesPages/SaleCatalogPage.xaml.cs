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
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.ComponentModel;
using System.Collections.ObjectModel;
using CSharpClient;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for CouponCatalog.xaml
    /// </summary>
    /// 

    public class Sale
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsFlat { get; set; }
        public bool Active { get; set; }

        public void Add( int id )
        {
            _ItemIDList.Add( id );
        }
        public bool AppliesTo( int id )
        {
            return _ItemIDList.Contains( id );
        }
        public List<int> _ItemIDList { get; set; } = new List<int>();
    }

    public partial class SaleCatalog :Page
    {
        public ObservableCollection<Sale> data = new ObservableCollection<Sale>();
        public SaleCatalog()
        {
            InitializeComponent();
            PopulateList();
            DBInterface.GetAllProducts();
            foreach( XmlNode node in DBInterface.Response )
            {
                int i = CheckBoxSP.Children.Add( new CheckBox()
                {
                    Content = node.Get( "Name" ),
                    DataContext = int.Parse( node.Get( "ProductID" ) )
                } );

                ( (CheckBox)CheckBoxSP.Children[i] ).Checked += CheckBox_Toggle;
                ( (CheckBox)CheckBoxSP.Children[i] ).Unchecked += CheckBox_Toggle;
            }

            SaleGrid.SelectedIndex = 0;
        }

        private void CheckBox_Toggle( object sender, RoutedEventArgs e )
        {
            if( _sale != null )
            {
                CheckBox cb = (CheckBox)sender;
                try
                {
                    string item = (string)cb.Content;
                    int itemid = (int)cb.DataContext;
                    bool state = cb.IsChecked == true; //convert a 'bool?' to 'bool'

                    if( state )
                        DBInterface.AddSaleRelation( _sale.Name, item, _sale.ID, itemid );
                    else
                        DBInterface.RemoveSaleRelation( _sale.Name, item, _sale.ID, itemid );

                }
                catch( UnauthorizedAccessException ex ) //if the user doesn't have permission we need to undo the check and display error message
                {
                    System.Windows.Forms.MessageBox.Show( ex.Message );

                    cb.Checked -= CheckBox_Toggle;
                    cb.Unchecked -= CheckBox_Toggle;

                    cb.IsChecked = !cb.IsChecked; //otherwise we infinite loop...

                    cb.Checked += CheckBox_Toggle;
                    cb.Unchecked += CheckBox_Toggle;
                }
            }
        }

        private void PopulateList()
        {
            Dictionary<int, List<int>> rel = new Dictionary<int, List<int>>();

            DBInterface.GetAppliedSales();
            foreach( XmlNode node in DBInterface.Response )
            {
                int couponid = int.Parse( node.Get( "SaleID" ) );
                int itemid = int.Parse( node.Get( "ProductID" ) );

                if( !rel.ContainsKey( couponid ) )
                    rel[couponid] = new List<int>();

                rel[couponid].Add( itemid );
            }

            DBInterface.GetAllSales();
            foreach( XmlNode node in DBInterface.Response )
            {
                int id = int.Parse( node.Get( "SaleID" ) );

                //if the coupon doesn't relate to any items it won't have been added in the previous loop
                if( !rel.ContainsKey( id ) )
                    rel[id] = new List<int>();

                Sale sale = new Sale()
                {
                    ID = id,
                    Name = node.Get( "Name" ),
                    IsFlat = node.Get( "Flat" )[0] == '1',
                    Active = node.Get( "Active" )[0] == '1',
                    Amount = decimal.Parse( node.Get( "Discount" ) ),
                    _ItemIDList = rel[id]
                };
                data.Add( sale );
            }

            LoadItems();
        }
        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
            {
                SaleGrid.ItemsSource = data_cv;
                try
                {
                    data_cv.Filter = CouponFilter; //can't add a filter when they're editing a column, but that's OK.
                }
                catch( Exception ) { }
            }
        }

        private bool CouponFilter( object o )
        {
            Sale sale = (Sale)o;
            return true;
        }

        Sale _sale = null;
        private void CouponGrid_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            try
            {
                Sale sale = (Sale)e.AddedItems[0];
                _sale = sale;

                foreach( var child in CheckBoxSP.Children )
                {
                    CheckBox cb = (CheckBox)child;

                    cb.Checked -= CheckBox_Toggle;
                    cb.Unchecked -= CheckBox_Toggle;

                    cb.IsChecked = sale.AppliesTo( (int)cb.DataContext );

                    cb.Checked += CheckBox_Toggle;
                    cb.Unchecked += CheckBox_Toggle;
                }
            }
            catch { }
        }

        private void AddCouponButton_Click( object sender, RoutedEventArgs e )
        {
            try
            {
                decimal amount;
                bool flat = FlatDiscountCheckBox.IsChecked == true;

                if( flat )
                    amount = FlatDiscountCurrencyBox.Number;
                else
                    amount = decimal.Parse( PercentDiscountDigitBox.Text ) / 100;

                DBInterface.AddSale( SaleNameTextbox.Text, flat, amount );
                RefreshButton_Click( null, null );
            }
            catch( InvalidOperationException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }

        private void FlatDiscountCheckBox_Checked( object sender, RoutedEventArgs e )
        {
            try
            {
                PercentDiscountCheckBox.IsChecked = false;
                PercentDiscountDigitBox.IsEnabled = false;

                FlatDiscountCurrencyBox.IsEnabled = true;
            }
            catch { }
        }

        private void PercentDiscountCheckBox_Checked( object sender, RoutedEventArgs e )
        {
            FlatDiscountCheckBox.IsChecked = false;
            FlatDiscountCurrencyBox.IsEnabled = false;

            PercentDiscountDigitBox.IsEnabled = true;
        }

        private void CouponGrid_PreviewKeyDown( object sender, KeyEventArgs e )
        {
            if( Key.Delete == e.Key )
            {
                try
                {
                    foreach( var row in SaleGrid.SelectedItems )
                    {
                        Sale sale = row as Sale;
                        if( sale.Name != string.Empty )
                        {
                            try
                            {
                                DBInterface.RemoveSale( sale.ID, sale.Name );
                                data.Remove( sale );
                            }
                            catch( UnauthorizedAccessException ex )
                            {
                                System.Windows.Forms.MessageBox.Show( ex.Message );
                                e.Handled = true;
                            }
                        }
                    }
                }
                catch( ArgumentException ex )
                {
                    System.Windows.Forms.MessageBox.Show( ex.Message );
                    e.Handled = true;
                }
                catch { }
            }
        }

        private void RefreshButton_Click( object sender, RoutedEventArgs e )
        {
            data.Clear();
            PopulateList();
        }

        private void CommitButton_Click( object sender, RoutedEventArgs e )
        {
            Sale sale = ( (FrameworkElement)sender ).DataContext as Sale;

            SaleGrid.CommitEdit();
            LoadItems();

            try
            {
                DBInterface.ModifySale( sale.ID, sale.Amount, sale.Name, sale.IsFlat, sale.Active );
            }
            catch( UnauthorizedAccessException ex )
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
            }
        }
    }
}
