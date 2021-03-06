﻿using System;
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
using PointOfSales.Permissions;

namespace Snap_Admin_System_Interface.AdminWindowParts.WPF_UI
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PermissionsGroup { get; set; }
        public bool Active { get; set; }
        public List<string> PermissionsGroups { get; set; }
    }

    public partial class CatalogEmployeePage : Page
    {
        private ObservableCollection<User> data = new ObservableCollection<User>();
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
                data.Add( new User()
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
            try
            {
                if (!Permissions.CheckPermissions(DBInterface.m_employee, Permissions.ResetEmployeePassword))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.ResetEmployeePassword));

                User user = ( (FrameworkElement)sender ).DataContext as User;

                string response = PromptDialog.Prompt( "New password:", "Set " + user.Username + "'s password." );

                if( !string.IsNullOrEmpty( response ) )
                {
                    DBInterface.SetUserPassword( user.UserID, response, user.Username );
                    MessageBox.Show( "Password for \"" + user.Username + "\" has been updated." );
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string oldgroup;
        private void PermissionGroup_Open( object sender, EventArgs e )
        {

            User user = ( (FrameworkElement)sender ).DataContext as User;
            oldgroup = user.PermissionsGroup;
        }

        private void PermissionGroup_Close( object sender, EventArgs e )
        {
            try
            {
                User user = ( (FrameworkElement)sender ).DataContext as User;
                if( user.PermissionsGroup != oldgroup )
                {
                    if (!Permissions.CheckPermissions(DBInterface.m_employee, Permissions.ChangeEmployeeCatalog))
                        throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.ChangeEmployeeCatalog));

                    DBInterface.ChangePermissions( user.Username, user.UserID, user.PermissionsGroup );
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Active_Toggle( object sender, RoutedEventArgs e )
        {
            try
            {
                if (!Permissions.CheckPermissions(DBInterface.m_employee, Permissions.ChangeEmployeeCatalog))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.ChangeEmployeeCatalog));

                User user = ( (FrameworkElement)sender ).DataContext as User;
                DBInterface.SetUserActivity( user.UserID, user.Active, user.Username );
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
                
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
