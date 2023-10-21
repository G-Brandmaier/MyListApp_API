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
    public async Task<IActionResult> CreateListItem(UserListDto dto)
    {
        if (ModelState.IsValid)
        {
            //Hämta användare, om användare finns fortsätt
            var listItem = _listService.CreateListItem(dto);
            if (listItem != null)
            {
                return Created("", null);
            }
            return BadRequest("Something went wrong, try again!");
        }
        return BadRequest("Invalid information received, try again!");
    }

    [HttpPost]
    [Route("AddToList")]
    public async Task<IActionResult> AddToList(ListItemDto item)
    {
        if (ModelState.IsValid)
        {
            var listItem = item;
            var result = _listService.AddListItemToList(listItem,item.UserListId, item.UserId);
            if (result != null)
            {
                return Created("", null);
            }
            return Problem("Could not add to list");
        }
        return BadRequest("Something went wrong, try again!");
    }
}
