﻿using MyListApp_API.Models;
using System.Text;
using System.Security.Cryptography;

namespace MyListApp_API.Repository
{
    public class UserRepo : IUserRepo
    {
        public List<User> _users = new List<User>();
        private object _context;

        public UserRepo()
        {
            // Hard-code tidiga users
            _users.Add(new User { Id = Guid.NewGuid(), UserName = "user1@example.com", Email = "user1@example.com", Password = "Password1" });
            _users.Add(new User { Id = Guid.NewGuid(), UserName = "user2@example.com", Email = "user2@example.com", Password = "Password2" });
        }

        public User GetUserById(Guid userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }

        public IList<User> GetAllUsers()
        {
            return _users;
        }



        public bool DeleteUser(User user)
        {
            if (_users.Contains(user))
            {
                _users.Remove(user);
                return true;
            }
            return false;
        }

    }
}
