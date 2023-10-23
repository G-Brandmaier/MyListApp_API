﻿using MyListApp_API.Controllers;
using MyListApp_API.Models;
using MyListApp_API.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;

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
    public async Task CreateListItem_ShouldCreateUserListAndAddToUsersLists_ReturnCreated()
    {
        //Arrange
        var userListDto = new UserListDto
        {
            UserId = new Guid("fa5c9752-2639-4061-b5d6-683871bf3fcd"),
            Title = "My new list"
        };
        var userList = new UserList
        {
            Id = new Guid("fa5c9752-2639-4061-b5d6-683871bf3fcd"),
            Title = "Saker att göra",
            ListContent = new List<string>
            {
                "Städa"
            }
        };



        //Act



        //Assert

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

        _listServiceMock.Setup(x => x.AddToUserList(listItemDto.Content, It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(false); //Gjort om kolla vad som faktiskt ska göras

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