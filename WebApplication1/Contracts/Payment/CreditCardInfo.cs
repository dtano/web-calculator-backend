namespace WebApplication1.Contracts.Payment
{
    public class CreditCardInfo
    {
        public string Number { get; set; }
        public DateTime ExpiryDate { get; set; }

        public CreditCardInfo(string number, DateTime expiryDate)
        {
            Number = number;
            ExpiryDate = expiryDate;
        }
    }
}
