using Microsoft.Extensions.Logging;
using Moq;
using MyListApp_API.Repository;
using MyListApp_API.Services;
using Xunit;
using MyListApp_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MyListApp_API_Tests.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepo> _userRepoMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;

        public UserServiceTests()
        {
            _userRepoMock = new Mock<IUserRepo>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnFalse_IfUserAlreadyExists()
        {
            // Arrange
            var email = "existingUser@example.com";
            _userRepoMock.Setup(repo => repo.GetUserByEmail(email))
                .Returns(new User { UserName = email, Email = email });

            var userDto = new RegisterUserDto { Email = email };

            // Act
            var result = await _userService.RegisterUserAsync(userDto);

            // Assert
            Assert.False(result);
        }
    }
}
