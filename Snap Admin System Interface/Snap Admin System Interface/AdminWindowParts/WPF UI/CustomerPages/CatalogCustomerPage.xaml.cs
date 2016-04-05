using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using CSharpClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SnapRegisters;
using System.Globalization;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    internal class UsernameValidationRule: ValidationRule
    {
        public override ValidationResult Validate( object value, CultureInfo cultureInfo )
        {
            if( value != null && value.ToString() != string.Empty && !DBInterface.UserExists( value.ToString() ) )
                return new ValidationResult( false, value.ToString() + " is not an existing user." );
            return ValidationResult.ValidResult;
        }
    }

    internal class StateValidationRule: ValidationRule
    {

        public override ValidationResult Validate( object value, CultureInfo cultureInfo )
        {
            if( value != null && value.ToString() != string.Empty && !AddCustomerPage.isStateAbbreviation( value.ToString() ) )
                return new ValidationResult( false, value.ToString() + " is not a state abbreviation." );
            return ValidationResult.ValidResult;
        }
    }

    internal class CountryValidationRule: ValidationRule
    {
        public override ValidationResult Validate( object value, CultureInfo cultureInfo )
        {
            if( value != null && value.ToString() != string.Empty && !AddCustomerPage.countries.Contains( value.ToString() ) )
                return new ValidationResult( false, value.ToString() + " is not a country." );
            return ValidationResult.ValidResult;
        }
    }

    public class Customer: IEquatable<Customer>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RewardsPoints { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public bool Active { get; set; }
        public string DateOfBirth { get; set; }
        public string AttachedUserName { get; set; }

        public Customer Clone()
        {
            Customer c = new Customer();
            c.AttachedUserName = this.AttachedUserName;
            c.Active = this.Active;
            c.Address1 = this.Address1;
            c.Address2 = this.Address2;
            c.City = this.City;
            c.Country = this.Country;
            c.DateOfBirth = this.DateOfBirth;
            c.Email = this.Email;
            c.FirstName = this.FirstName;
            c.LastName = this.LastName;
            c.PhoneNumber = this.PhoneNumber;
            c.RewardsPoints = this.RewardsPoints;
            c.State = this.State;
            c.Zip = this.Zip;
            return c;
        }

        public bool Equals( Customer r )
        {
            return ( AttachedUserName == r.AttachedUserName && Active == r.Active && Address1 == r.Address1 && Address2 == r.Address2
                && City == r.City && Country == r.Country && DateOfBirth == r.DateOfBirth
                && Email == r.Email && FirstName == r.FirstName && LastName == r.LastName
                && PhoneNumber == r.PhoneNumber && RewardsPoints == r.RewardsPoints
                && State == r.State && Zip == r.Zip );
        }
    }

    public partial class CatalogCustomerPage :Page
    {
        public ObservableCollection<Customer> data = new ObservableCollection<Customer>();
        public CatalogCustomerPage()
        {
            InitializeComponent();
            PopulateList();
        }

        private void PopulateList()
        {
            data.Clear();
            CustomerGrid.Items.Clear();

            DBInterface.GetAllCustomers();

            foreach( XmlNode node in DBInterface.Response )
            {
                DateTime? dob = null;
                try
                {
                    dob = DateTime.Parse( node.Get( "DateOfBirth" ) );
                }
                catch( Exception ) { }

                data.Add( new Customer()
                {
                    FirstName = node.Get( "FName" ),
                    LastName = node.Get( "LName" ),
                    Address1 = node.Get( "Address1" ),
                    Address2 = node.Get( "Address2" ),
                    City = node.Get( "City" ),
                    State = node.Get( "State" ),
                    Country = node.Get( "Country" ),
                    Zip = node.Get( "Zip" ),
                    Email = node.Get( "Email" ),
                    PhoneNumber = long.Parse( node.Get( "PhoneNumber" ) ),
                    Active = node.Get( "Active" )[0] == '1',
                    DateOfBirth = dob?.Date.ToShortDateString(),
                    AttachedUserName = node.Get( "Username" )
                } );
            }
            LoadItems();
        }
        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
            {
                //CustomerGrid.Items.Clear();
                CustomerGrid.ItemsSource = data_cv;
                try
                {
                    data_cv.Filter = CustomerFilter; //can't add a filter when they're editing a column, but that's OK.
                }
                catch( Exception ) { }
            }
        }

        private bool CustomerFilter( object o )
        {
            Customer cust = o as Customer;
            return true;
        }

        private void CommitButton_Click( object sender, RoutedEventArgs e )
        {
            Customer cust = ( (FrameworkElement)sender ).DataContext as Customer;
            CustomerGrid.CommitEdit();

            
            //DBInterface.ModifyCustomer();
        }

        Customer edit_copy;
        private void CustomerGrid_BeginningEdit( object sender, DataGridBeginningEditEventArgs e )
        {
            Customer cust = e.Row.Item as Customer;
            edit_copy = cust.Clone();
        }


        private void CustomerGrid_CellEditEnding( object sender, DataGridCellEditEndingEventArgs e )
        {
            Customer cust = e.Row.Item as Customer;
        }
    }
}
