using SnapRegisters;
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

namespace SnapRegisters
{
	// Document me.
	public partial class CouponDisplayBox : UserControl
    {
        public CouponDisplayBox(IDiscount discount)
        {
            InitializeComponent();

            m_source = discount;

			NameField.Text = discount.ToString().Substring(0, Math.Min(discount.ToString().Length, 40));

			if (discount.IsFlat())
				m_discount = discount.Discount().ToString("C");
			else
				m_discount = ((int)(discount.Discount() * 100)).ToString() + '%'; //multiply discount amount by 100 then remove decimals then convert to string and add % => .10 becomes "10%"

			AmountField.Text = m_discount;
        }

        private void DisplayItemClickedEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditDiscountMenu editMenu = new EditDiscountMenu(m_source);
            editMenu.Show();
        }

        private IDiscount m_source;
		private string m_discount = string.Empty;
    }
}
