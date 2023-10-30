using MyListApp_API.Models;

namespace MyListApp_API.Repository
{
    public interface IUserRepo
    {
        User GetUserById(Guid userId);
        User GetUserByEmail(string email);
        void AddUser(User user);
        IList<User> GetAllUsers();

        bool DeleteUser(User user);
    }
}
