using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IUserRepository
    {
        User FindUserByEmail(string email);
        void Save(User user);
    }
}
