using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public class ItemAndDiscountOutputObject
	//		SUMMARY:
	//			This is an object that contains reference to the item to display and it's coupons. This class's
	//			purpose is to remove this separation.
	//		MEMBERS:
	//			public double boxHeigh { get; set; }
	//				The height of a single coupon or item if no coupons are available.
	//			private StackPanel m_itemOutputPanel
	//				The panel to output the item box on.
	//			private StackPanel m_couponOutputPanel
	//				The panel to output coupons on.
	//			private Grid m_itemDiscriptionBox
	//				The GUI element for the item to display its name and price on.
	//			private StackPanel m_stackOfCoupons
	//				A stack of GUI elements containing information about coupons. Drawn to the couponOutputPanel.
	//		FUNCTIONS:
	//			public ItemAndDiscountOutputObject(...)
	//				Constructs an ItemDisplayBox and a series of coupons for a given item.
	//			public void AddDiscount(Coupon discount)
	//				Checks if the passed in coupon can be applied to this item and if so updates the display to
	//				reflect this.
	//			public void OutputItem()
	//				Draws this item in the output panels. Can be output multiple times if called more than once.
	//			public void UpdateHeight()
	//				Changes the height of the item box to reflect the size of all the coupons. This prevents
	//				coupons from looking like they belong to the wrong item.
	//*************************************************************************************************************
	class ItemAndDiscountOutputObject
	{
		public delegate void UpdateTotals();
		public ItemAndDiscountOutputObject(Item newItem, Transaction transaction, double heightOfEachBox, StackPanel itemOutputPanel, StackPanel couponOutputPanel, UpdateTotals updateFunction)
		{
			m_item = newItem;
			m_transaction = transaction;
			boxHeight = heightOfEachBox;
			m_itemOutputPanel = itemOutputPanel;
			m_couponOutputPanel = couponOutputPanel;

			m_stackOfCoupons = new StackPanel();

			m_updateFunction = updateFunction;

			ItemDisplayBox itemDisplayBox = new ItemDisplayBox(m_item, m_transaction, RemoveItem, UpdateItemDetails);
			itemDisplayBox.VerticalAlignment = System.Windows.VerticalAlignment.Top;
			itemDisplayBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			itemDisplayBox.Height = boxHeight;

			m_itemDescriptionBox = new Grid();
			m_itemDescriptionBox.Children.Add(itemDisplayBox);

			if (m_item.Discounts.Count() == 0)
			{
				Grid blankCoupon = new Grid();
				blankCoupon.Height = boxHeight;
				m_stackOfCoupons.Children.Add(blankCoupon);
				m_noDiscounts = true;
			}
			else
			{
				m_noDiscounts = false;
				foreach (IDiscount discount in m_item.Discounts)
				{
					CouponDisplayBox autoAppliedDiscount = new CouponDisplayBox(discount);
					autoAppliedDiscount.Height = boxHeight;
					m_stackOfCoupons.Children.Add(autoAppliedDiscount);
				}
			}
			UpdateHeight();
			OutputItem();
		}

		public void AddDiscount(Coupon discount)
		{
			if (discount.AppliesTo(m_item))
			{
				if (m_noDiscounts)
					m_stackOfCoupons.Children.Clear();

				m_noDiscounts = false;
				CouponDisplayBox newDiscount = new CouponDisplayBox(discount);
				newDiscount.Height = boxHeight;
				m_stackOfCoupons.Children.Add(newDiscount);
				UpdateHeight();
			}
		}

		public void  OutputItem()
		{
			m_itemOutputPanel.Children.Add(m_itemDescriptionBox);
			m_couponOutputPanel.Children.Add(m_stackOfCoupons);
		}

		public void UpdateHeight()
		{
			if (m_stackOfCoupons.Children.Count < 2 )
				m_itemDescriptionBox.Height = boxHeight;
			else
				m_itemDescriptionBox.Height = m_stackOfCoupons.Children.Count * boxHeight;
		}

		private void UpdateItemDetails()
		{
			ItemDisplayBox output = (ItemDisplayBox)m_itemDescriptionBox.Children[0];
			output.NameField.Text = m_item.ItemName.ToString();
			output.AmountField.Text = m_item.Price.ToString();
			m_updateFunction();
		}

		private void RemoveItem()
		{
			m_itemOutputPanel.Children.Remove(m_itemDescriptionBox);
			m_couponOutputPanel.Children.Remove(m_stackOfCoupons);
			m_updateFunction();
		}


		public double boxHeight { get; set; }

		private StackPanel m_itemOutputPanel;
		private StackPanel m_couponOutputPanel;
		private Transaction m_transaction;
		private bool m_noDiscounts;

		private Grid m_itemDescriptionBox;
		private StackPanel m_stackOfCoupons;
		private Item m_item;

		private UpdateTotals m_updateFunction;
	}
}
