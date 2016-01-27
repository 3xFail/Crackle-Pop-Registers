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
	//			This class represents an item in the sale. It contains an ItemDisplayBox and a list of
	//			DiscountDisplayBoxs that apply to the item.
	//		MEMBERS:
	//			public int id;
	//				The id of this item.
	//			public ItemDisplayBox
	//				This box is responsible for displaying the name and the price of the item. Contains a grid
	//				with the name of the item left aligned and the price of the item right aligned. Can be
	//				manipulated with the SetItemName() and SetItemPrice() functions.
	//			public List<DiscountDisplayBox> discounts
	//				This is a list of box's responsible for displaying one discount item for this item. Each box
	//				contains a grid with the name of the discount left aligned and the amount of the discount right
	//				aligned. Can be manipulated with the SetDiscountName() and SetDiscountAmount() functions.
	//		FUNCTIONS:
	//			public Item()
	//				Basic constructor.
	//			public Item(Item source)
	//				Copy Constructor.
	//		PERMISSIONS:
	//			None.
	//*************************************************************************************************************
	public class Item
	{
		public Item()
		{
			id = 0;
			itemDisplayBox = new ItemDisplayBox();
			discounts = new List<DiscountDisplayBox>();
		}
		public Item(Item source)
		{
			id = source.id;
			itemDisplayBox = new ItemDisplayBox(source.itemDisplayBox.GetItemNameAsString(), source.itemDisplayBox.GetItemPriceAsDouble());
			discounts = new List<DiscountDisplayBox>(source.discounts);
		}

		public int id;
		public ItemDisplayBox itemDisplayBox { get; set; }
		public List<DiscountDisplayBox> discounts { get; set; }
	}
}
