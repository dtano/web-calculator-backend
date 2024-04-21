namespace WebApplication1.Models
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Email { get; }
        public string Password { get; }
        public string CreditCardNumber { get; }
        public DateTime ExpiryDate { get; }
        public DateTime CreatedDate { get; }
        public DateTime LastModifiedDate { get; }

        public User(
            Guid id, 
            string name, 
            string email, 
            string password, 
            string creditCardNumber, 
            DateTime expiryDate, 
            DateTime createdDate, 
            DateTime lastModifiedDate)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            CreditCardNumber = creditCardNumber;
            ExpiryDate = expiryDate;
            CreatedDate = createdDate;
            LastModifiedDate = lastModifiedDate;
        }
    }
}
