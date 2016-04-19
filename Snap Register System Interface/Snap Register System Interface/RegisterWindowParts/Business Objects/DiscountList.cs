using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapRegisters
{
    //This class is a container class for a List of IDiscount objects.
    //It impliments foreach loop behavior with the IEnumerable interface
    //Every Item object holds a reference 
    public class DiscountList: IEnumerable<IDiscount>
    {
        //Every Item holds a list of Discounts. So what we do here is take in an Item ( parent obj passes in 'this' )
        //  and then recalculate the price of the item by applying all the discounts in a specific order.
        //This function first resets the price of the item to its original price so you can't keep applying the same discount list to an item 
        public void ApplyTo( Item item )
        {
            item.Price = item.OriginalPrice;
            foreach( IDiscount discount in Discounts ) //First apply all flat discounts
                if( discount.IsFlat() )
                    item.Price = discount.ChangeAmountTo( item.Price );

            foreach( IDiscount discount in Discounts ) //Then apply all percentage discounts
                if( !discount.IsFlat() )
                    item.Price = discount.ChangeAmountTo( item.Price );
        }

        //Put item in discount list, this also recalculates the item price.
        public void Add( IDiscount discount )
        {
            Discounts.Add( discount );
        }

		public void Remove(IDiscount discount)
		{
			Discounts.Remove(discount);
		}

		public IDiscount Find(Predicate<IDiscount> match)
		{
			return Discounts.Find(match);
		}
        //Overload so we can foreach
        public IEnumerator<IDiscount> GetEnumerator()
        {
            return Discounts.GetEnumerator();
        }

        //Overload so we can foreach
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Discounts.GetEnumerator();
        }

        private List<IDiscount> Discounts { get; set; } = new List<IDiscount>();
    }
}
