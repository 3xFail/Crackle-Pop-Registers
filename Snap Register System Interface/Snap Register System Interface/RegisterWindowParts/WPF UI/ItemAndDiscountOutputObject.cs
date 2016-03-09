using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SnapRegisters
{
	class ItemAndDiscountOutputObject
	{
		public ItemAndDiscountOutputObject(Item newItem, double heightOfEachBox, StackPanel itemOutputPanel, StackPanel couponOutputPanel)
		{
			m_item = newItem;
			boxHeight = heightOfEachBox;

			foreach(IDiscount discount in newItem.Discounts)
			{
				CouponDisplayBox autoAppliedDiscount = new CouponDisplayBox(discount);
				m_stackOfCoupons.Children.Add(autoAppliedDiscount);
			}

			UpdateHeight();
			OutputItem();
		}

		public void AddDiscount(Coupon discount)
		{
			if (discount.AppliesTo(m_item))
			{
				m_item.AddDiscount(discount);
				CouponDisplayBox autoAppliedDiscount = new CouponDisplayBox(discount);
				m_stackOfCoupons.Children.Add(autoAppliedDiscount);
				UpdateHeight();
			}
		}

		public void  OutputItem()
		{
			m_ItemOutputPanel.Children.Add(m_itemDescriptionBox);
			m_couponOutputPanel.Children.Add(m_stackOfCoupons);
		}

		public void UpdateHeight()
		{
			m_itemDescriptionBox.Height = m_stackOfCoupons.Height;
		}

		public double boxHeight { get; set; }

		private StackPanel m_ItemOutputPanel;
		private StackPanel m_couponOutputPanel;


		private ItemDisplayBox m_itemDescriptionBox;
		private StackPanel m_stackOfCoupons;
		private Item m_item;
	}
}
