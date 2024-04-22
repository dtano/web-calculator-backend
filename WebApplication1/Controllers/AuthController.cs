using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Contracts.Authentication;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterUserRequest request)
        {
            try
            {
                var response = _userService.CreateUser(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var response = _userService.Login(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
