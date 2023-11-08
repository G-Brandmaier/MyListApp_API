using MyListApp_API.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace MyListApp_API_Tests.APITests;

public class ListControllerAPITests
{
    private readonly RestClient _client;

    public ListControllerAPITests()
    {
        _client = new RestClient("https://localhost:7223/api");
    }

    #region Gabriella testar

    [Fact]
    public void CreateUserList_ShouldCreateUserList_ReturnCreatedWithUserList()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");
        var userList = new UserListDto { Title = "Test", UserId = userId };
        var jsonUserList = JsonConvert.SerializeObject(userList);

        var request = new RestRequest("List/AddList", Method.Post);
        request.AddJsonBody(jsonUserList, "application/json");
        var response = _client.Execute(request);

        var result = JsonConvert.DeserializeObject<UserList>(response.Content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.IsType<UserList>(result);
        Assert.Equal(userList.Title, result.Title);
    }

    [Fact]
    public void AddToUserList_ShouldAddContentToUserList_ReturnOkWithUpdatedUserList()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");
        var listItemDto = new ListItemDto { UserId = userId, UserListId = new Guid("c3a9e351-ed6b-4d36-84c0-7d29af59ad1b"), Content = "Plugga" };
        var jsonlistItemDto = JsonConvert.SerializeObject(listItemDto);

        var request = new RestRequest("List/AddToList", Method.Post);
        request.AddJsonBody(jsonlistItemDto, "application/json");
        var response = _client.Execute(request);

        var result = JsonConvert.DeserializeObject<UserList>(response.Content);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.IsType<UserList>(result);
    }

    [Fact]
    public void GetAllUserListsByUserId_ShouldGetAllUserListsByUserId_ReturnOkWithAllUserList()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");
        var request = new RestRequest($"List/GetAllUserLists/{userId}", Method.Get);
        var response = _client.Execute(request);

        var result = JsonConvert.DeserializeObject<List<UserList>>(response.Content);

        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<List<UserList>>(result);
    }

    [Fact]
    public void UpdateUserListContent_ShouldUpdateListContent_ReturnOkUpdatedUserList()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");
        var updateListItemDto = new UpdateListItemDto { UserId = userId, UserListId = new Guid("c3a9e351-ed6b-4d36-84c0-7d29af59ad1b"), NewContent = "Plugga", ContentPosition = 1 };
        var jsonlistItemDto = JsonConvert.SerializeObject(updateListItemDto);

        var request = new RestRequest("List/UpdateUserListContent", Method.Put);
        request.AddJsonBody(jsonlistItemDto, "application/json");
        var response = _client.Execute(request);

        var result = JsonConvert.DeserializeObject<UserList>(response.Content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<UserList>(result);
        Assert.Equal(updateListItemDto.NewContent, result.ListContent[updateListItemDto.ContentPosition -1]);
    }

    [Fact]
    public void UpdateUserListTitle_ShouldUpdateUserListTitle_ReturnOkUpdatedUserList()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");
        var updateUserListDto = new UpdateUserListDto { NewTitle = "Test", UserId = userId, UserListId = new Guid("c3a9e351-ed6b-4d36-84c0-7d29af59ad1b") };
        var jsonUserList = JsonConvert.SerializeObject(updateUserListDto);

        var request = new RestRequest("List/UpdateUserListTitle", Method.Put);
        request.AddJsonBody(jsonUserList, "application/json");
        var response = _client.Execute(request);

        var result = JsonConvert.DeserializeObject<UserList>(response.Content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.IsType<UserList>(result);
        Assert.Equal(updateUserListDto.NewTitle, result.Title);
    }

    [Fact]
    public void DeleteUserListContent_ShouldDeleteListContentFromUserList_ReturnOkWithMessage()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");
        var deleteListItemDto = new DeleteListItemDto { UserId = userId, UserListId = new Guid("c3a9e351-ed6b-4d36-84c0-7d29af59ad1b"), ContentPosition = 2 };
        var jsonUserList = JsonConvert.SerializeObject(deleteListItemDto);

        var request = new RestRequest("List/DeleteUserListContent", Method.Delete);
        request.AddJsonBody(jsonUserList, "application/json");
        var response = _client.Execute(request);
        var expectedMessage = "List content removed";

        var result = JsonConvert.DeserializeObject<string>(response.Content);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var value = Assert.IsType<string>(result);
        Assert.Equal(expectedMessage, value);
    }

    #endregion
}
