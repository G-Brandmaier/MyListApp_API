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


    #region Register-API-Test-Ria
    [Fact]
    public void Register_ValidUserDetails_ShouldRegisterAndReturnOk()
    {
        // Siapkan data input
        var registerData = new RegisterUserDto
        {
            Email = "test@email.com",
            Password = "Test@123",

        };

        var jsonRegisterData = JsonConvert.SerializeObject(registerData);
        var request = new RestRequest("account/register", Method.Post);
        request.AddJsonBody(jsonRegisterData, "application/json");
        var response = _client.Execute(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var registerResponse = JsonConvert.DeserializeObject<RegisterResponse>(response.Content);
        Assert.Equal("Registration successful", registerResponse.Message);
    }



    [Fact]
    public void Register_InvalidUserDetails_ShouldReturnBadRequest()
    {
        var registerData = new RegisterUserDto
        {
            // invalid input data (for example without email or username)
            Email = "",
            Password = "Test@123",
        };

        var request = new RestRequest("account/register", Method.Post);
        request.AddJsonBody(registerData);
        var response = _client.Execute(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        
        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);

        // Check if there are any errors regarding the "Email" field
        if (errorResponse.Errors.ContainsKey("Email"))
        {
            Assert.Contains("The Email field is required.", errorResponse.Errors["Email"]);
        }
    }

    #endregion

    #region Login-API-Test-Ria


    [Fact]
    public void Login_InvalidModelData_ReturnsBadRequest()
    {
        var loginData = new LoginUserDto { Email = "", Password = "" }; // Invalid data or no data
        var request = new RestRequest("account/login", Method.Post);
        request.AddJsonBody(loginData);
        var response = _client.Execute(request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }




    [Fact]
    public void Login_InvalidDetails_ReturnsUnauthorized()
    {
        var loginData = new LoginUserDto { Email = "invalid@email.com", Password = "invalidPassword" };
        var request = new RestRequest("account/login", Method.Post);
        request.AddJsonBody(loginData);
        var response = _client.Execute(request);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }


    #endregion

}
