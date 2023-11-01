using MyListApp_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.UnitTests.Models
{
   
    public class UpdatePasswordDtoTest
    {  
        private readonly UpdatePasswordDto _updatePasswordDto;

        public UpdatePasswordDtoTest(UpdatePasswordDto updatePasswordDto)
        {
            _updatePasswordDto = updatePasswordDto;
        }


        #region Ghazaleh UpdatePasswordDtoTest ( 5 st)
        public class UpdatePasswordDtoTests
        {
            [Fact]
            public void CheckPasswordStrength_ShouldReturnTrueForStrongPassword()
            {
                // Arrange
                var updatePasswordDto = new UpdatePasswordDto
                {
                    UserId = Guid.NewGuid(),
                    CurrentPassword = "OldPassword!123",
                    NewPassword = "NewStrongPassword1"
                };

                // Act
                var result = updatePasswordDto.CheckPasswordStrength();

                // Assert
                Assert.True(result);
            }

            [Fact]
            public void CheckPasswordStrength_ShouldReturnFalseForWeakPassword()
            {
                // Arrange
                var updatePasswordDto = new UpdatePasswordDto
                {
                    UserId = Guid.NewGuid(),
                    CurrentPassword = "OldPassword!123",
                    NewPassword = "weak"
                };

                // Act
                var result = updatePasswordDto.CheckPasswordStrength();

                // Assert
                Assert.False(result);
            }

            [Fact]
            public void ConfirmPasswordChange_ShouldReturnFalseIfNewPasswordIsSameAsCurrent()
            {
                // Arrange
                var password = "SamePassword123";
                var updatePasswordDto = new UpdatePasswordDto
                {
                    UserId = Guid.NewGuid(),
                    CurrentPassword = password,
                    NewPassword = password
                };

                // Act
                var result = updatePasswordDto.ConfirmPasswordChange();

                // Assert
                Assert.False(result);
            }



            [Fact]
            public void ConfirmPasswordChange_ShouldReturnTrueIfNewPasswordIsDifferent()
            {
                // Arrange
                var updatePasswordDto = new UpdatePasswordDto
                {
                    UserId = Guid.NewGuid(),
                    CurrentPassword = "OldPassword123",
                    NewPassword = "NewPassword123"
                };

                // Act
                var result = updatePasswordDto.ConfirmPasswordChange();

                // Assert
                Assert.True(result);
            }


            [Theory]
            [InlineData("Short1", false)] // För kort, men har en siffra
            [InlineData("LongEnoughButNoNumber", false)] // Tillräckligt långt men saknar siffra
            [InlineData("GoodPassword1", true)] // Tillräckligt långt och har en siffra
            public void CheckPasswordStrength_ShouldValidateBasedOnLengthAndDigits(string newPassword, bool expectedIsValid)
            {
                // Arrange
                var updatePasswordDto = new UpdatePasswordDto
                {
                    UserId = Guid.NewGuid(),
                    CurrentPassword = "OldPassword123",
                    NewPassword = newPassword
                };

                // Act
                var result = updatePasswordDto.CheckPasswordStrength();

                // Assert
                Assert.Equal(expectedIsValid, result);
            }

        }

        #endregion

    }
}
