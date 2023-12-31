﻿using Microsoft.AspNetCore.Mvc;
using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Services;

namespace MyListApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IUserService userService, ILogger<AccountController> logger)
        {
            _userService = userService; // Injects the IUserService dependency
            _logger = logger;
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
                return Ok(new RegisterResponse{ Message = "Registration successful" });
            }

            // If registration fails, send an error message
            return BadRequest(new { Message = "Registration failed" });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);    // Returns a 400 Bad Request if the model state is not valid
                }

                var loginResponse = await _userService.AuthenticateAsync(model.Email, model.Password);

                if (loginResponse != null)
                {
                    return Ok(loginResponse); // Returns a 200 OK response with user information if login is successful
                }

                return Unauthorized(new { Message = "Login failed" }); // Returns a 401 Unauthorized response if login fails

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during login.");
                return StatusCode(500, new { Message = "An error occurred during login." });
            }

        }


        [HttpGet("all-users")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();

                var userResponses = users.Select(user => new
                {
                    Id = user.Id,
                    Email = user.Email
                });

                return Ok(userResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all users.");
                return StatusCode(500, new { Message = "An error occurred while fetching all users." });
            }
        }




        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _userService.UpdatePassword(model.UserId, model.CurrentPassword, model.NewPassword); // assuming user id is in the token


            if (result)
            {
                return Ok(new UpdatePasswordResponse { Message = "Password updated successfully" });
            }

            return BadRequest(new UpdatePasswordResponse { Message = "Password update failed" });

        }

        [HttpDelete("delete-account")]
        public async Task<IActionResult> DeleteAccount(Guid userId)
        {
            var result = _userService.DeleteUserAsync(userId); // assuming user id is in the token

            if (result)
            {
                return Ok(new { Message = "Account deleted successfully" });
            }

            return BadRequest(new { Message = "Account deletion failed" });
        }

        [HttpPut("UpdateUserDetails")] 
        public IActionResult UpdateUserDetails([FromBody] UpdateUserDto updateUserDto)
        {
            if (_userService.UpdateUserDetails(updateUserDto))
            {
                return Ok("User details updated successfully.");
            }
            return NotFound("User not found.");
        }

    }
}

