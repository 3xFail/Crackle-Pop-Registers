using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSales.Payment;

namespace AccessLibraryTest
{
	[TestClass]
	public class CreditCardPaymentTest
	{
		[TestMethod]
		public void PayWithCreditCard()
		{
			string CardNumber = "1111-1111-1111-1111";
			// Day is irrelevant.
			DateTime ExiprationDate = new DateTime(1, 1, 1);
			string ZipCode = "12345";
			float Amount = 10.55F;
			
			CreditCardPayment payment = new CreditCardPayment(CardNumber, ExiprationDate, ZipCode, Amount);

			Assert.AreEqual(CardNumber, payment.cardNumber);
			Assert.AreEqual(ExiprationDate, payment.expirationDate);
			Assert.AreEqual(ZipCode, payment.zipCode);
			Assert.AreEqual(Amount, payment.amount);
		}

		[TestMethod]
		public void AssignProperties()
		{
			string CardNumber = "1111-1111-1111-1111";
			// Day is irrelevant.
			DateTime ExiprationDate = new DateTime(1, 1, 1);
			string ZipCode = "12345";
			float Amount = 10.55F;

			CreditCardPayment payment = new CreditCardPayment(CardNumber, ExiprationDate, ZipCode, Amount);

			float ChangeInPrice = 5.0F;

			payment.amount += ChangeInPrice;

			Assert.AreEqual(Amount + ChangeInPrice, payment.amount);
		}
	}
}
