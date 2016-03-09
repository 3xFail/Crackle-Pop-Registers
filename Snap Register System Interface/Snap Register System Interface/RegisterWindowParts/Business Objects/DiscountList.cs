using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapRegisters
{
    public class DiscountList: IEnumerable<IDiscount>
    {
        public void ApplyTo( Item item )
        {
            foreach( Sale sale in Discounts ) //First apply all flat discounts
                if( sale.Flat )
                    item.Apply( sale );

            foreach( Sale sale in Discounts ) //Then apply all percentage discounts
                if( !sale.Flat )
                    item.Apply( sale );
        }

        public void Add( IDiscount discount )
        {
            Discounts.Add( discount );
        }

        public IEnumerator<IDiscount> GetEnumerator()
        {
            return Discounts.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Discounts.GetEnumerator();
        }

        private List<IDiscount> Discounts { get; set; } = new List<IDiscount>();
    }
}
