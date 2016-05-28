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
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public bool IsFlat { get; set; }
        public bool Active { get; set; }
    }

    public partial class SaleCatalog :Page
    {
        public ObservableCollection<Coupon> data = new ObservableCollection<Coupon>();
        public SaleCatalog()
        {
            InitializeComponent();
            PopulateList();
        }

        private void PopulateList()
        {
            DBInterface.GetAllSales();

            foreach( XmlNode node in DBInterface.Response )
            {
                data.Add( new Coupon()
                {
                    Name = node.Get( "Name" ),
                    IsFlat = node.Get( "Flat" )[0] == '1',
                    Active = node.Get( "Active" )[0] == '1',
                    Amount = decimal.Parse( node.Get( "Discount" ) )
                } );
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
            Coupon coupon = (Coupon)o;
            return true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
