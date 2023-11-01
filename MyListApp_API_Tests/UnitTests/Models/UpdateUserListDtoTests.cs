using MyListApp_API.Models;

namespace MyListApp_API_Tests.UnitTests.Models;

public class UpdateUserListDtoTests
{
    private readonly UpdateUserListDto _updateUserListDto;

    public UpdateUserListDtoTests()
    {
        _updateUserListDto = new UpdateUserListDto();
    }

    #region Gabriella Testar 3st

    #region Testar metoden CheckValidAmountOfCharactersForTitle

    [Fact]
    public void CheckValidAmountOfCharactersForTitle_ShouldCheckValidStringLength_ReturnTrue()
    {
        //Arrange
        string validContentLength = "Att göra";

        //Act
        var result = _updateUserListDto.CheckValidAmountOfCharactersForTitle(validContentLength);

        //Assert
        Assert.True(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForTitle_ShouldCheckInvalidStringLength_ReturnFalse()
    {
        //Arrange
        char[] fixedSizeString = new char[29];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        string invalidContentLength = new string(fixedSizeString);

        //Act
        var result = _updateUserListDto.CheckValidAmountOfCharactersForTitle(invalidContentLength);

        //Assert
        Assert.False(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForTitle_ShouldCheckEmptyString_ReturnFalse()
    {
        //Arrange
        string validContentLength = "";

        //Act
        var result = _updateUserListDto.CheckValidAmountOfCharactersForTitle(validContentLength);

        //Assert
        Assert.False(result);
        Assert.IsType<bool>(result);
    }
    #endregion

    #endregion
}
