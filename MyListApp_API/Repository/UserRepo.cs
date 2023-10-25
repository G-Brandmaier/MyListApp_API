using MyListApp_API.Models;

namespace MyListApp_API.Repository
{
    public class UserRepo
    {
        public List<User> _users = new List<User>();

        public UserRepo()
        {
            // Hard-code tidiga users
            _users.Add(new User { Id = Guid.NewGuid(), UserName = "user1@example.com", Email = "user1@example.com", Password = "Password1"});
            _users.Add(new User { Id = Guid.NewGuid(), UserName = "user2@example.com", Email = "user2@example.com", Password = "Password2" });
        }

    }
}
