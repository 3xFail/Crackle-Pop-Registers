namespace SnapRegisters
{
	//*************************************************************************************************************
	// public class Item
	//		SUMMARY: 
	//			This class represents an item in the sale. It contains all information about the item
	//		PROPERTIES:
	//			public string ID [ get, set ] default: string.Empty
	//				The UPC code of this item.
	//			public string ItemName [ get, set ] default: string.Empty
	//				The name of this item.
	//			public double Price [ get, set ] default: 0
	//				The current price of the item with discounts applied.
    //          public double OriginalPrice [ get, private set ] default: 0
    //              The price of the item without discounts applied. 
    //              Used for recalculating discount values and showing total amount discounted
	//			public DiscountList Discounts [ get, set ] default: new DiscountList();
	//				Handles recalculating the price of the item when new discounts are applied
    //              Also contains information about all discounts that apply to the item
    //          
	//		FUNCTIONS:
	//			public Item()
	//				Basic constructor.
	//			public Item( string name, float price, string barcode, int product_id )
	//				Overloaded constructor allowing the construction from known elements of the item.
	//			public Item(Item source)
	//				Copy Constructor.
    //          public AddDiscount( IDiscount discount )
    //              Adds the passed discount to the Items DiscountList and recalculates the price
    //              
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class Item
	{
		public Item(string name, decimal price, string barcode, int product_id)
		{
            ID = product_id;
            Barcode = barcode;
			ItemName = name;
			Price = price;
            OriginalPrice = price;
		}
		public Item(Item source)
		{
			ID = source.ID;
            Barcode = source.Barcode;
			ItemName = source.ItemName;
			Price = source.Price;
            Discounts = source.Discounts;
            OriginalPrice = source.OriginalPrice;
		}

        public void AddDiscount( IDiscount discount )
        {
            Discounts.Add( discount );
            Discounts.ApplyTo( this );
        }

        public int ID { get; set; } = 0;
        public string Barcode { get; set; } = string.Empty;
        public string ItemName { get; set; } = string.Empty;
        public decimal Price { get; set; } = 0M;
        public decimal OriginalPrice { get; private set; } = 0M;
        private DiscountList _Discounts = new DiscountList();
        public DiscountList Discounts
        {
            get { return _Discounts; }
            set { _Discounts = value; Discounts.ApplyTo( this ); }
        }
   
	}
}
