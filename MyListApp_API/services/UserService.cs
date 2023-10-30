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
            var existingUser = GetUserByEmail(model.Email);
            if (existingUser != null)
            {
                return false; // Registered user
            }

            // Add users to the repository
            var newUser = new User { UserName = model.Email, Email = model.Email };
            AddUser(newUser);


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
                var user = GetUserByEmail(email);

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

        public User GetUserById(Guid userId)
        {

            return _userRepo._users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByEmail(string email)
        {
            return _userRepo._users.FirstOrDefault(x => x.Email == email);
        }

        public void AddUser(User user)
        {
            user.Id = Guid.NewGuid();
            _userRepo._users.Add(user);
        }



        public bool UpdatePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = GetUserById(userId); // Ddet ska använda Repo istället _usermanger*******
            if (user == null) return false;

            user.Password = newPassword;
            return true;
            //var result = await _userRepo.ChangePasswordAsync(user, currentPassword, newPassword);
            //return result.Succeeded;
        }

        public bool DeleteUserAsync(Guid userId)
        {
            var user = GetUserById(userId);//***** samma uppe
            if (user == null) return false;
            _userRepo._users.Remove(user);
            return true;
        }



    }
}

