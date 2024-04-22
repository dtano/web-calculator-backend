using WebApplication1.Contracts.Authentication;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Utils;
using System.Web;
using WebApplication1.Contracts.Email;
using Stripe;
using WebApplication1.Contracts.Payment;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IPaymentService _paymentService;

        public UserService(IUserRepository userRepository, IMailService mailService, IPaymentService paymentService)
        {
            _userRepository = userRepository;
            _mailService = mailService;
            _paymentService = paymentService;
        }

        public RegisterUserResponse CreateUser(RegisterUserRequest req)
        {
            // Validate here
            ValidateRegisterUserRequest(req);

            CreditCardInfo creditCardInfo = _paymentService.ExtractCreditCardInfoFromToken(req.ConfirmationToken);

            // Attempt to make stripe payment
            // If unsuccessful then stop
            PaymentResponse paymentResponse = _paymentService.MakePayment(req.ConfirmationToken);

            // Now create user
            var password = RandomUtils.GenerateRandomPassword();
            var encryptedPassword = EncryptionUtils.Encrypt(password);

            var newUser = new User(
                Guid.NewGuid(),
                req.Name,
                req.Email,
                encryptedPassword,
                creditCardInfo.Number,
                creditCardInfo.ExpiryDate,
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            // Send email to user
            _mailService.SendMail(CreateWelcomeEmailData(req.Email, req.Name, password));

            // If successful, then save to database
            _userRepository.Save(newUser);

            // Create login token
            var jwtToken = AuthUtils.GenerateJwtToken(req.Email);

            var response = new RegisterUserResponse(
                newUser.Id,
                newUser.Name,
                newUser.Email,
                newUser.CreditCardNumber,
                newUser.ExpiryDate,
                jwtToken
            );
            
            return response;
        }

        private MailData CreateWelcomeEmailData(string targetEmail, string userName, string password)
        {
            MailData mailData = new MailData();
            mailData.ReceiverEmail = targetEmail;
            mailData.ReceiverName = userName;
            mailData.EmailSubject = "Welcome to the Premium Program";
            mailData.EmailBody = $"We would like to welcome you to Web Calculator Premium!\n This is your login password: {password}\n\n Regards,\nWeb Calculator Team";

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
        }

        private void ValidateLoginRequest(LoginRequest req)
        {
            if(req.Email == null || req.Email.Length == 0)
            {
                throw new Exception("Email field cannot be empty");
            }

            if (req.Password == null || req.Password.Length == 0)
            {
                throw new Exception("Password field cannot be empty");
            }

            User existingUser = _userRepository.FindUserByEmail(req.Email);
            if(existingUser == null)
            {
                throw new Exception($"User with the given email does not exist");
            }

            // Now compare the passwords
            var decryptedPassword = EncryptionUtils.Decrypt(existingUser.Password);
            if(req.Password != decryptedPassword)
            {
                throw new Exception("Wrong password. Please try again");
            }
        }

        public LoginResponse Login(LoginRequest req)
        {
            // Make sure email and password are not empty
            ValidateLoginRequest(req);

            string jwtToken = AuthUtils.GenerateJwtToken(req.Email);
            LoginResponse response = new LoginResponse
            {
                Token = jwtToken
            };

            return response;
        }
    }
}
