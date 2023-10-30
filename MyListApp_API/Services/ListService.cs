using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using System.Reflection.Metadata.Ecma335;

namespace MyListApp_API.Services;

public class ListService : IListService
{
    private readonly ListRepo _listRepo;
    private readonly IUserService _userService;

    public ListService(ListRepo listRepo, IUserService userService)
    {
        _listRepo = listRepo;
        _userService = userService;
    }

    public UserList CreateUserList(UserListDto dto)
    {
        if (dto != null && dto.Title.Length <= 25)
        {
            var user = _userService.GetUserById(dto.UserId);

            if (user != null)
            {
                var userList = new UserList();
                userList = dto;
                _listRepo.UserList.Add(userList);
                return userList;
            }
        }
        return null;
    }

    public UserList AddToUserList(ListItemDto dto)
    {
        if (dto != null && dto.Content.Length <= 80)
        {
            var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
            if (listResult != null)
            {
                listResult.ListContent.Add(dto.Content);
                return listResult;
            }
        }
        return null;
    }
    //Get Lists
    public List<UserList> GetAllLists()
    {
        return _listRepo.UserList;
    }

    public bool DeleteList(DeleteUserListDto dto)
    {
        var listToRemove = _listRepo.UserList.FirstOrDefault(x => x.Id == dto.UserListId && x.UserId == dto.UserId);

        if(listToRemove == null) 
        {
            return false;   //hittar ej lista eller id som matchar
        }
        _listRepo.UserList.Remove(listToRemove);
        return true; //listan borttagen
    }
    public List<UserList> GetListsByUserId(Guid userId)
    {
        throw new NotImplementedException();

    }
}