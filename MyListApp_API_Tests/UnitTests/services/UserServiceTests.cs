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

        #region Ghazaleh Update UserServiceTest( 8 st )

        //klassen fungerar korrekt när en användare med ett giltigt ID försöker uppdatera sitt lösenord.

        [Fact]
        public void UpdatePassword_ShouldReturnTrue_WhenUserExistsAndPasswordIsUpdated()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "oldPassword";
            var newPassword = "newPassword";
            var user = new User { Id = userId, Password = currentPassword };

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, newPassword);

            // Assert
            Assert.True(result);
            Assert.Equal(newPassword, user.Password);
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }


        //klassen för att säkerställa att den hanterar situationer där användaren inte finns korrekt.

        [Fact]
        public void UpdatePassword_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "oldPassword";
            var newPassword = "newPassword";

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns((User)null);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, newPassword);

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }

        // Test när det nya lösenordet är samma som det gamla

        [Fact]
        public void UpdatePassword_ShouldReturnTrue_WhenNewPasswordIsSameAsOldPassword()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "password123";
            var user = new User { Id = userId, Password = currentPassword };

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, currentPassword);

            // Assert
            Assert.True(result);
            Assert.Equal(currentPassword, user.Password);
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }


        //Test när det nya lösenordet är tomt

        [Fact]
        public void UpdatePassword_ShouldReturnTrue_WhenNewPasswordIsEmpty()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "password123";
            var user = new User { Id = userId, Password = currentPassword };

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, "");

            // Assert
            Assert.True(result);
            Assert.Equal("", user.Password);
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }


        //Test när det nuvarande lösenordet är felaktigt
  
        [Fact]
        public void UpdatePassword_ShouldReturnTrue_EvenWhenCurrentPasswordIsIncorrect()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "password123";
            var incorrectPassword = "wrongPassword";
            var newPassword = "newPassword123";
            var user = new User { Id = userId, Password = currentPassword };

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.UpdatePassword(userId, incorrectPassword, newPassword);

            // Assert
            Assert.True(result, "UpdatePassword should return true even when the current password is incorrect");
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
            // You might also want to verify that the user's password has been updated.
            Assert.Equal(newPassword, user.Password);
        }


        //Test när användar-ID är ogiltigt

        [Fact]
        public void UpdatePassword_ShouldReturnFalse_WhenUserIdIsInvalid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "password123";
            var newPassword = "newPassword123";

            _userRepoMock.Setup(repo => repo.GetUserById(It.IsAny<Guid>())).Returns((User)null);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, newPassword);

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }

        //Test när det nya lösenordet är för kort eller inte uppfyller vissa kriterier

        [Theory]
        [InlineData("short")]
        [InlineData("simple")]
        public void UpdatePassword_ShouldReturnTrue_WhenNewPasswordDoesNotMeetCriteria(string newPassword)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "password123";
            var user = new User { Id = userId, Password = currentPassword };

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, newPassword);

            // Assert
            Assert.True(result, "UpdatePassword should return true even when the new password does not meet criteria");
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }


        //Test för att verifiera att UpdatePassword uppdaterar lösenordet och returnerar true när allt är korrekt:

        [Fact]
        public void UpdatePassword_ShouldReturnTrueAndUpdatePassword_WhenAllConditionsAreMet()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "password123";
            var newPassword = "newStrongPassword123";
            var user = new User { Id = userId, Password = currentPassword };

            _userRepoMock.Setup(repo => repo.GetUserById(userId)).Returns(user);

            // Act
            var result = _userService.UpdatePassword(userId, currentPassword, newPassword);

            // Assert
            Assert.True(result, "UpdatePassword should return true and update password when all conditions are met");
            Assert.Equal(newPassword, user.Password);
            _userRepoMock.Verify(repo => repo.GetUserById(userId), Times.Once);
        }





        #endregion

        #region Ghazaleh Delete UserServiceTest ( 8 st )

        /// Det här testet säkerställer att DeleteUserAsync fungerar korrekt när en existerande användare försöker raderas.


        [Fact]
        public void DeleteUserAsync_UserExists_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId };
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _userRepoMock.Setup(r => r.DeleteUser(user)).Returns(true);

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(user), Times.Once);
        }


        /// Det här testet säkerställer att DeleteUserAsync hanterar situationer korrekt när en icke-existerande användare försöker raderas.

        [Fact]
        public void DeleteUserAsync_UserDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns((User)null);

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never);
        }



        //när borttagningen av en användare misslyckas
        [Fact]
        public void DeleteUserAsync_DeleteFails_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId };
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _userRepoMock.Setup(r => r.DeleteUser(user)).Returns(false);

            // Act
            var result = _userService.DeleteUserAsync(userId); // Notera: inget 'await' här

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(user), Times.Once);
        }


        //när ett undantag uppstår under borttagning av en användare.
        [Fact]
        public void DeleteUserAsync_ThrowsException_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId };
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _userRepoMock.Setup(r => r.DeleteUser(user)).Throws(new Exception("Simulated exception"));

            bool result = false;
            Exception caughtException = null;

            // Act
            try
            {
                result = _userService.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert
            Assert.False(result);
            Assert.IsType<Exception>(caughtException);
            Assert.Equal("Simulated exception", caughtException.Message);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(user), Times.Once);
        }


        //DeleteUserAsync_UserIsNull_ShouldReturnFalse

        [Fact]
        public void DeleteUserAsync_UserIsNull_ShouldReturnFalse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns((User)null); // Ingen användare finns med det ID:t

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never); // Kontrollera att DeleteUser aldrig anropas
        }

        //DeleteUserAsync_UserIsDeleted_ShouldReturnTrue

        [Fact]
        public void DeleteUserAsync_UserIsDeleted_ShouldReturnTrue()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId };
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _userRepoMock.Setup(r => r.DeleteUser(user)).Returns(true);

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(user), Times.Once);
        }

        // Om en användare inte finns i databasen, så ska inte DeleteUser-metoden på repo-objektet anropas

        [Fact]
        public void DeleteUserAsync_UserDoesNotExist_ShouldNotCallDeleteUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns((User)null);

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
            _userRepoMock.Verify(r => r.GetUserById(userId), Times.Once);
            _userRepoMock.Verify(r => r.DeleteUser(It.IsAny<User>()), Times.Never);
        }

        //Detta test säkerställer att inget fel loggas om DeleteUser-metoden returnerar false

        [Fact]
        public void DeleteUserAsync_DeleteFails_ShouldNotLogError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User { UserId = userId };
            _userRepoMock.Setup(r => r.GetUserById(userId)).Returns(user);
            _userRepoMock.Setup(r => r.DeleteUser(user)).Returns(false);

            // Act
            var result = _userService.DeleteUserAsync(userId);

            // Assert
            Assert.False(result);
            _loggerMock.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Never);
        }



        #endregion
    }
}
