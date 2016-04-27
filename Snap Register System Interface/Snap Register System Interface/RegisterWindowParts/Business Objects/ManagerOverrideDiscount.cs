using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapRegisters
{
	class ManagerOverrideDiscount : IDiscount
	{
		public ManagerOverrideDiscount(decimal amount)
		{
			Amount = amount;
		}
		public decimal ChangeAmountTo(decimal amt)
		{
			return Math.Max(amt - Amount, 0);
        }
		public override string ToString()
		{
			return "Override Discount";
		}

		public decimal Discount()
		{
			return Amount;
		}

		public bool IsFlat()
		{
			return true;
		}

		public decimal Amount { get; set; } = 0M;
	}
}
