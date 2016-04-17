using SnapRegisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
	public partial class DiscountDisplayBox : UserControl
    {
		public delegate void RemoveDiscountFromDisplay(DiscountDisplayBox discount);
		public delegate void UpdateItemDisplay();
        public DiscountDisplayBox(IDiscount discount, Item possesingItem,Transaction transaction, RemoveDiscountFromDisplay removeFunction, UpdateItemDisplay updateFunction)
        {
            InitializeComponent();

            SourceDiscount = discount;
			PossessingItem = possesingItem;
			m_transaction = transaction;
			m_removeFunction = removeFunction;
			m_updateFunction = updateFunction;

			NameField.Text = discount.ToString().Substring(0, Math.Min(discount.ToString().Length, 40));

			UpdateDiscountString();

			AmountField.Text = Discount;

			PreviewMouseLeftButtonDown += DisplayItemClickedEvent;
		}

        private void DisplayItemClickedEvent(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
			EditMenu = new Popup();
			EditMenu.Child = new DiscountEditMenu(this, m_transaction, CloseEditMenu);
			EditMenu.IsOpen = true;
        }

		public void UpdateDiscountString()
		{
			if (SourceDiscount.IsFlat())
				Discount = SourceDiscount.Discount().ToString("C");
			else
				Discount = ((int)(SourceDiscount.Discount() * 100)).ToString() + '%'; //multiply discount amount by 100 then remove decimals then convert to string and add % => .10 becomes "10%"
		}

		private void CloseEditMenu()
		{
			DiscountEditMenu menu = (DiscountEditMenu)EditMenu.Child;

			EditMenu.IsOpen = false;

			if (menu.RemoveDiscount)
				m_removeFunction(this);
			else if (menu.AmountOverride)
			{
				PriceMenu = new Popup();
				PriceMenu.Child = new DiscountOverrideMenu(this, m_transaction , ClosePriceMenu);
				PriceMenu.IsOpen = true;
			}

			EditMenu = null;
		}

		private void ClosePriceMenu()
		{
			PriceMenu.IsOpen = false;
			PriceMenu = null;

			m_updateFunction();
		}

        public IDiscount SourceDiscount { get; }
		public Item PossessingItem { get; }
		private Transaction m_transaction;
		private RemoveDiscountFromDisplay m_removeFunction;
		private UpdateItemDisplay m_updateFunction;

		private Popup EditMenu { get; set; }
		private	Popup PriceMenu { get; set; }
		public string Discount { get; set; }
    }
}
