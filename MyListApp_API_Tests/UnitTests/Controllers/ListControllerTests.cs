using Microsoft.AspNetCore.Mvc;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.Models;
using MyListApp_API.Services;
using System.Security.Cryptography.X509Certificates;

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


    #region Stefanie Testar (5 st)
    //Test 1. returnerar en lista med UserList-objekt
    [Fact]
    public void GetAllUserLists_ReturnOkResulu_WithListOfUserLists()
    {
        //Arrange
        var mockLists = new List<UserList>
    {
        new UserList { Title = "List1"},
        new UserList { Title = "List2"}
    };
        _listServiceMock.Setup(x => x.GetAllLists()).Returns(mockLists);

        //Act
        var result = _listController.GetAllUserLists();

        //Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnLists = okResult.Value as List<UserList>;
        Assert.Equal(2, returnLists.Count);

    }
    //Test 2. returnerar en tom lista (inga UserLists-objekt att hämta)
    [Fact]
    public void GetAllUserLists_ReturnsOkResult_WithEmptyLists_WhenNoUserListExists()
    {
        //Arrange
        _listServiceMock.Setup(s => s.GetAllLists()).Returns(new List<UserList>());

        //Act
        var result = _listController.GetAllUserLists();

        //Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnLists = okResult.Value as IList<UserList>;
        Assert.Empty(returnLists);
    }

    //Test 3.Validera att GetAllUserList svarar när IListService kastar ett undantag
    [Fact]
    public void GetAllUserLists_ReturnsInternalServerError_WhenServiceThrowsException()
    {
        //Arrange
        _listServiceMock.Setup(s => s.GetAllLists()).Throws(new Exception());
        var controller = new ListController(_listServiceMock.Object);

        //Act
        var result = controller.GetAllUserLists();
        
        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    //Test 4. Kontrollera att svaret från GetAllUserLists har rätt Content-typ
    [Fact]
    public void GetAllUserLists_ReturnsCorrectContentType()
    {
        //Arrange 
        _listServiceMock.Setup(s => s.GetAllLists()).Returns(new List<UserList>());
        var controller = new ListController(_listServiceMock.Object);

        //Act 
        var result = controller.GetAllUserLists();

        //Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("application/json", objectResult.ContentTypes.First());
    }

    //Test 5. Validerar att metoden kan hantera ett stort antal listor utan att kasta ett undan tag
    [Fact]
    public void GetAllUserLists_HandleLargeNumberOfLists()
    {
        //Arrange
        var largeNumberOfLists = new List<UserList>();

        for (int i = 0; i < 10000; i++) // ex. 10.000
        {
            largeNumberOfLists.Add(new UserList
            {
                Title = i.ToString(),
                ListContent = new List<string> { i.ToString() }

                //alt 2. 
                //Title = $"Title{i}",
                //ListContent = new List<string> { $"Item{i}" }
            });
        }

        _listServiceMock.Setup(s => s.GetAllLists()).Returns(largeNumberOfLists);
        var controller = new ListController(_listServiceMock.Object);

        //Act
        var result = controller.GetAllUserLists();

        //Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        var returnedLists = Assert.IsType<List<UserList>>(objectResult.Value);
        Assert.Equal(10000, returnedLists.Count);
    }
}
#endregion

