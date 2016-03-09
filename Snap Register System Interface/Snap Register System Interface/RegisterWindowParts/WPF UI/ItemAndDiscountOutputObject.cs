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
	//			private ItemDisplayBox m_itemDiscriptionBox
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
		public ItemAndDiscountOutputObject(Item newItem, double heightOfEachBox, StackPanel itemOutputPanel, StackPanel couponOutputPanel)
		{
			m_item = newItem;
			boxHeight = heightOfEachBox;
			m_itemOutputPanel = itemOutputPanel;
			m_couponOutputPanel = couponOutputPanel;

			m_stackOfCoupons = new StackPanel();

			ItemDisplayBox itemDisplayBox = new ItemDisplayBox(newItem);

			m_itemDescriptionBox = itemDisplayBox;

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
			m_itemOutputPanel.Children.Add(m_itemDescriptionBox);
			m_couponOutputPanel.Children.Add(m_stackOfCoupons);
		}

		public void UpdateHeight()
		{
			m_itemDescriptionBox.Height = m_stackOfCoupons.Height;
		}

		public double boxHeight { get; set; }

		private StackPanel m_itemOutputPanel;
		private StackPanel m_couponOutputPanel;


		private ItemDisplayBox m_itemDescriptionBox;
		private StackPanel m_stackOfCoupons;
		private Item m_item;
	}
}
