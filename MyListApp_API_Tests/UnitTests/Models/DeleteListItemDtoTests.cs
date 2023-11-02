using MyListApp_API.Models;

namespace MyListApp_API_Tests.UnitTests.Models;

public class DeleteListItemDtoTests
{
    private readonly DeleteListItemDto _deleteListItemDto;

    public DeleteListItemDtoTests()
    {
        _deleteListItemDto = new DeleteListItemDto();
    }

    #region Gabriella Testar 6st

    #region Testar metoden CheckValidContentPosition

    [Fact]
    public void CheckValidContentPosition_ShouldCheckValidPosition_ReturnTrue()
    {
        //Arrange
        int position = 10;
        int existingListPositions = 14;

        //Act
        var result = _deleteListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.True(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckInvalidPositionZero_ReturnFalse()
    {
        //Arrange
        int position = 0;
        int existingListPositions = 14;

        //Act
        var result = _deleteListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckInvalidPositionNegativeNumber_ReturnFalse()
    {
        //Arrange
        int position = -5;
        int existingListPositions = 14;

        //Act
        var result = _deleteListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckInvalidPositionNumberBiggerThanExistingListContentCount_ReturnFalse()
    {
        //Arrange
        int position = 17;
        int existingListPositions = 14;

        //Act
        var result = _deleteListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckExistingListContentCountIsBiggerThanZero_ReturnTrue()
    {
        //Arrange
        int position = 3;
        int existingListPositions = 4;

        //Act
        var result = _deleteListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.True(result);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void CheckValidContentPosition_ShouldCheckExistingListContentCountIsZero_ReturnFalse()
    {
        //Arrange
        int position = 3;
        int existingListPositions = 0;

        //Act
        var result = _deleteListItemDto.CheckValidContentPosition(position, existingListPositions);

        //Assert
        Assert.False(result);
        Assert.IsType<bool>(result);
    }
    #endregion

    #endregion
}
