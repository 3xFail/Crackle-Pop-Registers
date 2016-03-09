using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PointOfSales.Users;
using PointOfSales.Permissions;
using System.Device;
using CSharpClient;
using System.Xml;

namespace SnapRegisters
{
    //*************************************************************************************************************
    // public class Transaction
    //		SUMMARY: 
    //			This class contains all information about the current sale. A new class should be created for each
    //			Transaction at a register. This class allows the managing of a sale from scanning to payment.
    //		MEMBERS:
    //			public delegate void AddItemToOutput(ref Item);
    //				After an item is created this delegate will be called allowing the item to be output to some
    //          public delegate void AddCouponToOutput(ref Coupon);
    //				After an Coupon is created this delegate will be called allowing the Coupon to be output to some
    //				sort of display.
    //			private List<Item> m_Items
    //				A list of all items in the transaction.
    //			private Employee m_Employee
    //				The currently logged in employee.
    //          private List<Coupon> m_Coupons
    //              A list of all currently applied coupons for the transaction 
    //		FUNCTIONS:
    //			public Transaction(Employee employee, ref DockPanel itemOutput, ref DockPanel discountOutput)
    //				Constructor for a new transaction opened by the specified employee. itemOutput is a DockPanel
    //				passed by reference where items should be displayed to while discountOutput is the same thing
    //				except for discounts instead.
    //			public void AddItem(int itemID)
    //				Adds an item to the current transaction. Displays this item to the outputs.
    //			public void RemoveItem(int itemID)
    //				Removes the item with the ID matching "itemID" from the transaction. removes it from the
    //				output.
    //			public void OverrideCost(string itemID, double newPrice, string reason = "No description")
    //				Overrides the cost of the item specified with the new price specified with "newPrice".
    //				"reason" is the reason the employee chose to override the price.
    //			public void ApplyCoupon(string couponID)
    //				Applies a coupon to the sale.
    //			public void Checkout()
    //				Finishes the transaction and begins processing payment.
    //			public List<Item> GetItems()
    //				Returns a copy of all the items in this sale. The list of items cannot be changed without
    //				proper permissions but can be read with this.
    //			private Item ConstructItem(string itemID)
    //				Contacts the database and constructs an item from the given item ID.
    //          public List<Coupon> GetCoupons()
    //              returns a copy of all the coupons applied to items in the transaction
    //          private Coupon ConstructCoupon(string coupon_id)
    //              Contacts the database and constructs an coupon from a given coupon_id
    //
    //
    //
    //		PERMISSIONS:
    //			AddItem						- UseRegister
    //			RemoveItem					- UseRegister
    //			OverrideCost				- PriceOverride, PriceOverrideNoReason
    //			ApplyCoupon					- ApplyCoupon
    //			Checkout					- ProcessPayment
    //			GetItems					- None
    //*************************************************************************************************************
    public class Transaction
	{

		// Delegate for output function
		public delegate void ItemOutputDelegate(Item itemToAdd);
        public delegate void CouponOutputDelegate(Coupon couponToAdd);


        // TODO: Make it so that multiple of the same item can be added without breaking functions.
        public Transaction(Employee employee, ItemOutputDelegate itemToAdd, CouponOutputDelegate couponToAdd, connection_session session)
		{
			if (employee == null)
				throw new InvalidOperationException("Invalid Employee Credentials.");
			if (!Permissions.CheckPermissions(employee, Permissions.SystemPermissions.LOG_IN_REGISTER))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            m_connection = session;
			m_Employee = employee;
			m_Items = new List<Item>();
            m_Coupons = new List<Coupon>();
			m_OutputDelegate = itemToAdd;
            m_CouponOutputDelegate = couponToAdd;
            
		}

		public void AddItem(string itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.LOG_IN_REGISTER))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before adding it to the list.
			try
			{
				// Construct a new item from the given itemID and add it to the list.
				Item item = ConstructItem(itemID);
                item.Discounts = GetSales( item );

				// Fire whatever Output method has been assigned for this item.
				m_OutputDelegate(item);

				m_Items.Add(item);

				// TODO: Make this box's height equal to the combined discount's height.
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}

        //Modifies the item price given by the sales that are assigned to the item in the database
        public DiscountList GetSales(Item new_item)
        {
            m_connection.write(string.Format("GetSale_ProdID \"{0}\"", new_item.ID));

            DiscountList Discounts = new DiscountList();
            foreach (XmlNode sale in m_connection.Response)
            {
                string name = sale.Get( "Name" );
                bool flat = sale.Get( "Flat" )[0] == '1';
                double amount = double.Parse( sale.Get( "Discount" ) );
                Discounts.Add( new Sale( flat, name, amount ) );
            }
            return Discounts;
        }

		public void RemoveItem(string itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.LOG_IN_REGISTER))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before removing it from the list.
			try
			{
				m_Items.RemoveAll(x => x.Barcode == itemID);
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void OverrideCost(string itemID, double newPrice, string reason = "No description")
		{
			// Find the item to change the price of in the list assign changedItem these values.
			Item changedItem = m_Items.Find(x => x.Barcode == itemID);

			if (changedItem == null)
				throw new InvalidOperationException("Item specified is not in sale.");

			if (reason == "No description")
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.CAN_DISCOUNT_ITEMS))
					throw new InvalidOperationException("A reason must be specified for this action.");
				else
					changedItem.Price = newPrice;
			}
			else
			{
				if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.CAN_DISCOUNT_ITEMS))
					throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");
				else
					changedItem.Price = newPrice;
			}
		}
		public void AddCoupon(string couponID)
		{
            if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.LOG_IN_REGISTER))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            try
            {
                Coupon coupon = ConstructCoupon( couponID );

                foreach( Item item in m_Items )
                    if( coupon.AppliesTo( item ) )
                        item.AddDiscount( coupon );

				m_CouponOutputDelegate(coupon);

                m_Coupons.Add(coupon);
            }
            catch( Exception )
            {
                throw new ArgumentException( "Item or coupon with ID \"" + couponID + "\" not found" );
            }

        }

        public void Checkout()
		{
			// TODO: Insert credit card magic here.
		}

		public List<Item> GetItems()
		{
			return m_Items;
		}

        public List<Coupon> GetCoupons()
        {
            return m_Coupons;
        }

		private Item ConstructItem(string itemID)
		{
            m_connection.write( string.Format( "GetItem \"{0}\"", itemID ) );

            try
            {
                XmlNode it = m_connection.Response[0];

                if( it.Get("Active_Use" )[0] == '0' )
                    throw new InvalidOperationException( "Cannot sell inactive item" );

                float price = float.Parse( it.Get( "Price" ) );
                string name = it.Get( "Name" );
                int product_id = int.Parse( it.Get( "ProductID" ) );
                
                return new Item( name, price, itemID, product_id );
            }
            catch( NullReferenceException )
            {
                //check to see if a coupon
                //if scan is not a item or a coupon then throw error
                throw new ArgumentException( "Item with barcode \"" + itemID + "\" not found." );
            }
		}

        private Coupon ConstructCoupon(string coupon_id)
        {
            m_connection.write(string.Format("GetCoupon \"{0}\"", coupon_id));

            try
            {
                XmlNode it = m_connection.Response[0];

                if( it.Get("Active")[0] == '1' )
                    throw new InvalidOperationException("Cannot use inactive coupon");

                double discount = double.Parse(it.Get("Discount"));
                string related_barcode = it.Get("Barcode");
                string name = it.Get("Name");
                bool flat = it.Get( "Flat" )[0] == '1';


                return new Coupon( coupon_id, flat, name, discount );
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException( "Coupon with barcode \"" + coupon_id + "\" not found." );
            }

        }

		public ItemOutputDelegate m_OutputDelegate { get; set; }
        public CouponOutputDelegate m_CouponOutputDelegate { get; set; }
		private List<Item> m_Items = null;
        private List<Coupon> m_Coupons = null;
		private Employee m_Employee = null;
        private connection_session m_connection = null;
	}
}
