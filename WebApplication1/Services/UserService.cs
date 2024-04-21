using WebApplication1.Contracts.Authentication;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Utils;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public RegisterUserResponse CreateUser(RegisterUserRequest req)
        {
            // Validate here
            ValidateRegisterUserRequest(req);

            // Now create user
            var encryptedPassword = EncryptionUtils.Encrypt(req.Password);
            var newUser = new User(
                Guid.NewGuid(),
                req.Name,
                req.Email,
                encryptedPassword,
                req.CreditCardNumber.Substring(req.CreditCardNumber.Length - 4), // Only save the last 4 characters
                req.ExpiryDate, // Convert string to date time
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            // Attempt to make stripe payment

            // If successful, then save to database
            _userRepository.Save(newUser);

            var response = new RegisterUserResponse(
                newUser.Id,
                newUser.Name,
                newUser.Email,
                newUser.Password,
                newUser.CreditCardNumber,
                newUser.ExpiryDate
            );
            
            return response;
        }

        private void ValidateRegisterUserRequest(RegisterUserRequest req)
        {
            // Check if a user with the given email already exists
            User existingUser = _userRepository.FindUserByEmail(req.Email);
            if(existingUser != null)
            {
                throw new Exception("A user with the given email already exists");
            }


            // Check credit card details here?

            // Check length of credit card number
            if(req.CreditCardNumber.Length != 16)
            {
                throw new Exception("Credit card number should be 16 characters long");
            }

            // Check CVC of credit cad
            if(req.Cvc.Length != 3)
            {
                throw new Exception("CVC should comprise of 3 numbers");
            }

            // Check if card is already expired
            if(DateTime.UtcNow > req.ExpiryDate.ToUniversalTime())
            {
                throw new Exception("Given credit card is already expired");
            }
        }

        public LoginResponse Login(LoginRequest req)
        {
            throw new NotImplementedException();
        }
    }
}
