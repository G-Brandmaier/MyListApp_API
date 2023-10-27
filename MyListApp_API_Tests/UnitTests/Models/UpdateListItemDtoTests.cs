using Moq;
using MyListApp_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.UnitTests.Models;

public class UpdateListItemDtoTests
{
    private readonly UpdateListItemDto _itemDto;

    public UpdateListItemDtoTests()
    {
        _itemDto = new UpdateListItemDto();
    }
    #region Gabriella testar

    [Fact]
    public void CheckValidAmountOfCharactersForNewContent_ShouldCheckValidStringLength_ReturnTrue()
    {
        //Arrange
        string validContentLength = "Hämpta ut paket från postombud";

        //Act
        var result = _itemDto.CheckValidAmountOfCharactersForNewContent(validContentLength);

        //Assert
        Assert.True(result);
    }
    #endregion
}
