using MyListApp_API.Models;

namespace MyListApp_API_Tests.UnitTests.Models;

public class UpdateListItemDtoTests
{
    private readonly UpdateListItemDto _updateListItemDto;

    public UpdateListItemDtoTests()
    {
        _updateListItemDto = new UpdateListItemDto();
    }

    #region Gabriella testar 7st

    #region Testar metoden CheckValidAmountOfCharactersForNewContent
    [Fact]
    public void CheckValidAmountOfCharactersForNewContent_ShouldCheckValidStringLength_ReturnTrue()
    {
        //Arrange
        string validContentLength = "Hämpta paket från postombud";

        //Act
        var result = _updateListItemDto.CheckValidAmountOfCharactersForNewContent(validContentLength);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForNewContent_ShouldCheckInvalidStringLength_ReturnFalse()
    {
        //Arrange
        char[] fixedSizeString = new char[85];
        for (int i = 0; i < fixedSizeString.Length; i++)
        {
            fixedSizeString[i] = 'A';
        }
        string invalidContentLength = new string(fixedSizeString);

        //Act
        var result = _updateListItemDto.CheckValidAmountOfCharactersForNewContent(invalidContentLength);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckValidAmountOfCharactersForNewContent_ReceivesEmptyString_ReturnFalse()
    {
        //Arrange
        string emptyContent = string.Empty;

        //Act
        var result = _updateListItemDto.CheckValidAmountOfCharactersForNewContent(emptyContent);

        //Assert
        Assert.False(result);
    }
    #endregion

    #region Testar metoden CheckValidContentPosition
    [Fact]
    public void CheckValidContentPosition_ShouldCheckValidPosition_ReturnTrue()
    {
        //Arrange
        int position = 10;
        int existingListPositions = 14;
        //Act
        var result = _updateListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckInvalidPositionZero_ReturnFalse()
    {
        //Arrange
        int position = 0;
        int existingListPositions = 14;

        //Act
        var result = _updateListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckInvalidPositionNegativeNumber_ReturnFalse()
    {
        //Arrange
        int position = -10;
        int existingListPositions = 14;

        //Act
        var result = _updateListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckInvalidPositionNumberBiggerThanActualListContentCount_ReturnFalse()
    {
        //Arrange
        int position = 14;
        int existingListPositions = 10;

        //Act
        var result = _updateListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
    }
    #endregion

    #endregion
}
