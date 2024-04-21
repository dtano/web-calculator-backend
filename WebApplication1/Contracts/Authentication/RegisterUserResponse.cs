namespace WebApplication1.Contracts.Authentication
{
    public record RegisterUserResponse
    {
        public Guid id;
        public string Name;
        public string Email;
        public string Password;
        public string CreditCardNumber;
        public DateTime ExpiryDate;

        public RegisterUserResponse(Guid id, string name, string email, string password, string creditCardNumber, DateTime expiryDate)
        {
            this.id = id;
            Name = name;
            Email = email;
            Password = password;
            CreditCardNumber = creditCardNumber;
            ExpiryDate = expiryDate;
        }
    }
}
