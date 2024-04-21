using WebApplication1.Contracts.Authentication;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        RegisterUserResponse CreateUser(RegisterUserRequest req);
        LoginResponse Login(LoginRequest req);
    }
}
