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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for AddCustomerPage.xaml
    /// </summary>
    public partial class AddCustomerPage : Page
    {
        // all US states
        private static String states = "AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY|";
        // all the countries
        private static String countries = "Afghanistan|Åland Islands|Albania|Algeria|American Samoa|Andorra|Angola|Anguilla|Antigua and Barbuda|Argentina|Armenia|Aruba|Australia|Austria|Azerbaijan|Bangladesh|Barbados|Bahamas|Bahrain|Belarus|Belgium|Belize|Benin|Bermuda|Bhutan|Bolivia|Bosnia and Herzegovina|Botswana|Brazil|British Indian Ocean Territory|British Virgin Islands|Brunei Darussalam|Bulgaria|Burkina Faso|Burma|Burundi|Cambodia|Cameroon|Canada|Cape Verde|Cayman Islands|Central African Republic|Chad|Chile|China|Christmas Island|Cocos (Keeling) Islands|Colombia|Comoros|Congo-Brazzaville|Congo-Kinshasa|Cook Islands|Costa Rica|$_[|Croatia|Curaçao|Cyprus|Czech Republic|Denmark|Djibouti|Dominica|Dominican Republic|East Timor|Ecuador|El Salvador|Egypt|Equatorial Guinea|Eritrea|Estonia|Ethiopia|Falkland Islands|Faroe Islands|Federated States of Micronesia|Fiji|Finland|France|French Guiana|French Polynesia|French Southern Lands|Gabon|Gambia|Georgia|Germany|Ghana|Gibraltar|Greece|Greenland|Grenada|Guadeloupe|Guam|Guatemala|Guernsey|Guinea|Guinea-Bissau|Guyana|Haiti|Heard and McDonald Islands|Honduras|Hong Kong|Hungary|Iceland|India|Indonesia|Iraq|Ireland|Isle of Man|Israel|Italy|Jamaica|Japan|Jersey|Jordan|Kazakhstan|Kenya|Kiribati|Kuwait|Kyrgyzstan|Laos|Latvia|Lebanon|Lesotho|Liberia|Libya|Liechtenstein|Lithuania|Luxembourg|Macau|Macedonia|Madagascar|Malawi|Malaysia|Maldives|Mali|Malta|Marshall Islands|Martinique|Mauritania|Mauritius|Mayotte|Mexico|Moldova|Monaco|Mongolia|Montenegro|Montserrat|Morocco|Mozambique|Namibia|Nauru|Nepal|Netherlands|New Caledonia|New Zealand|Nicaragua|Niger|Nigeria|Niue|Norfolk Island|Northern Mariana Islands|Norway|Oman|Pakistan|Palau|Panama|Papua New Guinea|Paraguay|Peru|Philippines|Pitcairn Islands|Poland|Portugal|Puerto Rico|Qatar|Réunion|Romania|Russia|Rwanda|Saint Barthélemy|Saint Helena|Saint Kitts and Nevis|Saint Lucia|Saint Martin|Saint Pierre and Miquelon|Saint Vincent|Samoa|San Marino|São Tomé and Príncipe|Saudi Arabia|Senegal|Serbia|Seychelles|Sierra Leone|Singapore|Sint Maarten|Slovakia|Slovenia|Solomon Islands|Somalia|South Africa|South Georgia|South Korea|Spain|Sri Lanka|Sudan|Suriname|Svalbard and Jan Mayen|Sweden|Swaziland|Switzerland|Syria|Taiwan|Tajikistan|Tanzania|Thailand|Togo|Tokelau|Tonga|Trinidad and Tobago|Tunisia|Turkey|Turkmenistan|Turks and Caicos Islands|Tuvalu|Uganda|Ukraine|United Arab Emirates|United Kingdom|United States|Uruguay|Uzbekistan|Vanuatu|Vatican City|Vietnam|Venezuela|Wallis and Futuna|Western Sahara|Yemen|Zambia|Zimbabwe|";
        // the three permission levels

        public AddCustomerPage()
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
            }
        }
        // load the states items in combobox
        private void State_Loaded(object sender, RoutedEventArgs e)
        {
            // gGet the combobox reference
            var comboBox = sender as ComboBox;

            // assign the ItemsSource to the states
            comboBox.ItemsSource = states.Split('|');

            // make no item selected
            comboBox.SelectedIndex = -1;
        }
        // select the state
        private void State_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the combobox
            var comboBox = sender as ComboBox;

            // set SelectedItem as Window Title
            string value = comboBox.SelectedItem as string;
            this.Title = value;
        }
        // load the list of items in combobox
        private void Country_Loaded(object sender, RoutedEventArgs e)
        {
            // get the combobox reference
            var comboBox = sender as ComboBox;

            // assign the ItemsSource to the countries
            comboBox.ItemsSource = countries.Split('|');

            // make no item selected
            comboBox.SelectedIndex = -1;
        }
        // select the country
        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get the combobox
            var comboBox = sender as ComboBox;

            // set SelectedItem as Window Title
            string value = comboBox.SelectedItem as string;
            this.Title = value;
        }
        // determines the US state
        public static bool isStateAbbreviation(String state)
        {
            return state.Length == 2 && states.IndexOf(state) > 0;
        }
        // submits the input values into the database
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if( textBoxFirstName.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "First name is required. Please enter it." );
            else if( textBoxPhone.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "Phone number is required. Please enter it." );
            else
            {
                try
                {
                    DBInterface.AddCust( textBoxFirstName.Text, textBoxLastName.Text, textBoxAddress1.Text, textBoxAddress2.Text, textBoxCity.Text, textBoxState.Text, textBoxCountry.Text, textBoxZip.Text, textBoxPhone.Text, textBoxEmail.Text, DOB.DisplayDate );
                }
                catch( Exception ex )
                {
                    MessageBox.Show( ex.Message );
                }
            }
        }

        private void ResetButton_Click( object sender, RoutedEventArgs e )
        {
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            textBoxEmail.Clear();
            DOB.SelectedDate = null;
            textBoxPhone = null;
            textBoxAddress1.Clear();
            textBoxAddress2.Clear();
            textBoxCity.Clear();
            textBoxState.SelectedIndex = -1;
            textBoxZip.Clear();
            textBoxCountry.SelectedIndex = -1;
        }
    }
}
