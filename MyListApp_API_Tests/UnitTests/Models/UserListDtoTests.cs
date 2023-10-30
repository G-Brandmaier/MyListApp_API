using MyListApp_API.Models;

namespace MyListApp_API_Tests.UnitTests.Models;

public class UserListDtoTests
{
    private readonly UserListDto _userListDto;

    public UserListDtoTests()
    {
        _userListDto = new UserListDto();
    }

    #region Gabriella Testar 3st

    #region Testar metoden CheckValidAmountOfCharactersForTitle
    [Fact]
    public void CheckValidAmountOfCharactersForTitle_ShouldCheckValidLengthOfTitle_ReturnTrue()
    {
        //Arrange
        string validTitleLength = "Att göra";

        //Act
        var result = _userListDto.CheckValidAmountOfCharactersForTitle(validTitleLength);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForTitle_ShouldCheckInvalidLengthOfTitle_ReturnFalse()
    {
        //Arrange
        char[] fixedSizeString = new char[30];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        string invalidTitleLength = new string(fixedSizeString);

        //Act
        var result = _userListDto.CheckValidAmountOfCharactersForTitle(invalidTitleLength);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForTitle_ReceivesEmptyString_ReturnFalse()
    {
        //Arrange
        string invalidTitleLength = string.Empty;

        //Act
        var result = _userListDto.CheckValidAmountOfCharactersForTitle(invalidTitleLength);

        //Assert
        Assert.False(result);
    }
    #endregion

    #endregion
}
