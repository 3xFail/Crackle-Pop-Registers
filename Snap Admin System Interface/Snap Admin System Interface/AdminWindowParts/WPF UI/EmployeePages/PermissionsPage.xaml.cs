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
using System.Xml;
using CSharpClient;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SnapRegisters;
using System.Globalization;
using System.Data;
using PointOfSales.Permissions;
using SystemPermissions = PointOfSales.Permissions.Permissions.SystemPermissions;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    public class PermissionGroup
    {
        public string Name { get; set; } = string.Empty;
        public ulong Flags { get; set; } = 0;
        public bool LogIntoRegister { set { LogIntoRegister = value; } get { return Permissions.CheckPermissions( Flags, SystemPermissions.LOG_IN_REGISTER ); } }
        public bool IsOwner { set {  } get { return Permissions.CheckPermissions( Flags, SystemPermissions.IS_OWNER ); } }
    }

    public partial class PermissionsPage :Page
    {
        public ObservableCollection<PermissionGroup> data = new ObservableCollection<PermissionGroup>();
        public PermissionsPage()
        {
            InitializeComponent();
            PopulateList();
        }

        private void PopulateList()
        {
            data.Clear();
            PermissionsGrid.Items.Clear();

            DBInterface.GetAllPermissions();

            foreach( XmlNode node in DBInterface.Response )
            {
                data.Add( new PermissionGroup()
                {
                    Name = node.Get( "PermissionsGroup" ),
                    Flags = ulong.Parse( node.Get( "PermissionID" ) )
                } );
            }

            PermissionsGrid.ItemsSource = data;
        }

        private void PermissionsGrid_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            try
            {
                PermissionGroup group = (PermissionGroup)e.AddedItems[0];

                CheckBoxSP.DataContext = group;
            }
            catch { }
        }
    }
}
