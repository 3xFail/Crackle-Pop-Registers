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

    public class Coupon
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

    public partial class CouponCatalog :Page
    {
        public ObservableCollection<Coupon> data = new ObservableCollection<Coupon>();
        public CouponCatalog()
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

            CouponGrid.SelectedIndex = 0;
        }

        private void CheckBox_Toggle( object sender, RoutedEventArgs e )
        {
            if( _coupon != null )
            {
                CheckBox cb = (CheckBox)sender;
                try
                {
                    string item = (string)cb.Content;
                    int itemid = (int)cb.DataContext;
                    bool state = cb.IsChecked == true; //convert a 'bool?' to 'bool'

                    if( state )
                        DBInterface.AddCouponRelation( _coupon.Name, item, _coupon.ID, itemid );
                    else
                        DBInterface.RemoveCouponRelation( _coupon.ID, itemid );

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

            DBInterface.GetAppliedCoupons();
            foreach( XmlNode node in DBInterface.Response )
            {
                int couponid = int.Parse( node.Get( "CouponID" ) );
                int itemid = int.Parse( node.Get( "ProductID" ) );

                if( !rel.ContainsKey( couponid ) )
                    rel[couponid] = new List<int>();

                rel[couponid].Add( itemid );
            }

            DBInterface.GetAllCoupons();
            foreach( XmlNode node in DBInterface.Response )
            {
                Coupon coupon = new Coupon()
                {
                    Name = node.Get( "Name" ),
                    IsFlat = node.Get( "Flat" )[0] == '1',
                    Active = node.Get( "Active" )[0] == '1',
                    Amount = decimal.Parse( node.Get( "Discount" ) ),
                    ID = int.Parse( node.Get( "CouponID" ) ),
                    _ItemIDList = rel[int.Parse( node.Get( "CouponID" ) )]
                };
                data.Add( coupon );
            }

            LoadItems();
        }
        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
            {
                CouponGrid.ItemsSource = data_cv;
                try
                {
                    data_cv.Filter = CouponFilter; //can't add a filter when they're editing a column, but that's OK.
                }
                catch( Exception ) { }
            }
        }

        private bool CouponFilter( object o )
        {
            Coupon coupon = (Coupon)o;
            return true;
        }

        Coupon _coupon = null;
        private void CouponGrid_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            try
            {
                Coupon coupon = (Coupon)e.AddedItems[0];
                _coupon = coupon;

                foreach( var child in CheckBoxSP.Children )
                {
                    CheckBox cb = (CheckBox)child;

                    cb.Checked -= CheckBox_Toggle;
                    cb.Unchecked -= CheckBox_Toggle;

                    cb.IsChecked = coupon.AppliesTo( (int)cb.DataContext );

                    cb.Checked += CheckBox_Toggle;
                    cb.Unchecked += CheckBox_Toggle;
                }
            }
            catch { }
        }
    }
}
