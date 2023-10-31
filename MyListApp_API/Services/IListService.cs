using MyListApp_API.models;
using MyListApp_API.Models;
using MyListApp_API.Repository;
using System.Collections.Generic;


namespace MyListApp_API.Services;

public interface IListService
{
    UserList CreateUserList(UserListDto dto);

    UserList AddToUserList(ListItemDto dto);

    List<UserList> GetAllLists();

    bool DeleteList(DeleteUserListDto dto);

    List<UserList>? GetAllUserListsById(Guid userId);

    UserList? UpdateUserListContent(UpdateListItemDto dto);

    UserList? UpdateUserListTitle(UpdateUserListDto dto);

    bool DeleteUserListContent(DeleteListItemDto dto);

}