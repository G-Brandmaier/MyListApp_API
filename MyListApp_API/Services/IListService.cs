using MyListApp_API.Models;
using MyListApp_API.Repository;
using System.Collections.Generic;


namespace MyListApp_API.Services;

public interface IListService
{
    UserList CreateUserList(UserListDto dto);

    UserList AddToUserList(ListItemDto dto);

    //Add list
    List<UserList> GetAllLists();

    //Delete list
    bool DeleteList(Guid listId);
}