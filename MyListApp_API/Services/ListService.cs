using MyListApp_API.Models;
using MyListApp_API.Repository;

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

    public UserList? CreateUserList(UserListDto dto)
    {
        if(dto != null && dto.Title.Length <= 25)
        {
            var user = _userService.GetUserById(dto.UserId);

            if(user != null)
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
        if(dto != null && dto.Content.Length <= 80)
        {
            var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
            if(listResult != null)
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

    //Delete list
    public bool DeleteList(Guid listId)
    {
        var listRemove = _listRepo.UserList.FirstOrDefault(x => x.Id == listId);
        if(listRemove != null)
        {
            return false; //list is not found
        }
        _listRepo.UserList.Remove(listRemove);
        return true; // list sucessfully moved
    }

    public List<UserList>? GetAllUserListsById(Guid userId)
    {
        var user = _userService.GetUserById(userId);

        if(user != null)
        {
            return _listRepo.UserList.Where(x => x.UserId == userId).ToList();  
        }
        return null;
    }

    public UserList? UpdateUserListContent(UpdateListItemDto dto)
    {
        var user = _userService.GetUserById(dto.UserId);
        if(user != null && dto.ContentPosition != 0 && dto.NewContent.Length <= 80)
        {
            var listResult = _listRepo.UserList.Where(x => x.Id == dto.UserListId && x.UserId == dto.UserId).SingleOrDefault();
            if(listResult != null)
            {
                listResult.ListContent[dto.ContentPosition -1] = dto.NewContent;
                return listResult;
            }
        }
        return null;
    }
}
