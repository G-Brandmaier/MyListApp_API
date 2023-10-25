using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Services;
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


    }
}
