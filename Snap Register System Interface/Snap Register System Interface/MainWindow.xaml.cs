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

namespace Snap_Register_System_Interface
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainMenuButtonClick(object sender, RoutedEventArgs e)
		{
			if (m_mainMenu == null)
			{
				this.Close();
				m_mainMenu = new Y();
				m_mainMenu.Show();
			}
			else
			{
				m_mainMenu.Hide();
				m_mainMenu.Close();
				m_mainMenu = null;
			}
		}

		private Snap_Register_System_Interface.Y m_mainMenu = null;
	}
}
