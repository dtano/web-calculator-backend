namespace WebApplication1.Contracts.Authentication
{
    public record RegisterUserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CreditCardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string JwtToken { get; set; }

        public RegisterUserResponse(Guid id, string name, string email, string creditCardNumber, DateTime expiryDate, string jwtToken)
        {
            Id = id;
            Name = name;
            Email = email;
            CreditCardNumber = creditCardNumber;
            ExpiryDate = expiryDate;
            JwtToken = jwtToken;
        }
    }
}
