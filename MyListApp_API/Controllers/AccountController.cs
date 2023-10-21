using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MyListApp_API.Models;
using MyListApp_API.Services;

namespace MyListApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService; // Injects the IUserService dependency
        }

        // Endpoint for new users
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto model)
        {
            // Model validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // Returns a 400 Bad Request if the model state is not valid
            }

            var registrationResult = await _userService.RegisterUserAsync(model);

            // If registration is successful - Send success response
            if (registrationResult)
            {
                return Ok(new { Message = "Registration successful" });
            }

            // If registration fails, send an error message
            return BadRequest(new { Message = "Registration failed" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);    // Returns a 400 Bad Request if the model state is not valid
            }

            var user = await _userService.AuthenticateAsync(model.Email, model.Password);

            if (user != null)
            {
                return Ok(new { Message = "Login successful", UserId = user.Id }); // Returns a 200 OK response with user information if login is successful
            }

            return Unauthorized(new { Message = "Login failed" }); // Returns a 401 Unauthorized response if login fails
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.UpdatePasswordAsync(User.Identity.Name, model.CurrentPassword, model.NewPassword); // assuming user id is in the token

            if (result)
            {
                return Ok(new { Message = "Password updated successfully" });
            }

            return BadRequest(new { Message = "Password update failed" });
        }


        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount()
        {
            var result = await _userService.DeleteUserAsync(User.Identity.Name); // assuming user id is in the token

            if (result)
            {
                return Ok(new { Message = "Account deleted successfully" });
            }

            return BadRequest(new { Message = "Account deletion failed" });
        }



    }
}

