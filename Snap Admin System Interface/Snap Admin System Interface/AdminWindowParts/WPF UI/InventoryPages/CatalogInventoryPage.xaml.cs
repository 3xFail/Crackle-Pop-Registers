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
        
        public CatalogInventoryPage()
        {
            InitializeComponent();

            //creating a table based on the passed values
            DataTable Table = new DataTable("ItemsCatalog");
            Table.Columns.Add("ProductID", typeof(Int32));
            Table.Columns.Add("Name", typeof(string));
            Table.Columns.Add("Price", typeof(double));
            Table.Columns.Add("Barcode", typeof(string));
            Table.Columns.Add("Active", typeof(bool));



            //populates the response with the list of item nodes
            DBInterface.GetAllProducts();
            XmlNodeList List = DBInterface.Response;
            //loop though the list Assigning values to each column
            foreach(XmlNode Node in List)
            {
                Table.Rows.Add(new Object[] { 0, Int32.Parse(Node.Get("ProductID")) });
                Table.Rows.Add(new Object[] { 1, Node.Get("Name") });
                Table.Rows.Add(new Object[] { 2, double.Parse(Node.Get("Price")) });
                Table.Rows.Add(new Object[] { 3, Node.Get("Barcode") });
                Table.Rows.Add(new Object[] { 4, Node.Get("Active") });

            }
            //assigning the data from the table to displayed in the grid view
            Catalog.ItemsSource = Table.DefaultView;
        }

        

        
    }
}
