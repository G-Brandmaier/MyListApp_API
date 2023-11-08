using MyListApp_API.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace MyListApp_API_Tests.APITests;

public class AccountControllerAPITests
{
    private readonly RestClient _client;

    public AccountControllerAPITests()
    {
        _client = new RestClient("https://localhost:7223/api");
    }

    #region Ghazaleh tests
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

    #endregion


    #region Register-API-Test-Ria
    [Fact]
    public void Register_ValidUserDetails_ShouldRegisterAndReturnOk()
    {

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
