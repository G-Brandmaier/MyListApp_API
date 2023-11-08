using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Repository;

namespace MyListApp_API.Services;

public class ListService : IListService
{
    private readonly IListRepo _listRepo;
    private readonly IUserService _userService;

    public ListService(IListRepo listRepo, IUserService userService)
    {
        _listRepo = listRepo;
        _userService = userService;
    }

    public UserList? CreateUserList(UserListDto dto)
    {
        if (dto != null && dto.CheckValidAmountOfCharactersForTitle(dto.Title) != false)
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

    public UserList? AddToUserList(ListItemDto dto)
    {
        if (dto != null && dto.CheckValidAmountOfCharactersForContent(dto.Content) != false)
        {
            var user = _userService.GetUserById(dto.UserId);
            if (user != null)
            {
                var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
                if (listResult != null)
                {
                    listResult.ListContent.Add(dto.Content);
                    return listResult;
                }
            }
        }
        return null;
    }
    //Get Lists
    public List<UserList> GetAllLists()
    {
        var lists = _listRepo.UserList;
        return lists ?? new List<UserList>();
    }

    public bool DeleteList(DeleteUserListDto dto)
    {
        if (dto == null)
        {
            return false;
        }

        var listToRemove = _listRepo.UserList.FirstOrDefault(x => x.Id == dto.UserListId && x.UserId == dto.UserId);

        if (listToRemove == null)
        {
            return false;   //hittar ej lista eller id som matchar
        }
        return _listRepo.DeleteUserList(dto.UserListId);
    }


    public List<UserList>? GetAllUserListsById(Guid userId)
    {
        var user = _userService.GetUserById(userId);

        if (user != null)
        {
            return _listRepo.UserList.Where(x => x.UserId == userId).ToList();
        }
        return null;
    }

    public UserList? UpdateUserListContent(UpdateListItemDto dto)
    {
        if (dto != null && dto.CheckValidAmountOfCharactersForNewContent(dto.NewContent) != false)
        {
            var user = _userService.GetUserById(dto.UserId);
            if (user != null)
            {
                var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
                if (listResult != null)
                {
                    if (dto.CheckValidContentPosition(dto.ContentPosition, listResult.ListContent.Count) != false)
                    {
                        listResult.ListContent[dto.ContentPosition - 1] = dto.NewContent;
                        return listResult;
                    }
                }
            }
        }
        return null;
    }

    public UserList? UpdateUserListTitle(UpdateUserListDto dto)
    {
        if (dto != null && dto.CheckValidAmountOfCharactersForTitle(dto.NewTitle) != false)
        {
            var user = _userService.GetUserById(dto.UserId);

            if (user != null)
            {
                var userList = _listRepo.UserList.Where(x => x.UserId == dto.UserId && x.Id == dto.UserListId).SingleOrDefault();
                if (userList != null)
                {
                    userList.Title = dto.NewTitle;
                    return userList;
                }
            }
        }
        return null;
    }

    public bool DeleteUserListContent(DeleteListItemDto dto)
    {
        if (dto != null)
        {
            var user = _userService.GetUserById(dto.UserId);
            if (user != null)
            {
                var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
                if (listResult != null)
                {
                    if (dto.CheckValidContentPosition(dto.ContentPosition, listResult.ListContent.Count) != false)
                    {
                        listResult.ListContent.RemoveAt(dto.ContentPosition - 1);
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
