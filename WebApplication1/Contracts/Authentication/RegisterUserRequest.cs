namespace WebApplication1.Contracts.Authentication
{
    public record RegisterUserRequest
    {
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public string CreditCardNumber { get; }
        public DateTime ExpiryDate { get; }
    }
}
