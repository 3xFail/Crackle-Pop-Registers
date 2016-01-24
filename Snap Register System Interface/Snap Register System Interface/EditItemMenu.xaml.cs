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

namespace SnapRegisters
{
	/// <summary>
	/// Interaction logic for EditItemMenu.xaml
	/// </summary>
	public partial class EditItemMenu : Window
	{
		public EditItemMenu()
		{
			InitializeComponent();
		}

		private void CloseWindow(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
