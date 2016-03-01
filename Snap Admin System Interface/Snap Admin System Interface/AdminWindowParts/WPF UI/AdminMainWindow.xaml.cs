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
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device;
//using SnapRegisters.AdminWindowParts.WPF_UI;
using System.Windows.Threading;
using Snap_Admin_System_Interface.AdminWindowParts.WPF_UI;

namespace SnapRegisters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow(Employee currentEmployee)
        {
            InitializeComponent();
            //m_employee = currentEmployee;
        }
        //    public AdminMainWindow()
        //{
        //    InitializeComponent();
        //}

        // opens the inventory page
        private void btn_Inv(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new InventoryPage());
            this.NavigationFrame.Navigate(new Uri("InventoryPage.xaml", UriKind.Relative));
        }
        // opens the analysis page
        private void btn_Anal(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new AnalysisPage());
            this.NavigationFrame.Navigate(new Uri("AnalysisPage.xaml", UriKind.Relative));
        }
        // opens the employees page
        private void btn_Emp(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new EmployeesPage());
            this.NavigationFrame.Navigate(new Uri("EmployeesPage.xaml", UriKind.Relative));
        }
        // opens the customers page
        private void btn_Cust(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new CustomersPage());
            this.NavigationFrame.Navigate(new Uri("CustomersPage.xaml", UriKind.Relative));
        }
        // opens the sales page
        private void btn_Sales(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new SalesPage());
            this.NavigationFrame.Navigate(new Uri("SalesPage.xaml", UriKind.Relative));
        }
        // opens the options page
        private void btn_Opt(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new OptionsPage());
            this.NavigationFrame.Navigate(new Uri("OptionsPage.xaml", UriKind.Relative));
        }
        //// opens the add employee page
        //private void AddEmp(object sender, RoutedEventArgs e)
        //{
        //    NavigationFrame.Navigate(new AddEmployeePage());
        //    this.NavigationFrame.Navigate(new Uri("AddEmployeePage.xaml", UriKind.Relative));
        //}
    }
}

