using Microsoft.AspNetCore.Mvc;
using MyListApp_API.Models;
using MyListApp_API.Services;

namespace MyListApp_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ListController : ControllerBase
{
    private readonly IListService _listService;

    public ListController(IListService listService)
    {
        _listService = listService;
    }

    [HttpPost]
    [Route("AddList")]
    public async Task<IActionResult> CreateUserList(UserListDto dto)
    {
        if (ModelState.IsValid)
        {
            if (string.IsNullOrEmpty(dto.Title))
            {
                return BadRequest("Title input can't be empty!");
            }
            var userList = _listService.CreateUserList(dto);
            if (userList != null)
            {
                return Created("List successfully created", userList);
            }
        }
        return BadRequest("Invalid information received, try again!");
    }

    [HttpPost]
    [Route("AddToList")]
    public async Task<IActionResult> AddToUserList(ListItemDto dto)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(dto.Content.TrimEnd()))
            {
                var result = _listService.AddToUserList(dto);
                if (result != null)
                {
                    return Created("Content added to selected list", result);
                }
                return Problem("Could not add to list");
            }
            return BadRequest("Content input can't be empty!");
        }
        return BadRequest("Invalid input, try again!");
    }

    //Get list
    [HttpGet]
    public IActionResult GetAllUserLists()
    {
        var lists = _listService.GetAllLists();
        return Ok(lists);
    }

    //Delete list
    [HttpDelete("DeleteList/{listId}")]
    public IActionResult DeleteUserList(Guid listId)
    {
        if(_listService.DeleteList(listId))
        {
            return Ok("List successfully deleted");
        }
        return NotFound("List not found");
    }

    [HttpGet]
    [Route("GetAllUserLists/{userId}")]
    public async Task<IActionResult> GetAllUserListsById(Guid userId)
    {
        if (ModelState.IsValid)
        {
            if(userId != Guid.Empty)
            {
                var resultUserLists = _listService.GetAllUserListsById(userId);
                if(resultUserLists != null)
                { 
                    return Ok(resultUserLists); 
                }
                return Problem("Could not fetch all lists!");
            }
        }
        return BadRequest("Invalid input, try again!");
    }

    [HttpPost]
    [Route("UpdateUserListContent")]
    public async Task<IActionResult> UpdateUserListContent(UpdateListItemDto dto)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(dto.NewContent.TrimEnd()))
            {
                var result = _listService.UpdateUserListContent(dto);
                if (result != null)
                {
                    return Ok(result);
                }
                return Problem("Could not update list");
            }
            return BadRequest("Content input can't be empty!");
        }
        return BadRequest("Invalid information received, try again");
    }

}
