using MyListApp_API.Models;

namespace MyListApp_API.Services
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto model); // Interface method for user registration
        Task<User> AuthenticateAsync(string email, string password); // Interface method for user authentication
        
        //Task RegisterUserAsync(RegisterUserDto model);
    }
}
