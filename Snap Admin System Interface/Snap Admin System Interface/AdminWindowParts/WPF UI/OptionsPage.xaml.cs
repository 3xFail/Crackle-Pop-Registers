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
    /// Interaction logic for OptionsPage.xaml
    /// </summary>
    public partial class OptionsPage : Page
    {
        public OptionsPage()
        {
            InitializeComponent();
        }

        private void LogoBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();




            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string sourcePath = dlg.FileName;
                string targetPath = "..\\..\\..\\..\\SharedResources\\Images";

                string fileName = "Emblem.png";


                string sourceFile = System.IO.Path.Combine(sourcePath);
                string destFile = System.IO.Path.Combine(targetPath, fileName);

                //System.IO.File.Copy(sourceFile, destFile, true);

                string logoInStringFormat = LogoOperations.ImageToString(System.Drawing.Image.FromFile(dlg.FileName));



                DBInterface.ChangeLogo(7, logoInStringFormat);




                MessageBox.Show("You must restart the program for changes to take effect.");
            }






        }
    }
}
