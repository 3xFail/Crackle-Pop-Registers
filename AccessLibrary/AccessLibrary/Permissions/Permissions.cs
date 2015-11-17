using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSales.Permissions
{
	class Permissions
	{
		public enum RegisterPermissions
		{
			UseRegister = 1,
			ProcessPayment = 2,
			PriceOverride = 4,
			ApplyCoupon = 8,
		};
	}
}
