using Microsoft.AspNetCore.Identity;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using System.Threading.Tasks;

namespace MyListApp_API.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepo _userRepo;
        private readonly ILogger<UserService> _logger;

        public UserService(UserRepo userRepo, ILogger<UserService> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto model)
        {
            // Check whether the user is registered
            var existingUser = _userRepo.GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                return false; // Registered user
            }

            // Add users to the repository
            var newUser = new User { UserName = model.Email, Email = model.Email };
            _userRepo.AddUser(newUser);


            //Hard-code the password for example purposes
            var password = "ExempleHardCodedPassword";


            //return result.Succeeded;
            return true;
        }

        public async Task<string> AuthenticateAsync(string email, string password)
        {
            try
            {
                // Search for users in the repository
                var user = _userRepo.GetUserByEmail(email);

                if (user != null)
                {

                    return user.Id.ToString();
                }

                return null;
            }
            catch (Exception ex)
            {
                // An error Log
                _logger.LogError(ex, "An error occurred during user authentication.");
                throw; // Continue throwing exceptions after logging
            }

        }

    }
}

