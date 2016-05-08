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
using System.Collections.ObjectModel;
using PointOfSales.Permissions;
using System.Xml;
using CSharpClient;
using System.ComponentModel;

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        RegisterMainWindow m_win;
        public ObservableCollection<Item> data = new ObservableCollection<Item>();
        public CatalogPage()
        {
            InitializeComponent();
        }
        public CatalogPage(RegisterMainWindow win)
        {
            InitializeComponent();
            m_win = win;
            CreateList();
            SearchBox.Focus();
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            m_win.Main_Frame.Navigate(string.Empty);
            m_win.UPCField.Focus();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBox.Clear();
            data.Clear();
            CreateList();
            SearchBox.Focus();
        }
        
        private void CreateList()
        {
            if (m_win.m_employee.HasPermisison(Permissions.ViewItemCatalog))
            {
                m_win.m_connection.Write("GetAllItemsInProducts");

                foreach(XmlNode node in m_win.m_connection.Response)
                {
                    data.Add(new Item()
                    {
                        Barcode = node.Get("Barcode"), Name = node.Get("Name")
                    });
                }
                LoadList();
            }
            else throw new UnauthorizedAccessException(Permissions.ErrorMessage(Permissions.ViewItemCatalog));

        }

        private void LoadList()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView(data);
            if (data_cv != null)
            {
                Catalog.ItemsSource = data_cv;
                try
                {
                    data_cv.Filter = ItemFilter;
                }
                catch (Exception) { }
            }
        }

        private void Search_Button_Click(object sender, RoutedEventArgs e)
        {
            LoadList();
            SearchBox.Clear();
        }
        public bool ItemFilter(object o)
        {
            Item item = o as Item;

            if (item == null)
                return false;

            if (!item.Name.ToLower().Contains(SearchBox.Text.ToLower()))
                return false;
            
            return true;
        }

        private void Row_DoubleClick( object sender, MouseButtonEventArgs e )
        {
            DataGridRow row = sender as DataGridRow;
            Item item = ( sender as DataGridRow ).Item as Item;

            m_win.m_transaction.AddItem(item.Barcode);
        }
    }

    public class Item 
    {
        private string barcode;
        private string name;
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
