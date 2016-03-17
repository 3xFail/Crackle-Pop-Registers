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
    /// Interaction logic for AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        // all US states
        private static string states = "AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY";
        // all the countries
        private static string countries = "Afghanistan|Åland Islands|Albania|Algeria|American Samoa|Andorra|Angola|Anguilla|Antigua and Barbuda|Argentina|Armenia|Aruba|Australia|Austria|Azerbaijan|Bangladesh|Barbados|Bahamas|Bahrain|Belarus|Belgium|Belize|Benin|Bermuda|Bhutan|Bolivia|Bosnia and Herzegovina|Botswana|Brazil|British Indian Ocean Territory|British Virgin Islands|Brunei Darussalam|Bulgaria|Burkina Faso|Burma|Burundi|Cambodia|Cameroon|Canada|Cape Verde|Cayman Islands|Central African Republic|Chad|Chile|China|Christmas Island|Cocos (Keeling) Islands|Colombia|Comoros|Congo-Brazzaville|Congo-Kinshasa|Cook Islands|Costa Rica|$_[|Croatia|Curaçao|Cyprus|Czech Republic|Denmark|Djibouti|Dominica|Dominican Republic|East Timor|Ecuador|El Salvador|Egypt|Equatorial Guinea|Eritrea|Estonia|Ethiopia|Falkland Islands|Faroe Islands|Federated States of Micronesia|Fiji|Finland|France|French Guiana|French Polynesia|French Southern Lands|Gabon|Gambia|Georgia|Germany|Ghana|Gibraltar|Greece|Greenland|Grenada|Guadeloupe|Guam|Guatemala|Guernsey|Guinea|Guinea-Bissau|Guyana|Haiti|Heard and McDonald Islands|Honduras|Hong Kong|Hungary|Iceland|India|Indonesia|Iraq|Ireland|Isle of Man|Israel|Italy|Jamaica|Japan|Jersey|Jordan|Kazakhstan|Kenya|Kiribati|Kuwait|Kyrgyzstan|Laos|Latvia|Lebanon|Lesotho|Liberia|Libya|Liechtenstein|Lithuania|Luxembourg|Macau|Macedonia|Madagascar|Malawi|Malaysia|Maldives|Mali|Malta|Marshall Islands|Martinique|Mauritania|Mauritius|Mayotte|Mexico|Moldova|Monaco|Mongolia|Montenegro|Montserrat|Morocco|Mozambique|Namibia|Nauru|Nepal|Netherlands|New Caledonia|New Zealand|Nicaragua|Niger|Nigeria|Niue|Norfolk Island|Northern Mariana Islands|Norway|Oman|Pakistan|Palau|Panama|Papua New Guinea|Paraguay|Peru|Philippines|Pitcairn Islands|Poland|Portugal|Puerto Rico|Qatar|Réunion|Romania|Russia|Rwanda|Saint Barthélemy|Saint Helena|Saint Kitts and Nevis|Saint Lucia|Saint Martin|Saint Pierre and Miquelon|Saint Vincent|Samoa|San Marino|São Tomé and Príncipe|Saudi Arabia|Senegal|Serbia|Seychelles|Sierra Leone|Singapore|Sint Maarten|Slovakia|Slovenia|Solomon Islands|Somalia|South Africa|South Georgia|South Korea|Spain|Sri Lanka|Sudan|Suriname|Svalbard and Jan Mayen|Sweden|Swaziland|Switzerland|Syria|Taiwan|Tajikistan|Tanzania|Thailand|Togo|Tokelau|Tonga|Trinidad and Tobago|Tunisia|Turkey|Turkmenistan|Turks and Caicos Islands|Tuvalu|Uganda|Ukraine|United Arab Emirates|United Kingdom|United States|Uruguay|Uzbekistan|Vanuatu|Vatican City|Vietnam|Venezuela|Wallis and Futuna|Western Sahara|Yemen|Zambia|Zimbabwe";
        // the three permission levels
        private static string permissions = "Cashier|Manager|Owner";

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
        // load the permission items in combobox
        private void Authorization_Loaded(object sender, RoutedEventArgs e)
        {
            // get the combobox reference
            var comboBox = sender as ComboBox;

            // assign the ItemsSource to the permission levels
            comboBox.ItemsSource = permissions.Split('|');

            // make no item selected
            comboBox.SelectedIndex = -1;
        }
        // select the permission level
        private void Authorization_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        public static bool isStateAbbreviation(string state)
        {
            return state.Length == 2 && states.IndexOf(state) > 0;
        }
        // submits the input values into the database
        private void SubmitButton_Click( object sender, RoutedEventArgs e )
        {
            if( textBoxFirstName.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "First name is required. Please enter it." );
            else if( textBoxUsername.Text.Length < 8 )
                System.Windows.Forms.MessageBox.Show( "Username is required and must be 8 characters or more. Please enter it." );
            else if( textBoxPassword.Password == string.Empty )
                System.Windows.Forms.MessageBox.Show( "Password is required. Please enter it." );
            else if( textBoxPhone.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "Phone number is required. Please enter it." );
            else if( textBoxAuthorization.Text == string.Empty )
                System.Windows.Forms.MessageBox.Show( "Authorization level is required. Please select one." );
            else
            {
                try
                {
                    DBInterface.AddEmployee( textBoxFirstName.Text, textBoxLastName.Text, textBoxUsername.Text, textBoxEmail.Text, textBoxPassword.Password, textBoxAuthorization.Text, DOB.DisplayDate, textBoxPhone.Text, textBoxAddress1.Text, textBoxAddress2.Text, textBoxCity.Text, textBoxState.Text, textBoxCountry.Text, textBoxZip.Text );
                    System.Windows.Forms.MessageBox.Show( "Successfully added " + textBoxUsername.Text + '!' );
                    ResetButton_Click( null, null );
                }
                catch( Exception exp)
                {
                    System.Windows.Forms.MessageBox.Show( exp.Message);
                }
            }
        }

        private void ResetButton_Click( object sender, RoutedEventArgs e )
        {
            textBoxFirstName.Text = string.Empty;
            textBoxLastName.Text = string.Empty;
            textBoxUsername.Text = string.Empty;
            textBoxEmail.Text = string.Empty;
            textBoxPassword.Password = string.Empty;
            DOB.SelectedDate = null;
            textBoxPhone.Text = string.Empty;
            textBoxAddress1.Text = string.Empty;
            textBoxAddress2.Text = string.Empty;
            textBoxCity.Text = string.Empty;
            textBoxState.SelectedIndex = -1;
            textBoxZip.Text = string.Empty;
            textBoxCountry.SelectedIndex = -1;
            textBoxAuthorization.SelectedIndex = -1;
        }
    }
}
