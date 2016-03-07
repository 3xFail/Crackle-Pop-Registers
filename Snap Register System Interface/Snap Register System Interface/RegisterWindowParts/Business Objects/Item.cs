using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapRegisters
{
	//*************************************************************************************************************
	// public class Item
	//		SUMMARY: 
	//			This class represents an item in the sale. It contains basic information about it such as price,
	//			name and its id number.
	//		MEMBERS:
	//			public string ID;
	//				The UPC code of this item.
	//			public string ItemName
	//				The name of this item.
	//			public double Price
	//				The price of this item.
	//			public List<KeyValuePain<string name, double amount>> Discounts
	//				A list of all the discounts this item has. Discounts are stored as a key value pair, key
	//				being the name of the discount and the value being the amount of the discount.
	//		FUNCTIONS:
	//			public Item()
	//				Basic constructor.
	//			public Item(int id, string name, double price, List<KeyValuePair<string, double>> Discounts)
	//				Overloaded constructor allowing the construction from known elements of the item.
	//			public Item(Item source)
	//				Copy Constructor.
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class Item
	{
		public Item(string Name, float price, string barcode, int product_id)
		{
            ID = product_id;
            Barcode = barcode;
			ItemName = Name;
			Price = price;
            OriginalPrice = price;
		}
		public Item(Item source)
		{
			ID = source.ID;
            Barcode = source.Barcode;
			ItemName = source.ItemName;
			Price = source.Price;
            // Might be a shallow copy, not quite sure if STL's list does deep copy.
            Discounts = source.Discounts;
            OriginalPrice = source.OriginalPrice;
		}

        public void Apply( IDiscount sale )
        {
            Price = sale.ChangeAmountTo( Price );
        }

        public void AddDiscount( IDiscount discount )
        {
            Discounts.Add( discount );
            RecalculatePrice();
        }

        private void RecalculatePrice()
        {
            Price = OriginalPrice;
            Discounts.ApplyTo( this );
        }

        public int ID { get; set; } = 0;
        public string Barcode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public double Price { get; set; } = 0.0;
        private double OriginalPrice { get; set; } = 0.0;
        private DiscountList _Discounts = new DiscountList();
        public DiscountList Discounts
        {
            get { return _Discounts; }
            set { _Discounts = value; RecalculatePrice(); }
        }
   
	}
}
