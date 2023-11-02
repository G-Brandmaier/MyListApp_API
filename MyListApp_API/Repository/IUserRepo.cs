using MyListApp_API.models;
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
        //Task<User> GetUserByIdAsync(Guid userId);
        //Task<bool> DeleteUserAsync(User user);

        bool UpdateUser(UpdateUserDto updateUserDto);
    }
}
