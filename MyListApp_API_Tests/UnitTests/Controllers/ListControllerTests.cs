using Microsoft.AspNetCore.Mvc;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.Models;
using MyListApp_API.Services;

namespace MyListApp_API_Tests.UnitTests.Controllers;

public class ListControllerTests
{
    private readonly ListController _listController;
    private readonly Mock<ListService> _listServiceMock;

    public ListControllerTests()
    {
        _listServiceMock = new Mock<ListService>();
        _listController = new ListController(_listServiceMock.Object);
    }

    #region Gabriella Testar

    [Fact]
    public async Task CreateUserList_ShouldCreateUserList_ReturnCreatedWithTheCreatedList()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            UserId = new Guid("fa5h6752-2639-4061-b5d6-683871bf3fcd"),
            Title = "My new list"
        };
        var expectedResult = new UserList { Id = It.IsAny<Guid>(), UserId = It.IsAny<Guid>(), Title = "My new list", ListContent = new List<string>() };

       _listServiceMock.Setup(x => x.CreateUserList(userListDto)).Returns(expectedResult);


        //Act

        var result = _listController.CreateUserList(userListDto);

        //Assert
        Assert.NotNull(result);
        //var response = Assert.IsType<CreatedResult>(result.Result);
        //var value = Assert.IsType<string>(response.Value);
    }

    [Fact]
    public async Task CreateListItem_ReceivesInvalidObject_ReturnBadRequestWithMessage()
    {
        //Arrange
        UserListDto userListDto = null;

        string errorMessage = "Invalid information received, try again!";

        //Act
        var result = _listController.CreateUserList(userListDto);


        //Assert

        var response = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(errorMessage, response.Value);
    }

    [Fact]
    public async Task AddToList_ShouldAddListItemToUsersList_ReturnCreated()
    {
        //Arrange
        var listItemDto = new ListItemDto
        {
            UserId = Guid.NewGuid(),
            UserListId = Guid.NewGuid(),
            Content = "Handla"
        };

        var expectedListItemReturned = "Handla";


        //Användare kopplad till listan

       // _listServiceMock.Setup(x => x.AddToUserList(listItemDto.Content, It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(false); //Gjort om kolla vad som faktiskt ska göras

        //Act

        var result = await _listController.AddToUserList(listItemDto);

        //Assert


        Assert.NotNull(result);
        var response = Assert.IsType<CreatedResult>("");
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedListItemReturned, value);
    }





    #endregion

}
