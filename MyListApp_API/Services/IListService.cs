using MyListApp_API.models;
using MyListApp_API.Models;


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