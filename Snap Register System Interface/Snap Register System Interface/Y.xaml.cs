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
using System.Windows.Shapes;

namespace Snap_Register_System_Interface
{
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class Y : Window
	{
		public Y()
		{
			InitializeComponent();
		}

		private void Window_Deactivated(object sender, EventArgs e)
		{
			this.Topmost = true;
		}
	}
}
