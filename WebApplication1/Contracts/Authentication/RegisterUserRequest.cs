﻿namespace WebApplication1.Contracts.Authentication
{
    public record RegisterUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        //public string CreditCardNumber { get; set; }
        //public string Cvc { get; set; }
        //public DateTime ExpiryDate { get; set; }
        public string ConfirmationToken { get; set; }
    }
}
