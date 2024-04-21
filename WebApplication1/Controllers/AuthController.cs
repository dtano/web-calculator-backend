using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Authentication;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpPost("/register")]
        public IActionResult Register(RegisterUserRequest request)
        {
            var newUser = new User(
                Guid.NewGuid(),
                request.Name,
                request.Email,
                request.Password, // Password needs to be encrypted
                request.CreditCardNumber,
                request.ExpiryDate, // Convert string to date time
                DateTime.UtcNow,
                DateTime.UtcNow
            );

            var response = new RegisterUserResponse(
                newUser.Id,
                newUser.Name,
                newUser.Email,
                newUser.Password,
                newUser.CreditCardNumber,
                newUser.ExpiryDate
            );

            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            return Ok(request);
        }
    }
}
