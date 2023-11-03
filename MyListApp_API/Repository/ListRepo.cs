using MyListApp_API.Models;
using System.Security.Cryptography.X509Certificates;

namespace MyListApp_API.Repository;

public class ListRepo : IListRepo
{
    public List<UserList> UserList { get; set; }
    public List<UserList> _userLists;        
  
    public ListRepo()
    {
        UserList = new List<UserList>
        {
            new UserList
            {
                Title = "Att göra",
                ListContent =
                {
                    "Städa", "Handla", "Träna"
                },
                UserId = Guid.NewGuid()
            },
            new UserList
            {
                Title = "Fixa inför fest",
                ListContent =
                {
                    "Städa", "Handla"
                },
                UserId = Guid.NewGuid()  
            }
        };
        
    }
    public bool DeleteUserList(Guid userListId)
    {
        var userList = _userLists.FirstOrDefault(l => l.Id == userListId);

        if (userList !=null)
        {
            _userLists.Remove(userList);
            return true;
        }
        return false;

    }


    public void Remove(UserList userList)
    {
        _userLists.Remove(userList);
    }
}
