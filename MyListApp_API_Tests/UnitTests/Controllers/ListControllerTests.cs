using Microsoft.AspNetCore.Mvc;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.Models;
using MyListApp_API.Services;

namespace MyListApp_API_Tests.UnitTests.Controllers;

public class ListControllerTests
{
    private readonly ListController _listController;
    private readonly Mock<IListService> _listServiceMock;

    public ListControllerTests()
    {
        _listServiceMock = new Mock<IListService>();
        _listController = new ListController(_listServiceMock.Object);
    }

    #region Gabriella Testar (7st)

    [Fact]
    public async Task CreateUserList_ShouldCreateUserList_ReturnCreatedWithTheCreatedList()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            UserId = Guid.NewGuid(),
            Title = "My new list"
        };
        var expectedResult = new UserList { Id = It.IsAny<Guid>(), UserId = It.IsAny<Guid>(), Title = "My new list", ListContent = new List<string>() };

       _listServiceMock.Setup(x => x.CreateUserList(userListDto)).Returns(expectedResult);

        //Act
        var result = await _listController.CreateUserList(userListDto);

        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<CreatedResult>(result);
        var value = Assert.IsType<UserList>(response.Value);
    }

    [Fact]
    public async Task CreateUserList_ReceivesInvalidModel_ReturnBadRequestWithMessage()
    {
        //Arrange
        UserListDto userListDto = new UserListDto();

        string expectedErrorMessage = "Invalid information received, try again!";
        _listController.ModelState.AddModelError("Invalid input", "Invalid information received, try again!");

        //Act
        var result = await _listController.CreateUserList(userListDto);


        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task CreateUserList_UserIdDoesNotExcist_ReturnBadRequestWithMessage() //Måste skriva checken i controllern sen fixa klart i testet
    {
        //Arrange
        UserListDto userListDto = new UserListDto
        {
            Title = "Att göra"
        };

        string expectedErrorMessage = "Invalid information received, try again!";

        //Act
        var result = await _listController.CreateUserList(userListDto);


        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task AddToUserList_ShouldAddStringToSpecifiedUserList_ReturnCreatedWithUpdatedUserList()
    {
        //Arrange
        var listItemDto = new ListItemDto
        {
            UserId = Guid.NewGuid(),
            UserListId = Guid.NewGuid(),
            Content = "Handla"
        };

        var expectedUserListReturned = new UserList
        {
            Id = It.IsAny<Guid>(),
            Title = "Att göra",
            ListContent =
            {
                "Handla"
            },
            UserId = It.IsAny<Guid>()
        };

        _listServiceMock.Setup(x => x.AddToUserList(listItemDto)).Returns(expectedUserListReturned);

        //Act
        var result = await _listController.AddToUserList(listItemDto);

        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<CreatedResult>(result);
        var value = Assert.IsType<UserList>(response.Value);
        Assert.Equal(expectedUserListReturned, value);
    }

    [Fact]
    public async Task AddToUserList_ReceivesEmptyString_ReturnBadRequestWithMessage()
    {
        //Arrange
        var listItemDto = new ListItemDto
        {
            UserId = Guid.NewGuid(),
            UserListId = Guid.NewGuid(),
            Content = string.Empty
        };
        string expectedErrorMessage = "Content input can't be empty!";

        //Act
        var result = await _listController.AddToUserList(listItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task AddToUserList_ReceivesBlankSpaceOnly_ReturnBadRequestWithMessage()
    {
        //Arrange
        var listItemDto = new ListItemDto
        {
            UserId = Guid.NewGuid(),
            UserListId = Guid.NewGuid(),
            Content = "  "
        };
        string expectedErrorMessage = "Content input can't be empty!";

        //Act
        var result = await _listController.AddToUserList(listItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task AddToUserList_ReceivesInvalidModel_ReturnBadRequestWithMessage()
    {
        //Arrange
        var listItemDto = new ListItemDto();

        string expectedErrorMessage = "Invalid input, try again!";
        _listController.ModelState.AddModelError("Invalid model", "Invalid information received, try again!");

        //Act
        var result = await _listController.AddToUserList(listItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }



    #endregion

}
