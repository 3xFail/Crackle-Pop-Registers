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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        // list of all US states
        private static String states = "|AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY|";
        private static String permissions = "|Cashier|Manager|Owner|";

        public AddEmployeePage()
        {
            InitializeComponent();
        }

        // changing date to selected date
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // get DatePicker reference
            var picker = sender as DatePicker;

            // get nullable DateTime from SelectedDate
            DateTime? date = picker.SelectedDate;
            if (date == null)
            {
                // a null object
                this.Title = "No date";
            }
            else
            {
                // no need to display the time
                this.Title = date.Value.ToShortDateString();
                //textBoxMM = Day.
            }
        }
        // load the list of items in combobox
        private void State_Loaded(object sender, RoutedEventArgs e)
        {
            // gGet the ComboBox reference
            var comboBox = sender as ComboBox;

            // assign the ItemsSource to the List
            comboBox.ItemsSource = states.Split('|');

            // make the first item selected
            comboBox.SelectedIndex = 0;
        }

        private void State_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the ComboBox
            var comboBox = sender as ComboBox;

            // set SelectedItem as Window Title
            string value = comboBox.SelectedItem as string;
            this.Title = value;
        }
        // load the list of items in combobox
        private void Authorization_Loaded(object sender, RoutedEventArgs e)
        {
            // gGet the ComboBox reference
            var comboBox = sender as ComboBox;

            // assign the ItemsSource to the List
            comboBox.ItemsSource = permissions.Split('|');

            // make the first item selected
            comboBox.SelectedIndex = 0;
        }

        private void Authorization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the ComboBox
            var comboBox = sender as ComboBox;

            // set SelectedItem as Window Title
            string value = comboBox.SelectedItem as string;
            this.Title = value;
        }

        // determines which US state to use
        public static bool isStateAbbreviation(String state)
        {
            return state.Length == 2 && states.IndexOf(state) > 0;
        }



    }
}
