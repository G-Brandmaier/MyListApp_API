using Microsoft.AspNetCore.Mvc;
using MyListApp_API.Models;
using MyListApp_API.Services;

namespace MyListApp_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ListController : ControllerBase
{
    private readonly ListService _listService;

    public ListController(ListService listService)
    {
        _listService = listService;
    }

    [HttpPost]
    [Route("AddList")]
    public async Task<IActionResult> CreateUserList(UserListDto dto)
    {
        if (ModelState.IsValid)
        {
            //Hämta användare, om användare finns fortsätt
            var userList = _listService.CreateUserList(dto);
            if (userList != null)
            {
                return Created("List successfully created", userList);
            }
            return Problem("Could not create list");
        }
        return BadRequest("Invalid information received, try again!");
    }

    [HttpPost]
    [Route("AddToList")]
    public async Task<IActionResult> AddToUserList(ListItemDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _listService.AddToUserList(dto);
            if (result != null)
            {
                return Created("Content added to selected list", result);
            }
            return Problem("Could not add to list");
        }
        return BadRequest("Something went wrong, try again!");
    }
}
