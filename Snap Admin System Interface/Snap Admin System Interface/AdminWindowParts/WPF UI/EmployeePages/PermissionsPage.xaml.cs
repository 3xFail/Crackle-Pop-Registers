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

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    internal class GroupNameValidationRule: ValidationRule
    {
        public override ValidationResult Validate( object value, CultureInfo cultureInfo )
        {
            value = value ?? string.Empty;
                DBInterface.GetAllPermissions();

                foreach( XmlNode node in DBInterface.Response )
                    if( node.Get( "PermissionsGroup") == value.ToString() )
                        return new ValidationResult( false, value.ToString() + " is already an existing group." );
            return ValidationResult.ValidResult;
        }
    }

    public class PermissionGroup
    {

        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public ulong Flags = 0;
    }

    public partial class PermissionsPage :Page
    {
        public ObservableCollection<PermissionGroup> data = new ObservableCollection<PermissionGroup>();
        public PermissionsPage()
        {
            InitializeComponent();
            PopulateList();
            for( int i = 0; i < Permissions._Permissions.Count; ++i )
            {
                CheckBoxSP.Children.Add( new CheckBox()
                {
                    Content = Permissions._Permissions[i]
                } );

                ( (CheckBox)CheckBoxSP.Children[i] ).Checked += CheckBox_Toggle;
                ( (CheckBox)CheckBoxSP.Children[i] ).Unchecked += CheckBox_Toggle;
            }
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

        PermissionGroup _group = null;
        private void PermissionsGrid_SelectionChanged( object sender, SelectionChangedEventArgs e )
        {
            try
            {
                PermissionGroup group = (PermissionGroup)e.AddedItems[0];
                _group = group;

                foreach( var child in CheckBoxSP.Children )
                {
                    CheckBox cb = (CheckBox)child;

                    cb.Checked -= CheckBox_Toggle;
                    cb.Unchecked -= CheckBox_Toggle;

                    cb.IsChecked = Permissions.CheckPermissions( group.Flags, (string)cb.Content );

                    cb.Checked += CheckBox_Toggle;
                    cb.Unchecked += CheckBox_Toggle;
                }
            }
            catch { }
        }

        private void CheckBox_Toggle( object sender, RoutedEventArgs e )
        {
            if( _group != null )
            {
                CheckBox cb = (CheckBox)sender;
                Permissions.SetPermission( ref _group.Flags, cb.IsChecked == true, (string)cb.Content );
                DBInterface.ModifyPermissionValue( _group.Name, (long)_group.Flags, (string)cb.Content, cb.IsChecked == true );
            }
        }

        string oldname;
        private void PermissionsGrid_BeginningEdit( object sender, DataGridBeginningEditEventArgs e )
        {
            PermissionGroup group = e.Row.Item as PermissionGroup;
            oldname = group.Name;
        }

        private void PermissionsGrid_CellEditEnding( object sender, DataGridCellEditEndingEventArgs e )
        {
            PermissionGroup group = e.Row.DataContext as PermissionGroup;
            TextBox t = e.EditingElement as TextBox;
            if( group != null && t?.Text != oldname )
            {
                if( oldname == string.Empty )
                {
                    try
                    {
                        DBInterface.AddPermission( t.Text );
                        System.Windows.Forms.MessageBox.Show( $"Successfully added group {t.Text}" );
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show( "Could not create group." );
                        t.Text = oldname;
                    }
                }
                else
                {
                    try
                    {
                        DBInterface.RenamePermissionlevel( t.Text, oldname, (long)group.Flags );
                        System.Windows.Forms.MessageBox.Show( $"Successfully renamed {oldname} to {t.Text}!" );
                    }
                    catch
                    {
                        System.Windows.Forms.MessageBox.Show( "Could not rename group." );
                        t.Text = oldname;
                    }
                }
            }
        }
    }
}
