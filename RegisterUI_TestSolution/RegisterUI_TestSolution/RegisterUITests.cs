using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UITesting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UITest.Extension;
using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;


namespace RegisterUI_TestSolution
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
		}

		[TestMethod]
		public void Logout()
		{
			this.UIMap.Login();
			this.UIMap.Logout();
		}

		[TestMethod]
		public void AddSingleItem()
		{
			this.UIMap.Login();
			this.UIMap.Add_Item_Plain();
			this.UIMap.Assert_Single_Plain_Item();
			this.UIMap.Logout();
		}

		[TestMethod]
		public void AddMultiplePlainItems()
		{
			this.UIMap.Login();
			this.UIMap.Add_Item_Plain();
			this.UIMap.Add_Item_Plain();
			this.UIMap.Add_Item_Plain();
			this.UIMap.Assert_Multiple_Plain_Items();
			this.UIMap.Logout();
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
