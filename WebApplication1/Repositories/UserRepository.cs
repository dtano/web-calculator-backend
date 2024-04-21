using Newtonsoft.Json;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string FileName = "Users.json";

        // Have a reference to the json file that will hold all the user info
        public User FindUserByEmail(string email)
        {
            string userDatabaseJson = File.ReadAllText(FileName);

            var userDatabase = JsonConvert.DeserializeObject<List<User>>(userDatabaseJson);
            if (userDatabase == null) return null;

            foreach(User user in userDatabase)
            {
                if(user.Email == email)
                {
                    return user;
                }
            }

            return null;
        }

        public void Save(User user)
        {
            if (user == null) return;

            string userDatabaseJson = File.ReadAllText(FileName);

            var userDatabase = JsonConvert.DeserializeObject<List<User>>(userDatabaseJson);
            if(userDatabase == null)
            {
                userDatabase = new List<User>();
            }

            userDatabase.Add(user);
            var updatedDatabase = JsonConvert.SerializeObject(userDatabase, Formatting.Indented);

            File.WriteAllText(FileName, updatedDatabase);
        }
    }
}
