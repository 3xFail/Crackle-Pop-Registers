using CSharpClient;
using SnapRegisters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI.SalesPages
{
    /// <summary>
    /// Interaction logic for OrdersCatalogPage.xaml
    /// </summary>
    public partial class OrdersCatalogPage : Page
    {
        public ObservableCollection<Item> data = new ObservableCollection<Item>();
        public OrdersCatalogPage()
        {
            InitializeComponent();
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            DBInterface.GetOrdersCatalog();

            //loop though the list adding each row to the list
            foreach (XmlNode node in DBInterface.Response)
            {
  
                data.Add(new Item()
                {
                    order_id = node.Get("OrderID")
                    ,
                    order_date = node.Get("OrderTime")
                    ,
                    cashier_name = node.Get("Username")
                    ,
                    customer_name = node.Get("LName")
                    ,
                    product_name = node.Get("Name")
                    ,
                    final_price = decimal.Parse( node.Get("FinalPrice") ).ToString( "C" )
                });
            }
            LoadItems();
        }

        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView(data);
            if (data_cv != null)
            {
                Catalog.ItemsSource = data_cv;
                try
                {
                    data_cv.Filter = ItemFilter; //can't add a filter when they're editing a column, but that's OK.
                }
                catch (Exception) { }
            }
        }

        public bool ItemFilter(object o)
        {
            Item item = o as Item;

            if (item == null)
                return false;   

            return true;
        }
    }

    //Order ID
    //Cashier Name, Emp table join
    //Customer Name, Cust table join
    //Order date
    //Product name, Products table from orderitems
    //Final Price, Orderitems

    public class Item : INotifyPropertyChanged
    {
        public string order_id { get; set; }
        public string cashier_name { get; set; }

        public string customer_name { get; set; }

        public string order_date { get; set; }

        public string product_name { get; set; }

        public string final_price { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Changed { get; set; } = false;
        public bool WasChanged { get; set; } = false;
    }
}
