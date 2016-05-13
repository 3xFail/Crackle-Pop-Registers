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
using System.Windows.Forms;
using System.Threading;

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
    //			public void RemoveItem(Item item)
    //				Removes the first item that matches the item passed in, This does not use the barcode
    //				as several copies of an item with different data could exist (e.g. coupons).
    //			public void OverrideCost(Item item, double newPrice, string reason = "No description")
    //				Overrides the cost of the item specified with the new price specified with "newPrice".
    //				"reason" is the reason the employee chose to override the price.
    //			public void ApplyCoupon(string couponID)
    //				Applies a coupon to the sale.
    //			public void AddCustomCoupon(Item itemToApplyTo, decimal amount);
    //				Applies a custom flat coupon to the item specified.
    //			public void RemoveDiscount(Item itemToRemoveFrom, IDiscount discountToRemove)
    //				Removes the given discount from the given item.
    //			public void OverrideDiscount(Item itemToChange, IDiscount discountToChange, decimal newAmount)
    //				Changes the value of a discount to the given amount.
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
    //*************************************************************************************************************
    public class Transaction
    {

        // Delegates for output function
        public delegate void ItemOutputDelegate(Item itemToAdd);
        public delegate void CouponOutputDelegate(Coupon couponToAdd);

        public ItemOutputDelegate m_OutputDelegate { get; set; }
        public CouponOutputDelegate m_CouponOutputDelegate { get; set; }
        public List<Item> m_Items { get; private set; } = new List<Item>();
        private List<Coupon> m_Coupons { get; set; } = new List<Coupon>();
        private Employee m_Employee { get; set; }
        public Snap_Register_System_Interface.RegisterWindowParts.Business_Objects.Customer m_customer { get; set; } = null;
        private ConnectionSession m_connection { get; set; }
        private static readonly decimal errorMargin = .05M;

        // TODO: Make it so that multiple of the same item can be added without breaking functions.
        public Transaction(Employee employee, Snap_Register_System_Interface.RegisterWindowParts.Business_Objects.Customer cust, ItemOutputDelegate itemToAdd, CouponOutputDelegate couponToAdd, ConnectionSession session)
        {
            if (employee == null)
                throw new InvalidOperationException("Invalid Employee Credentials.");
            if (!employee.HasPermisison(Permissions.RegisterLogIn))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            m_connection = session;
            m_Employee = employee;
            m_customer = cust;
            m_OutputDelegate = itemToAdd;
            m_CouponOutputDelegate = couponToAdd;

        }

        public void AddItem(string itemID, Scale.Scale theScale)
        {
            if (!m_Employee.HasPermisison(Permissions.RegisterLogIn))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            // Checks to make sure the item was valid before adding it to the list.
            try
            {
                // Construct a new item from the given itemID and add it to the list.
                Item item = ConstructItem(itemID, theScale);

                

                item.Discounts = GetSales(item);

                ApplyItemToExistingCoupons(ref item);

                // Fire whatever Output method has been assigned for this item.
                m_Items.Add(item);
                m_OutputDelegate(item);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
        }

        //Modifies the item price given by the sales that are assigned to the item in the database
        public DiscountList GetSales(Item new_item)
        {
            m_connection.Write("GetSale_ProdID @0", new_item.ID);

            DiscountList Discounts = new DiscountList();
            foreach (XmlNode sale in m_connection.Response)
            {
                string name = sale.Get("Name");
                bool flat = sale.Get("Flat")[0] == '1';
                decimal amount = decimal.Parse(sale.Get("Discount"));
                Discounts.Add(new Sale(flat, name, amount));
            }
            return Discounts;
        }

        public void RemoveItem(Item item)
        {
            //if (m_Employee.HasPermisison(Permissions.CanVoidItem))
            //throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.CanVoidItem)); //("User does not have sufficient permissions to use this machine.");

            // Checks to make sure the item was valid before removing it from the list.
            //try
            //{
            if (m_Employee.HasPermisison(Permissions.CanVoidItem))
                m_Items.Remove(item);
            else throw new UnauthorizedAccessException(Permissions.ErrorMessage(Permissions.CanVoidItem));
            //}
            //catch (InvalidOperationException e)
            //{
            //throw e;
            //}
            //else throw new UnauthorizedAccessException(Permissions.ErrorMessage(Permissions.CanVoidItem));

        }

        public void ApplyItemToExistingCoupons(ref Item item)
        {
            foreach (Coupon coupon in m_Coupons)
                if (coupon.AppliesTo(item))
                    item.AddDiscount(coupon);

            StringBuilder coupon_list = new StringBuilder();

            foreach (Coupon coupon in m_Coupons)
                if (!coupon.AppliesTo(item))
                    coupon_list.Append(coupon.Barcode + ',');

            if (coupon_list.Length != 0)
            {
                coupon_list.Length--;
                m_connection.Write("CheckItem_list @0, @1", coupon_list.ToString(), item.ID);

                foreach (XmlNode node in m_connection.Response)
                {
                    foreach (Coupon coupon in m_Coupons)
                    {
                        if (coupon.Barcode == node.Get("CouponID"))
                        {
                            item.AddDiscount(coupon);
                            coupon.AddRelatedID(item.ID);
                        }
                    }
                }
            }

        }

        // Deprecated.
        public void OverrideCost(Item item, decimal newPrice)
        {
            // Find the item to change the price of in the list assign changedItem these values.
            Item changedItem = m_Items.Find(x => x == item);

            if (changedItem == null)
                throw new InvalidOperationException("Item specified is not in sale.");

            if (!m_Employee.HasPermisison(Permissions.CanDiscountItems))
                throw new InvalidOperationException("User does not have sufficient permissions to perform this action.");
            else
                changedItem.Price = newPrice;
        }
        public void AddCustomCoupon(Item item, decimal amount)
        {
            try
            {
                // Find the item to change the price of in the list assign changedItem these values.
                Item changedItem = m_Items.Find(x => x == item);

                if (changedItem == null)
                    throw new InvalidOperationException("Item specified is not in sale.");

                if (!m_Employee.HasPermisison(Permissions.CanDiscountItems))
                    throw new InvalidOperationException(Permissions.ErrorMessage(Permissions.CanDiscountItems));

                item.AddDiscount(new ManagerOverrideDiscount(item.OriginalPrice - amount));
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void AddCoupon(string couponID)
        {
            if (!m_Employee.HasPermisison(Permissions.RegisterLogIn))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            if (m_Coupons.Any(x => x.Barcode == couponID))
                throw new ArgumentException("Cannot scan the same coupon twice");

            try
            {
                Coupon coupon = ConstructCoupon(couponID);

                foreach (Item item in m_Items)
                    if (coupon.AppliesTo(item))
                        item.AddDiscount(coupon);

                m_Coupons.Add(coupon);
                m_CouponOutputDelegate(coupon);
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new ArgumentException("Item or coupon with ID \"" + couponID + "\" not found");
            }

        }

        public void RemoveDiscount(Item itemToRemoveFrom, IDiscount discountToRemove)
        {
            if (!m_Employee.HasPermisison(Permissions.RegisterLogIn))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            // Checks to make sure the item was valid before removing it from the list.
            try
            {
                Item containingItem = m_Items.Find(x => x == itemToRemoveFrom);

                containingItem.Discounts.Remove(discountToRemove);
                containingItem.Price += discountToRemove.Discount();
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
        }

        public void OverrideDiscount(Item itemToChange, IDiscount discountToChange, decimal amount)
        {
            if (!m_Employee.HasPermisison(Permissions.RegisterLogIn))
                throw new InvalidOperationException("User does not have sufficient permissions to use this machine.");

            // Checks to make sure the item was valid before removing it from the list.
            try
            {
                //Item containingItem = m_Items.Find(x => x == itemToChange);

                //IDiscount discount = containingItem.Discounts.Find(x => x == discountToChange);

                // This function does not appear to do what it's name implies.
                //discount.ChangeAmountTo(amount);

                itemToChange.Price += discountToChange.Amount - amount;
                discountToChange.Amount = amount;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }
        }
        public void Checkout()
        {
            object id = m_customer?.cust_id ?? (object)DBNull.Value; //cust_id or DBNull if m_cust is null

            m_connection.Write("CreateOrder @0, @1", id, m_Employee.ID);
            int OrderID = int.Parse(m_connection.Response[0].Get("OrderID"));

            foreach (Item item in m_Items)
                m_connection.WriteNoResponse("AddOrderItem_ProductID @0, @1, @2", item.ID, OrderID, item.Price);
        }

        private Item ConstructItem(string itemID, Scale.Scale theScale)
        {
            
            m_connection.Write("GetItem @0", itemID);

            try
            {
                XmlNode it = m_connection.Response[0];
                string name = it.Get("Name");

                if (it.Get("Active")[0] == '0')
                    throw new InvalidOperationException("Cannot sell inactive item \"" + name + "\"");

                decimal price = 0;

                //If the item is sold by weight
                //Can't disable to bypass this, because how can you construct
                //items that sell by their weight, if you don't have their weight?
                if (it.Get("Weighable")[0] == '1')
                {
                   
                    //Quickchecks, don't need to stabilize to check for these error conditions.
                    if (theScale.GetWeightAsDecimal() == 0)
                        throw new InvalidOperationException("Please place items on the scale before continuing.");
                    if (theScale.GetWeightAsDecimal() == null)
                        throw new InvalidOperationException("Scale not found. Please check your connection.");
                    if (theScale.GetWeightAsDecimal() == -1)
                        throw new InvalidOperationException("Please remove items and recalibrate the scale before continuing.");


                    //while (!theScale._isStable) ;

                    //After all quick checks pass, get stabilized weight. To use in all calculations.
                    decimal? realWeightOfItem = theScale.getStabilizedWeight();

                    price = decimal.Parse(it.Get("Price")) * Convert.ToDecimal(realWeightOfItem);

                    //Display how heavy 
                    name += " " + Math.Round((decimal)realWeightOfItem, 2) + " Lb " + "@ $" + Math.Round(decimal.Parse(it.Get("Price")), 2) + "/Lb";
                }
                else    //Regular item - CHECK FOR CORRECT WEIGHT HERE
                {
#if DEBUG //Change this tag if you want to disable weight checking for fixed-weight items
                    if (theScale.IsConnected) //If a scale is connected, check for correct weight
                    {

                        if (it.Get("Weight") == "0.00" )   //If the weight is 0, it is assumed that this item doesn't need to be weighed (e.g. a 5 pack of bananas)
                        {
                            

                        }
                        else
                        {
                            //Quickchecks
                            if (theScale.GetWeightAsDecimal() == null)
                                throw new InvalidOperationException("Scale not found. Please check your connection.");
                            if (theScale.GetWeightAsDecimal() == -1)
                                throw new InvalidOperationException("Please remove items and recalibrate the scale before continuing.");


                            //while (!theScale._isStable || theScale.Status() == 0x2) { theScale.GetWeightAsDecimal(); Thread.Sleep(50); }

                            decimal? realWeightofItem = theScale.getStabilizedWeight();
                            if (realWeightofItem == null)
                                throw new NullReferenceException("Scale disconnected during stabilized weight retrieval.");

                            decimal dbItemWeight = decimal.Parse(it.Get("Weight"));

                            if (
                                (realWeightofItem > (dbItemWeight * (1 + errorMargin)))
                                || (realWeightofItem < (dbItemWeight * (1 - errorMargin))))
                            {
                                throw new InvalidOperationException("Weight is incorrect. Please try again.");
                            }


                        }
                    }
                    else
                    {

                    }
#endif

                    price = decimal.Parse(it.Get("Price"));
                }

                int product_id = int.Parse(it.Get("ProductID"));

                if (price == 0)
                    throw new ArgumentException("Oops. Something went wrong. (Price calculated to zero)");


                return new Item(name, price, itemID, product_id);
            }
            catch (NullReferenceException)
            {
                //check to see if a coupon
                //if scan is not a item or a coupon then throw error
                throw new ArgumentException("Item with barcode \"" + itemID + "\" not found.");
            }
        }

        private Coupon ConstructCoupon(string coupon_id)
        {
            m_connection.Write("GetCoupon_ID @0", coupon_id);

            try
            {
                XmlNode it = m_connection.Response[0];

                if (it.Get("Active")[0] == '0')
                    throw new InvalidOperationException("Cannot use inactive coupon");

                decimal discount = decimal.Parse(it.Get("Discount"));
                string name = it.Get("Name");
                bool flat = it.Get("Flat")[0] == '1';

                Coupon coupon = new Coupon(coupon_id, flat, name, discount);

                StringBuilder item_list = new StringBuilder();
                m_Items.ForEach(i => item_list.Append(i.ID.ToString() + ','));

                if (item_list.Length != 0)
                {
                    item_list.Length--;
                    m_connection.Write("CheckCoupons_list @0, @1", item_list.ToString(), coupon_id); //product ID

                    foreach (XmlNode node in m_connection.Response)
                        coupon.AddRelatedID(int.Parse(node.Get("ProductID")));
                }

                return coupon;
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (NullReferenceException e)
            {
                throw new ArgumentException("Coupon with barcode \"" + coupon_id + "\" not found.");
            }

        }
    }
}
