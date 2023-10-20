using Microsoft.AspNetCore.Identity;
using MyListApp_API.Models;
using System.Threading.Tasks;

namespace MyListApp_API.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;          // User manager for managing user data
            _signInManager = signInManager;      // Sign-in manager for handling user sign-ins
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto model)
        {
            var user = new User { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            return result.Succeeded;             // Returns true if user registration is successful
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, false, false);

            if (result.Succeeded)
            {
                return await _userManager.FindByEmailAsync(email); // Returns the user if authentication is successful
            }

            return null; // Returns null if authentication fails
        }
    }
}

