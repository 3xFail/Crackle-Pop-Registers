﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace RegisterUITests
{
	/// <summary>
	/// Summary description for CodedUITest1
	/// </summary>
	[CodedUITest]
	public class RegisterUITests
	{
		public RegisterUITests()
		{
		}

		[TestMethod]
		public void Login()
		{
			this.UIMap.Login();
			this.UIMap.CheckLoggedInUsername();
		}

		[TestMethod]
		public void AddItemByTypingBarcode()
		{
			this.UIMap.Login();
			this.UIMap.addItemWithUPCField();
			this.UIMap.SingleItemTotalsCheck();
		}

		[TestMethod]
		public void AddMultipleItemsByTypeingBarcode()
		{
			this.UIMap.Login();
			this.UIMap.addTomatoToSale();
			this.UIMap.addItemWithUPCField();
			this.UIMap.addTomatoToSale();
			this.UIMap.checkTotalsForMultipleItems();

		}

		[TestMethod]
		public void AddItemWithSale()
		{
			this.UIMap.Login();
			this.UIMap.addItemWithSale();
			this.UIMap.CheckSaleTotalsAmount();

		}

		[TestMethod]
		public void AddCoupon()
		{
			this.UIMap.Login();
			this.UIMap.addItemWithSale();
			this.UIMap.addCouponTo3ds();
			this.UIMap.checkTotalsForSingle3DSWithCoupon();

		}

		[TestMethod]
		public void RemoveSale()
		{
			this.UIMap.Login();
			this.UIMap.addItemWithSale();
			this.UIMap.RemoveSale();
			this.UIMap.checkTotalsRemoveSaleFrom3DS();
		}

		[TestMethod]
		public void RemoveCoupon()
		{
			this.UIMap.Login();
			this.UIMap.addItemWithSale();
			this.UIMap.addCouponTo3ds();
			this.UIMap.RemoveCouponFrom3DS();
			this.UIMap.checkTotalsAfterRemovingCouponFrom3DSWithSale();

		}

		[TestMethod]
		public void RemoveCouponAndSale()
		{
			this.UIMap.Login();
			this.UIMap.addItemWithSale();
			this.UIMap.addCouponTo3ds();
			this.UIMap.RemoveCouponFrom3DS();
			this.UIMap.RemoveSale();
			this.UIMap.checkTotalsRemoveSaleFrom3DS();
		}
		#region Additional test attributes

		// You can use the following additional attributes as you write your tests:

		////Use TestInitialize to run code before running each test 
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{        
		//    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
		//}

		////Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{        
		//    // To generate code for this test, select "Generate Code for Coded UI Test" from the shortcut menu and select one of the menu items.
		//}

		#endregion

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}
		private TestContext testContextInstance;

		public UIMap UIMap
		{
			get
			{
				if ((this.map == null))
				{
					this.map = new UIMap();
				}

				return this.map;
			}
		}

		private UIMap map;
	}
}