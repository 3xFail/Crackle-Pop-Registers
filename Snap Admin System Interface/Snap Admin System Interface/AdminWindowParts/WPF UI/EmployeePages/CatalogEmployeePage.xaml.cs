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
using System.Xml;
using CSharpClient;
using System.ComponentModel;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    public class UserData
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PermissionsGroup { get; set; }
        public bool Active { get; set; }
        public List<string> PermissionsGroups { get; set; }
    }
    /// <summary>
    /// Interaction logic for SearchEmployeePage.xaml
    /// </summary>
    public partial class CatalogEmployeePage : Page
    {
        private ObservableCollection<UserData> data = new ObservableCollection<UserData>();
        public CatalogEmployeePage()
        {
            InitializeComponent();
            PopulateList();
        }

        private void PopulateList()
        {
            data.Clear();

            DBInterface.GetAllPermissions();

            List<string> groups = new List<string>();
            foreach( XmlNode node in DBInterface.Response )
                groups.Add( node.Get( "PermissionsGroup" ) );


            DBInterface.GetAllEmployees();
            foreach( XmlNode node in DBInterface.Response )
            {
                data.Add( new UserData()
                {
                    UserID = int.Parse( node.Get( "UserID" ) )
                    , Username = node.Get( "Username" )
                    , PermissionsGroup = node.Get( "PermissionsGroup" )
                    , Active = node.Get( "Active" )[0] == '1'
                    , PermissionsGroups = groups
                } );
            }
            LoadItems();
        }

        private void LoadItems()
        {
            ICollectionView data_cv = CollectionViewSource.GetDefaultView( data );
            if( data_cv != null )
                EmployeeGrid.ItemsSource = data_cv;
        }

        private void ResetPasswordButton_Click( object sender, RoutedEventArgs e )
        {
            UserData user = ( (FrameworkElement)sender ).DataContext as UserData;

            string resp = PromptDialog.Prompt( "New password:", "Set " + user.Username + "'s password." );

            if( !string.IsNullOrEmpty( resp ) )
            {
                DBInterface.SetUserPassword( user.UserID, resp );
                MessageBox.Show( "Password for \"" + user.Username + "\" has been updated." );
            }
        }
        string oldgroup;
        private void PermissionGroup_Open( object sender, EventArgs e )
        {
            UserData user = ( (FrameworkElement)sender ).DataContext as UserData;
            oldgroup = user.PermissionsGroup;
        }

        private void PermissionGroup_Close( object sender, EventArgs e )
        {
            UserData user = ( (FrameworkElement)sender ).DataContext as UserData;
            if( user.PermissionsGroup != oldgroup )
            {
                DBInterface.ChangePermissions( user.UserID, user.PermissionsGroup );
            }
        }

        private void Active_Toggle( object sender, RoutedEventArgs e )
        {
            UserData user = ( (FrameworkElement)sender ).DataContext as UserData;
            DBInterface.SetUserActivity( user.UserID, user.Active );
        }
    }
}
