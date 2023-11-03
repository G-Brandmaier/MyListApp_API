using MyListApp_API.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyListApp_API_Tests.APITests;

public class AccountControllerAPITests
{
    private readonly RestClient _client;

    public AccountControllerAPITests()
    {
        _client = new RestClient("https://localhost:7223/api");
    }
    [Fact]
    public void UpdatePassword_ShouldUpdatePassword_ReturnSuccess()
    {
       
        var updatePasswordDto = new UpdatePasswordDto
        {
            UserId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed"),
            CurrentPassword = "Password1",
            NewPassword = "nyttLösenord123"
        };

        var request = new RestRequest("Account/update-password", Method.Put);
        request.AddJsonBody(updatePasswordDto);

        var response = _client.Execute(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }



    [Fact]
    public async Task DeleteAccount_ShouldDeleteUser_ReturnOk()
    {
      
        var userId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed");

      
        using var client = new HttpClient();
        var requestUri = $"https://localhost:7223/api/Account/delete-account?userId={userId}";


       
        var response = await client.DeleteAsync(requestUri);

        // Kontrollera att statuskoden som returneras är 200 OK.
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
       
        Assert.Contains("Account deleted successfully", content);
    }



}


