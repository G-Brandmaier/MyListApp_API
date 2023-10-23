using MyListApp_API.Models;
using MyListApp_API.Repository;

namespace MyListApp_API.Services;

public interface IListService
{
    UserList CreateUserList(UserListDto dto);

    UserList AddToUserList(ListItemDto dto);
}
