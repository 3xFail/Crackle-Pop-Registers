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

namespace Snap_Admin_System_Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Inv(object sender, RoutedEventArgs e)
        {
           // TabControl tab_inv = new TabControl();
            //int newIndex = Tabs.SelectedIndex + 1;
            //if (newIndex >= Tabs.Items.Count)
            //    newIndex = 0;
            //Tabs.SelectedIndex = newIndex;

            //MessageBox.Show("Selected tab: " + (Tabs.SelectedItem as TabItem).Header);
        }
        private void btn_Anal(object sender, RoutedEventArgs e)
        {
            //int newIndex = Tabs.SelectedIndex + 1;
            //if (newIndex >= Tabs.Items.Count)
            //    newIndex = 0;
            //Tabs.SelectedIndex = newIndex;

            MessageBox.Show("Selected tab: " + (Tabs.SelectedItem as TabItem).Header);
        }
        private void btn_Emp(object sender, RoutedEventArgs e)
        {
            //int newIndex = Tabs.SelectedIndex + 1;
            //if (newIndex >= Tabs.Items.Count)
            //    newIndex = 0;
            //Tabs.SelectedIndex = newIndex;

            MessageBox.Show("Selected tab: " + (Tabs.SelectedItem as TabItem).Header);
        }
        private void btn_Cust(object sender, RoutedEventArgs e)
        {
            //int newIndex = Tabs.SelectedIndex + 1;
            //if (newIndex >= Tabs.Items.Count)
            //    newIndex = 0;
            //Tabs.SelectedIndex = newIndex;

            MessageBox.Show("Selected tab: " + (Tabs.SelectedItem as TabItem).Header);
        }
        private void btn_Sales(object sender, RoutedEventArgs e)
        {
            //int newIndex = Tabs.SelectedIndex + 1;
            //if (newIndex >= Tabs.Items.Count)
            //    newIndex = 0;
            //Tabs.SelectedIndex = newIndex;

            MessageBox.Show("Selected tab: " + (Tabs.SelectedItem as TabItem).Header);
        }
        private void btn_Opt(object sender, RoutedEventArgs e)
        {
            //int newIndex = Tabs.SelectedIndex + 1;
            //if (newIndex >= Tabs.Items.Count)
            //    newIndex = 0;
            //Tabs.SelectedIndex = newIndex;

            MessageBox.Show("Selected tab: " + (Tabs.SelectedItem as TabItem).Header);
        }

    }
}
