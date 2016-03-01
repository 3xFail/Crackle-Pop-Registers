using Snap_Register_System_Interface.RegisterWindowParts.Business_Objects;
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

namespace Snap_Register_System_Interface.RegisterWindowParts.WPF_UI
{
    /// <summary>
    /// Interaction logic for CouponDisplayBox.xaml
    /// </summary>
    public partial class CouponDisplayBox : UserControl
    {
        public CouponDisplayBox(Coupon source)
        {
            InitializeComponent();

            m_source = source;

            NameField.Text = Convert.ToString(source.m_name[0]);
            for (int idx = 1; idx < 40 && idx < source.m_name.Length; idx++)
            {
                NameField.Text += source.m_name[idx];
            }

            raw_discount = m_source.m_discount;
            AmountField.Text = raw_discount.ToString("C");
        }

        private void DisplayItemClickedEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditDiscountMenu editMenu = new EditDiscountMenu(m_source);
            editMenu.Show();
        }

        private Coupon m_source;
        private double raw_discount = 0;
    }
}
