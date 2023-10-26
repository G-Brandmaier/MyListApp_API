using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly AccountController _accountController;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<AccountController>> _logger;

        public AccountControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _logger = new Mock<ILogger<AccountController>>();
            _accountController = new AccountController(_userServiceMock.Object, _logger.Object);
            
        }


        #region update Ghazaleh Test (14 st)


        [Fact]
        public async Task UpdatePassword_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            _accountController.ModelState.AddModelError("key", "error message");

            var updatePasswordDto = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "OldPassword",
                NewPassword = "NewPassword"
            };

            // Act
            var result = await _accountController.UpdatePassword(updatePasswordDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }


        [Fact]
        public async Task UpdatePassword_Should_Return_Ok_When_Password_Is_Updated()
        {
            // Arrange
            var model = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };
            _userServiceMock.Setup(x => x.UpdatePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(okResult.Value);
            Assert.Equal("Password updated successfully", returnValue.Message);
        }

        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_Password_Is_Not_Updated()
        {
            // Arrange
            var model = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };
            _userServiceMock.Setup(x => x.UpdatePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic returnValue = badRequestResult.Value;
            Assert.Equal("Password update failed", returnValue.Message);
        }

        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_Password_Update_Fails()
        {
            // Arrange
            var model = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };
            _userServiceMock.Setup(x => x.UpdatePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }



        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_For_Invalid_New_Password()
        {
            // Arrange
            var model = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "currentPassword",
                NewPassword = "short"
            };

            _userServiceMock.Setup(x => x.UpdatePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }

        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_UserId_Is_Invalid()
        {
            // Arrange
            var model = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };

            _userServiceMock.Setup(x => x.UpdatePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }


        [Fact]
        public async Task UpdatePassword_Should_Return_Ok_When_Password_Is_Successfully_Updated()
        {
            // Arrange
            var model = new UpdatePasswordDto
            {
                UserId = Guid.NewGuid(),
                CurrentPassword = "currentPassword",
                NewPassword = "newPassword"
            };

            _userServiceMock.Setup(x => x.UpdatePassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(okResult.Value);
            Assert.Equal("Password updated successfully", returnValue.Message);
        }


        
        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_New_Password_Is_Same_As_Old_Password()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "currentPassword";
            var newPassword = "currentPassword";
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, currentPassword, newPassword)).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }


        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_User_Does_Not_Exist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = "SomePassword",
                NewPassword = "NewPassword"
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, It.IsAny<string>(), It.IsAny<string>())).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }


        [Fact]
        public async Task UpdatePassword_Should_Return_Ok_When_Credentials_Are_Correct()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "correctPassword";
            var newPassword = "newCorrectPassword";
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, currentPassword, newPassword)).Returns(true);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(okResult.Value);
            Assert.Equal("Password updated successfully", returnValue.Message);
        }

        [Fact]
        public async Task UpdatePassword_Should_Allow_Short_Passwords_If_No_Validation_Is_Performed()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "short";
            var newPassword = "alsoShort";
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = newPassword
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, currentPassword, newPassword)).Returns(true);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(okResult.Value);
            Assert.Equal("Password updated successfully", returnValue.Message);
        }

        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_Current_Password_Is_Incorrect()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var incorrectCurrentPassword = "incorrectPassword";
            var newPassword = "newCorrectPassword";
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = incorrectCurrentPassword,
                NewPassword = newPassword
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, incorrectCurrentPassword, newPassword)).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }

        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_New_Password_Does_Not_Meet_Security_Requirements()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "CurrentPassword123!";
            var invalidNewPassword = "123"; // Anta att detta inte uppfyller säkerhetskraven
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = invalidNewPassword
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, currentPassword, invalidNewPassword)).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }


        [Fact]
        public async Task UpdatePassword_Should_Return_BadRequest_When_New_Password_Is_Too_Similar_To_Old_Password()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var currentPassword = "CurrentPassword123!";
            var tooSimilarNewPassword = "CurrentPassword1234!";
            var model = new UpdatePasswordDto
            {
                UserId = userId,
                CurrentPassword = currentPassword,
                NewPassword = tooSimilarNewPassword
            };

            _userServiceMock.Setup(x => x.UpdatePassword(userId, currentPassword, tooSimilarNewPassword)).Returns(false);

            // Act
            var result = await _accountController.UpdatePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var returnValue = Assert.IsType<UpdatePasswordResponse>(badRequestResult.Value);
            Assert.Equal("Password update failed", returnValue.Message);
        }

        #endregion


        #region Ghazaleh Delete Test (9 st)

        [Fact]    //Test när användare raderas korrekt:
        public async Task DeleteAccount_UserExists_ReturnsOkResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(u => u.DeleteUserAsync(userId)).Returns(true);

            // Act
            var result = await _accountController.DeleteAccount(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var jsonResult = Newtonsoft.Json.Linq.JObject.FromObject(okResult.Value);
            Assert.Equal("Account deleted successfully", jsonResult["Message"].ToString());
        }

        //Test när användaren inte finns och inte kan raderas:

        [Fact]
        public async Task DeleteAccount_UserDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(u => u.DeleteUserAsync(userId)).Returns(false);

            // Act
            var result = await _accountController.DeleteAccount(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var resultString = JsonConvert.SerializeObject(badRequestResult.Value);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultString);

            Assert.NotNull(dictionary);
            Assert.True(dictionary.ContainsKey("Message"));
            Assert.Equal("Account deletion failed", dictionary["Message"]);
        }


        //Testa med en ogiltig användar-ID




        [Fact]
        public async Task DeleteAccount_WithInvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(It.IsAny<Guid>())).Returns(false);

            // Act
            var result = await _accountController.DeleteAccount(invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Lägg till denna rad för att skriva ut vad badRequestResult.Value innehåller
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(badRequestResult.Value));

            // Resten av din Assert-kod kommer här...
        }

        //Testa med ett giltigt användar-ID

      
        [Fact]
        public async Task DeleteAccount_WithValidUserId_ReturnsOk()
        {
            // Arrange
            var validUserId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(validUserId)).Returns(true);

            // Act
            var result = await _accountController.DeleteAccount(validUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }



        //Testa med ett användar-ID som inte finns
        [Fact]
        public async Task DeleteAccount_WithNonexistentUserId_ReturnsBadRequest()
        {
            // Arrange
            var nonexistentUserId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(nonexistentUserId)).Returns(false);

            // Act
            var result = await _accountController.DeleteAccount(nonexistentUserId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        
        // Verifierar att ett undantag propageras upp genom stacken när DeleteUserAsync-metoden i UserService kastar ett undantag.
       

        [Fact]
        public async Task DeleteAccount_WhenServiceThrowsException_ThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(userId)).Throws(new Exception());

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _accountController.DeleteAccount(userId));
        }

        //DeleteAccount när user gör en delete och få ok:

        [Fact]
        public async Task DeleteAccount_WhenUserDeleted_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(userId)).Returns(true);

            // Act
            var result = await _accountController.DeleteAccount(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var resultValue = okResult.Value;
            Assert.NotNull(resultValue);

            var resultDictionary = new RouteValueDictionary(resultValue);
            Assert.True(resultDictionary.ContainsKey("Message"));
            Assert.Equal("Account deleted successfully", resultDictionary["Message"]);
        }


        //Testa när användartjänsten returnerar false
        [Fact]
        public async Task DeleteAccount_WhenUserNotDeleted_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(userId)).Returns(false);

            // Act
            var result = await _accountController.DeleteAccount(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            var resultDictionary = new RouteValueDictionary(badRequestResult.Value);
            Assert.True(resultDictionary.ContainsKey("Message"));
            Assert.Equal("Account deletion failed", resultDictionary["Message"]);
        }


        //Testa när tjänsten returnerar null

        [Fact]
        public async Task DeleteAccount_WhenUserDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userServiceMock.Setup(x => x.DeleteUserAsync(userId)).Returns(false);

            // Act
            var result = await _accountController.DeleteAccount(userId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);

            // Using reflection to access the "Message" property
            var messageProperty = badRequestResult.Value.GetType().GetProperty("Message");
            var messageValue = messageProperty.GetValue(badRequestResult.Value) as string;

            Assert.Equal("Account deletion failed", messageValue);
        }



        //Testa när användar-ID är Guid.Empty

       



    }

    #endregion
}
