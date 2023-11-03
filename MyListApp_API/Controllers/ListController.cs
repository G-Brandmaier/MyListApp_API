using Microsoft.AspNetCore.Mvc;
using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Services;
using System.Net;

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
    [HttpGet("GetAllUserLists")]
    public IActionResult GetAllUserLists()
    {
        try
        {
            var lists = _listService.GetAllLists();
            return Ok(lists);
        }
        catch (Exception ex)
        {
            return new ObjectResult("Internal server error") { StatusCode = (int)HttpStatusCode.InternalServerError };
        }
    }

    [HttpDelete("DeleteList")]
    public IActionResult DeleteUserList([FromBody] DeleteUserListDto dto)
    {
        if(dto == null)
        {
            return BadRequest("Provided data is invalid");
        }

        if (_listService.DeleteList(dto))
        {
            return Ok("List sucessfully deleted");
        }

        return NotFound("List not found or userId dosn't match");
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

    [HttpPut]
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

    [HttpPut]
    [Route("UpdateUserListTitle")]
    public async Task<IActionResult> UpdateUserListTitle(UpdateUserListDto dto)
    {
        if (ModelState.IsValid)
        {
            if (!string.IsNullOrEmpty(dto.NewTitle.TrimEnd()))
            {
                var result = _listService.UpdateUserListTitle(dto);
                if (result != null)
                {
                    return Ok(result);
                }
                return Problem("Could not update list title");
            }
            return BadRequest("Title input can't be empty!");
        }
        return BadRequest("Invalid information received, try again");
    }

    [HttpDelete]
    [Route("DeleteUserListContent")]
    public async Task<IActionResult> DeleteUserListContent(DeleteListItemDto dto)
    {
        if (ModelState.IsValid)
        {
            var result = _listService.DeleteUserListContent(dto);
            if (result != false)
            {
                return Ok("List content removed");
            }
            return Problem("Could not delete list content");
        }
        return BadRequest("Invalid information received, try again");
    }
}
