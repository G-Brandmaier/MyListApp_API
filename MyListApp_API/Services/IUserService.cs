using MyListApp_API.models;
using MyListApp_API.Models;

namespace MyListApp_API.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto model); // Interface method for user registration
        Task<string> AuthenticateAsync(string email, string password); // Interface method for user authentication
        User GetUserById(Guid userId);
        User GetUserByEmail(string email);
        void AddUser(User user);

        IEnumerable<User> GetAllUsers();
        bool UpdatePassword(Guid userId, string currentPassword, string newPassword);

        bool DeleteUserAsync(Guid userId);
        bool UpdateUserDetails(UpdateUserDto updateUserDto);
    }
}
