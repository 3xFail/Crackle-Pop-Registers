using SnapRegisters.RegisterWindowParts.Business_Objects;
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
		public EditDiscountMenu(Coupon CouponToEdit)
		{
			InitializeComponent();
            m_source = CouponToEdit;
            ItemNameBox.Text = m_source.m_name;
		}

		private void DiscountWindowDeactivated(object sender, EventArgs e)
		{
			this.Close();
		}

        private Coupon m_source = null;
	}
}
