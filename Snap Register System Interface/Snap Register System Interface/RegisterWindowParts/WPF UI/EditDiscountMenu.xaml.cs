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
	/// Interaction logic for EditDiscountMenu.xaml
	/// </summary>
	public partial class EditDiscountMenu : Window
	{
		public EditDiscountMenu(IDiscount CouponToEdit)
		{
			InitializeComponent();
            m_source = CouponToEdit;
            ItemNameBox.Text = m_source.ToString();
		}

		private void DiscountWindowDeactivated(object sender, EventArgs e)
		{
			this.Close();
		}

        private IDiscount m_source = null;
	}
}
