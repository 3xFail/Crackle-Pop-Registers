using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PointOfSales.Payment
{
	// Storage class for a credit card payment. Inherits from payment.
	public class CreditCardPayment : Payment
	{
		public CreditCardPayment(string CardNumber, DateTime ExpirationDate, string ZipCode, float Amount)
			: base(Amount)
		{
			cardNumber = CardNumber;
			expirationDate = ExpirationDate;
			zipCode = ZipCode;
		}

		public string cardNumber { get { return m_cardNumber; } set { m_cardNumber = value; } }
		private string m_cardNumber;

		public DateTime expirationDate {  get { return m_expirationDate; } set { m_expirationDate = value; } }
		private DateTime m_expirationDate;

		public string zipCode { get { return m_zipCode; } set { m_zipCode = value; } }
		private string m_zipCode;
	}
}
