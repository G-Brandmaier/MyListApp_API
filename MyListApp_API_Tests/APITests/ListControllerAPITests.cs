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

    [Fact]
    public void CreateUserList_ShouldCreateUserList_ReturnCreatedWithUserList()
    {
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");

        var userList = new UserListDto { Title = "Test", UserId = userId };
        var jsonUserList = JsonConvert.SerializeObject(userList);
        var request = new RestRequest("List/AddList", Method.Post);
        request.AddJsonBody(userList, "application/json");
        var response = _client.Execute(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

    }

}
