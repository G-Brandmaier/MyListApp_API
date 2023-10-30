using MyListApp_API.Models;

namespace MyListApp_API_Tests.UnitTests.Models;

public class ListItemDtoTests
{
    private readonly ListItemDto _listItemDto;

    public ListItemDtoTests()
    {
        _listItemDto = new ListItemDto();
    }

    #region Gabriella Testar 3st

    #region Testar metoden CheckValidAmountOfCharactersForContent
    [Fact]
    public void CheckValidAmountOfCharactersForContent_ShouldCheckValidLengthOfContent_ReturnTrue()
    {
        //Arrange
        string validContentLength = "Att göra";

        //Act
        var result = _listItemDto.CheckValidAmountOfCharactersForContent(validContentLength);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForContent_ShouldCheckInvalidLengthOfContent_ReturnFalse()
    {
        //Arrange
        char[] fixedSizeString = new char[85];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        string invalidContentLength = new string(fixedSizeString);

        //Act
        var result = _listItemDto.CheckValidAmountOfCharactersForContent(invalidContentLength);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForContent_ReceivesEmptyString_ReturnFalse()
    {
        //Arrange
        string invalidContentLength = string.Empty;

        //Act
        var result = _listItemDto.CheckValidAmountOfCharactersForContent(invalidContentLength);

        //Assert
        Assert.False(result);
    }
    #endregion

    #endregion
}
