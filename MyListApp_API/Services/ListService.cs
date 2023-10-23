using MyListApp_API.Models;

namespace MyListApp_API.Services;

public class ListService
{
    // Fake database for lists all users create
    private readonly List<UserList> _userLists;

    public ListService()
    {
        _userLists = new List<UserList>
        {
            new UserList
            {
                Id = Guid.NewGuid(), 
                Title = "Att göra",
                ListContent =
                {
                    "Städa", "Handla", "Träna"
                },
                UserId = Guid.NewGuid() //vet inte än om det ska vara en string eller inte eller likanande  
            },
            new UserList
            {
                Id = Guid.NewGuid(),
                Title = "Fixa inför fest",
                ListContent =
                {
                    "Städa", "Handla"
                },
                UserId = Guid.NewGuid() //vet inte än om det ska vara en string eller inte eller likanande  
            }
        };
    }

    public UserList CreateUserList(UserListDto dto)
    {
        if(dto != null)
        {
            var userList = new UserList
            {
                Title = dto.Title
            };
            _userLists.Add(userList);

            return userList;
        }
        return null;
    }

    public bool AddToUserList(string item, Guid listId, Guid userId)
    {
        if(item != null)
        {
            try
            {
                 //Hämnta användare med userId, hitta listan med lisId och lägg till item
            }
            catch
            {

            }
        }
        return false;
    }
}
