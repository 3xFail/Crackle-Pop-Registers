using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSales.Payment
{
	abstract class Payment
	{
		public Payment()
		{
			amount = 0;
		}

		public Payment(float Amount)
		{
			amount = Amount;
		}

		// property for the amount.
		public float amount { get { return m_amount; } set { m_amount = value; } }
		private float m_amount;
	}
}
