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
using SnapRegisters.RegisterWindowParts.Business_Objects;

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
				Item newItem = ConstructItem(itemID);

				// Fire whatever Output method has been assigned for this item.
				m_OutputDelegate(newItem);

				m_Items.Add(newItem);

				// TODO: Make this box's height equal to the combined discount's height.
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void RemoveItem(string itemID)
		{
			if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.LOG_IN_REGISTER))
				throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

			// Checks to make sure the item was valid before removing it from the list.
			try
			{
				m_Items.RemoveAll(x => x.ID == itemID);
			}
			catch (InvalidOperationException e)
			{
				throw e;
			}
		}
		public void OverrideCost(string itemID, double newPrice, string reason = "No description")
		{
			// Find the item to change the price of in the list assign changedItem these values.
			Item changedItem = m_Items.Find(x => x.ID == itemID);

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
            // used to check if the coupon matched any of the items in the transaction
            bool matching_flag = false;

            if (!Permissions.CheckPermissions(m_Employee, Permissions.SystemPermissions.LOG_IN_REGISTER))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            try
            {
                
                Coupon newCoupon = ConstructCoupon(couponID);

                // don't know what todo about this, have a funtion but not sure 
                // if i need to make another deleget to handle Coupons...

                //m_OutputDelegate(newCoupon);

                foreach (Item i in m_Items)
                {
                    if (i.ID == newCoupon.m_related_barcode)
                    {
                        matching_flag = true;
                        i.Discounts.Add(newCoupon);
                    }
                }

                if(matching_flag)
                    m_Coupons.Add(newCoupon);       
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
                
                return new Item( name, price, itemID );
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

                float discount = float.Parse(it.Get("PriceModification"));
                string related_barcode = it.Get("Barcode");
                string name = it.Get("Coupon_Name");


                return new Coupon( coupon_id, related_barcode, name, discount );
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
