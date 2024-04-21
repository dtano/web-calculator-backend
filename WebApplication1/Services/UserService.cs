using WebApplication1.Contracts.Authentication;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Utils;
using System.Web;
using WebApplication1.Contracts.Email;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public UserService(IUserRepository userRepository, IMailService mailService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
        }

        public RegisterUserResponse CreateUser(RegisterUserRequest req)
        {
            // Validate here
            ValidateRegisterUserRequest(req);

            // Now create user
            var password = RandomUtils.GenerateRandomPassword();
            var encryptedPassword = EncryptionUtils.Encrypt(password);
            
            
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

            // Send email to user
            _mailService.SendMail(CreateWelcomeEmailData(req.Email, req.Name, password));

            var response = new RegisterUserResponse(
                newUser.Id,
                newUser.Name,
                newUser.Email,
                newUser.CreditCardNumber,
                newUser.ExpiryDate
            );
            
            return response;
        }

        private MailData CreateWelcomeEmailData(string targetEmail, string userName, string password)
        {
            MailData mailData = new MailData();
            mailData.ReceiverEmail = targetEmail;
            mailData.ReceiverName = userName;
            mailData.EmailSubject = "Welcome to the Premium Program";
            mailData.EmailBody = $"We would like to welcome you to Web Calculator Premium!\n This is your login password: {password}.\n\n Regards,\nWeb Calculator Team";

            return mailData;
        }   

        private void ValidateRegisterUserRequest(RegisterUserRequest req)
        {
            // Check if a user with the given email already exists
            User existingUser = _userRepository.FindUserByEmail(req.Email);
            if(existingUser != null)
            {
                throw new Exception("A user with the given email already exists");
            }


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
