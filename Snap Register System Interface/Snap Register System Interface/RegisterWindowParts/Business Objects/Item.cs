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
	//			public int ID;
	//				The id of this item.
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
		public Item()
		{
			ID = 0;
			ItemName = "";
			Price = 0;
			Discounts = new List<KeyValuePair<string, double>>();
		}
		public Item(Item source)
		{
			ID = source.ID;
			ItemName = source.ItemName;
			Price = source.Price;
			// Might be a shallow copy, not quite sure if STL's list does deep copy.
			Discounts = source.Discounts;
		}

		public int ID { get; set; }
		public string ItemName { get; set; }
		public double Price { get; set; }
		public List<KeyValuePair<string, double>> Discounts { get; set; }
	}
}
