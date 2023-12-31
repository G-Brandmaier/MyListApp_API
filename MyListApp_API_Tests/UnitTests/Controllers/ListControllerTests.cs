﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using MyListApp_API.Controllers;
using MyListApp_API.models;
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

    #region Gabriella Testar (26st)

    #region Testar metoden CreateUserList
    [Fact]
    public async Task CreateUserList_ShouldCreateUserList_ReturnCreatedWithTheCreatedList()
    {
        //Arrange
        var userListDto = new UserListDto { UserId = Guid.NewGuid(), Title = "My new list" };
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
    public async Task CreateUserList_UserListCouldNotBeCreated_ReturnBadRequestWithMessage()
    {
        //Arrange
        UserListDto userListDto = new UserListDto { Title = "Att göra" };
        string expectedErrorMessage = "Invalid information received, try again!";

        //Act
        var result = await _listController.CreateUserList(userListDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task CreateUserList_ReceivesEmptyTitle_ReturnBadRequestWithMessage()
    {
        //Arrange
        UserListDto userListDto = new UserListDto { Title = "", UserId = Guid.NewGuid() };
        string expectedErrorMessage = "Title input can't be empty!";

        //Act
        var result = await _listController.CreateUserList(userListDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    #endregion

    #region Testar metoden AddToUserList
    [Fact]
    public async Task AddToUserList_ShouldAddStringToSpecifiedUserList_ReturnCreatedWithUpdatedUserList()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), Content = "Handla" };
        var expectedUserListReturned = new UserList { Id = It.IsAny<Guid>(), Title = "Att göra", ListContent = { "Handla" }, UserId = It.IsAny<Guid>() };
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
    public async Task AddToUserList_UserListCouldNotBeUpdated_ReturnProblemWithMessage()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), Content = "Handla" };
        UserList userList = null;
        var expectedErrorMessage = "Could not add to list";
        _listServiceMock.Setup(x => x.AddToUserList(listItemDto)).Returns(userList);

        //Act
        var result = await _listController.AddToUserList(listItemDto);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var value = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(expectedErrorMessage, value.Detail);
    }

    [Fact]
    public async Task AddToUserList_ReceivesEmptyContent_ReturnBadRequestWithMessage()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), Content = string.Empty };
        string expectedErrorMessage = "Content input can't be empty!";

        //Act
        var result = await _listController.AddToUserList(listItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task AddToUserList_ReceivesBlankSpaceOnlyContent_ReturnBadRequestWithMessage()
    {
        //Arrange
        var listItemDto = new ListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), Content = "  " };
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

    #region Testar metoden GetAllUserListsById

    [Fact]
    public async Task GetAllUserListsById_ShouldGetAllUserListsByUserId_ReturnOkWithListOfUserLists()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        var expectedListUserLists = new List<UserList> { new UserList { Title = "Test List", UserId = new Guid("12a643af-a171-4ce3-9b55-a7e017607497"), Id = new Guid("b1de0c7b-b4af-4dca-8f17-9a3656f0c60c") } };
        _listServiceMock.Setup(x => x.GetAllUserListsById(It.IsAny<Guid>())).Returns(expectedListUserLists);

        //Act
        var result = _listController.GetAllUserListsById(userId);

        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<List<UserList>>(response.Value);
    }

    [Fact]
    public async Task GetAllUserListsById_UserIdDoesNotExist_ReturnProblemWithMessage()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        List<UserList> expectedListUserLists = null;
        var expectedErrorMessage = "Could not fetch all lists!";
        _listServiceMock.Setup(x => x.GetAllUserListsById(It.IsAny<Guid>())).Returns(expectedListUserLists);

        //Act
        var result = _listController.GetAllUserListsById(userId);

        //Assert
        var response = Assert.IsType<ObjectResult>(result.Result);
        var errorMessage = Assert.IsType<ProblemDetails>(response.Value);
        Assert.Equal(expectedErrorMessage, errorMessage.Detail);
    }

    [Fact]
    public async Task GetAllUserListsById_UserIdIsEmpty_ReturnBadRequestWithMessage()
    {
        //Arrange
        Guid userId = Guid.Empty;
        var expectedErrorMessage = "Invalid input, try again!";

        //Act
        var result = _listController.GetAllUserListsById(userId);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result.Result);
        var errorMessage = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, errorMessage);
    }

    [Fact]
    public async Task GetAllUserListsById_ReceivesInvalidModel_ReturnBadRequestWithMessage()
    {
        //Arrange
        Guid userId = Guid.NewGuid();
        string expectedErrorMessage = "Invalid input, try again!";
        _listController.ModelState.AddModelError("Invalid input", "Invalid input, try again!");

        //Act
        var result = await _listController.GetAllUserListsById(userId);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    #endregion

    #region Testar metoden UpdateUserListContent

    [Fact]
    public async Task UpdateUserListContent_ReceivesInvalidModel_ReturnBadRequestWithMessage()
    {
        //Arrange
        var updateListItemDto = new UpdateListItemDto();
        string expectedErrorMessage = "Invalid information received, try again";
        _listController.ModelState.AddModelError("Invalid model", "Invalid information received, try again");

        //Act
        var result = await _listController.UpdateUserListContent(updateListItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task UpdateUserListContent_ShouldUpdateListContentInUserList_ReturnOkWithUpdatedList()
    {
        //Arrange
        var updatedListItemDto = new UpdateListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), ContentPosition = 1, NewContent = "Plugga" };
        var expectedUserListReturned = new UserList { Id = Guid.NewGuid(), Title = "Title", ListContent = new List<string> { "Plugga", "Städa" }, UserId = Guid.NewGuid() };
        _listServiceMock.Setup(x => x.UpdateUserListContent(updatedListItemDto)).Returns(expectedUserListReturned);

        //Act
        var result = await _listController.UpdateUserListContent(updatedListItemDto);

        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<OkObjectResult>(result);
        var returnedUserList = Assert.IsType<UserList>(response.Value);
        Assert.Equal(2, returnedUserList.ListContent.Count);
        Assert.Equal(updatedListItemDto.NewContent, returnedUserList.ListContent[updatedListItemDto.ContentPosition - 1]);
    }

    [Fact]
    public async Task UpdateUserListContent_ReceivesEmptyNewContent_ReturnBadRequestWithMessage()
    {
        //Arrange
        var updatedListItemDto = new UpdateListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), ContentPosition = 1, NewContent = "" };
        var expectedErrorMessage = "Content input can't be empty!";

        //Act
        var result = await _listController.UpdateUserListContent(updatedListItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, value);
    }

    [Fact]
    public async Task UpdateUserListContent_ReceivesBlankSpaceOnlyNewContent_ReturnBadRequestWithMessage()
    {
        //Arrange
        var updatedListItemDto = new UpdateListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), ContentPosition = 1, NewContent = "       " };
        var expectedErrorMessage = "Content input can't be empty!";

        //Act
        var result = await _listController.UpdateUserListContent(updatedListItemDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, value);
    }

    [Fact]
    public async Task UpdateUserListContent_UserListCouldNotBeUpdated_ReturnProblemWithMessage()
    {
        //Arrange
        var updatedListItemDto = new UpdateListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), ContentPosition = 1, NewContent = "Handla" };
        UserList userList = null;
        var expectedErrorMessage = "Could not update list";
        _listServiceMock.Setup(x => x.UpdateUserListContent(updatedListItemDto)).Returns(userList);

        //Act
        var result = await _listController.UpdateUserListContent(updatedListItemDto);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var value = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(expectedErrorMessage, value.Detail);
    }
    #endregion

    #region Testar metoden UpdateUserListTitle

    [Fact]
    public async Task UpdateUserListTitle_ShouldUpdateUserListTitle_ReturnOkWithUpdatedUserList()
    {
        //Arrange
        var updateUserListDto = new UpdateUserListDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), NewTitle = "Ny Titel" };
        var expectedUserListReturned = new UserList { Id = Guid.NewGuid(), Title = "Ny Titel", ListContent = new List<string> { "Plugga", "Städa" }, UserId = Guid.NewGuid() };
        _listServiceMock.Setup(x => x.UpdateUserListTitle(updateUserListDto)).Returns(expectedUserListReturned);

        //Act
        var result = await _listController.UpdateUserListTitle(updateUserListDto);

        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedUserListReturned, response.Value);
        Assert.IsType<UserList>(response.Value);

    }

    [Fact]
    public async Task UpdateUserListTitle_ReceivesInvalidModel_ReturnBadRequestWithMessage()
    {
        //Arrange
        var updateUserListDto = new UpdateUserListDto();
        string expectedErrorMessage = "Invalid information received, try again";
        _listController.ModelState.AddModelError("Invalid model", "Invalid information received, try again");

        //Act
        var result = await _listController.UpdateUserListTitle(updateUserListDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task UpdateUserListTitle_ReceivesEmptyNewTitle_ReturnBadRequestWithMessage()
    {
        //Arrange
        var updateUserListDto = new UpdateUserListDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), NewTitle = "" };
        var expectedErrorMessage = "Title input can't be empty!";

        //Act
        var result = await _listController.UpdateUserListTitle(updateUserListDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, value);
    }

    [Fact]
    public async Task UpdateUserListTitle_ReceivesBlankSpaceOnlyNewContent_ReturnBadRequestWithMessage()
    {
        //Arrange
        var updateUserListDto = new UpdateUserListDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), NewTitle = "     " };
        var expectedErrorMessage = "Title input can't be empty!";

        //Act
        var result = await _listController.UpdateUserListTitle(updateUserListDto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, value);
    }

    [Fact]
    public async Task UpdateUserListTitle_UserListTitleCouldNotBeUpdated_ReturnProblemWithMessage()
    {
        //Arrange
        var updateUserListDto = new UpdateUserListDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), NewTitle = "Ny test titel" };
        UserList userList = null;
        var expectedErrorMessage = "Could not update list title";
        _listServiceMock.Setup(x => x.UpdateUserListTitle(updateUserListDto)).Returns(userList);

        //Act
        var result = await _listController.UpdateUserListTitle(updateUserListDto);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var value = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(expectedErrorMessage, value.Detail);
    }

    #endregion

    #region Testar metoden DeleteUserListContent

    [Fact]
    public async Task DeleteUserListContent_ShouldDeleteListContentInUserList_ReturnOkWithUpdatedUserList()
    {
        //Arrange
        var deleteListItemdto = new DeleteListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), ContentPosition = 1 };
        _listServiceMock.Setup(x => x.DeleteUserListContent(deleteListItemdto)).Returns(true);
        var expectedResponseMessage = "List content removed";

        //Act
        var result = await _listController.DeleteUserListContent(deleteListItemdto);

        //Assert
        Assert.NotNull(result);
        var response = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedResponseMessage, value);
    }

    [Fact]
    public async Task DeleteUserListContent_ReceivesInvalidModel_ReturnBadRequestWithMessage()
    {
        //Arrange
        var deleteListItemdto = new DeleteListItemDto();
        string expectedErrorMessage = "Invalid information received, try again";
        _listController.ModelState.AddModelError("Invalid model", "Invalid information received, try again");

        //Act
        var result = await _listController.DeleteUserListContent(deleteListItemdto);

        //Assert
        var response = Assert.IsType<BadRequestObjectResult>(result);
        var value = Assert.IsType<string>(response.Value);
        Assert.Equal(expectedErrorMessage, response.Value);
    }

    [Fact]
    public async Task DeleteUserListContent_CouldNotDeleteListContentInUserList_ReturnProblemWithMessage()
    {
        //Arrange
        var deleteListItemdto = new DeleteListItemDto { UserId = Guid.NewGuid(), UserListId = Guid.NewGuid(), ContentPosition = 1 };
        _listServiceMock.Setup(x => x.DeleteUserListContent(deleteListItemdto)).Returns(false);
        var expectedErrorMessage = "Could not delete list content";

        //Act
        var result = await _listController.DeleteUserListContent(deleteListItemdto);

        //Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        var value = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(expectedErrorMessage, value.Detail);
    }

    #endregion

    #endregion


    #region Stefanie Testar (11 st)

    #region GetAllUserLists
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
        Assert.Equal("application/json", objectResult.ContentTypes.FirstOrDefault());
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

    //Test 6
    [Fact]
    public void GetallUserLists_ReturnsOkResult_WhenReturnsNull()
    {
        //Arrange
        _listServiceMock.Setup(s => s.GetAllLists()).Returns((List<UserList>)null);

        //Act
        var result = _listController.GetAllUserLists();

        //Assign
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Null(okResult.Value);
    }

    //Test 7.
    [Fact]
    public void GetAllUserLists_HandleListWithLargeDataValues()
    {
        //Arrange
        var largeDataLists = new List<UserList>
        {
            new UserList { Title = new string('A', 1000000) },
            new UserList { Title = new string('B', 1000000) }
        };
        _listServiceMock.Setup(s => s.GetAllLists()).Returns(largeDataLists);

        //Act
        var result = _listController.GetAllUserLists();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedList = Assert.IsType<List<UserList>>(okResult.Value);
        Assert.Equal(2, returnedList.Count);
    }

    //Test 8.
    [Fact]
    public void GetAllUserLists_HandleTimeOut()
    {
        // Arrange
        _listServiceMock.Setup(s => s.GetAllLists()).Throws(new TimeoutException());

        // Act & Assert
        Assert.Throws<TimeoutException>(() => _listController.GetAllUserLists());
    }

    #endregion

    #region DeleteUserList

    //Test 1.
    [Fact]
    public void DeleteUserList_ReturnNotFound_WhenDtoIsNull()
    {
        //Act
        var result = _listController.DeleteUserList(null);

        //Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    //Test 2.
    [Fact]
    public void DeleteUserList_ReturnsNotFound_WhenFails()
    {
        //Arrange
        _listServiceMock.Setup(s => s.DeleteList(It.IsAny<DeleteUserListDto>())).Returns(false);

        //Act
        var result = _listController.DeleteUserList(new DeleteUserListDto());

        //Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("List not found or userId dosn't match", notFoundResult.Value);
    }

    //Test 3.
    [Fact]
    public void DeleteUserList_ReturnOk_WhenSucessfull()
    {
        //Arrange
        _listServiceMock.Setup(s => s.DeleteList(It.IsAny<DeleteUserListDto>())).Returns(true);

        //Act
        var result = _listController.DeleteUserList(new DeleteUserListDto());
        
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("List sucessfully deleted", okResult.Value);
    }
    
}

#endregion

#endregion

