using MyListApp_API.Models;

namespace MyListApp_API.Services;

public class ListService
{
    public UserList CreateListItem(UserListDto dto)
    {
        if(dto != null)
        {
            var userList = new UserList
            {
                Title = dto.Title
            };
            return userList;
        }
        return null;
    }

    public ListItem AddListItemToList(ListItem item, Guid listId, Guid userId)
    {
        if(item != null)
        {
            try
            {
                ListItem listItem; //Hämnta användare med userId, hitta listan med lisId och lägg till item
            }
            catch
            {

            }
        }
        return null;
    }
}
