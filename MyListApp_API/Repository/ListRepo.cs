using MyListApp_API.Models;
using System.Diagnostics.CodeAnalysis;

namespace MyListApp_API.Repository;

[ExcludeFromCodeCoverage]
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
                Id = new Guid("c3a9e351-ed6b-4d36-84c0-7d29af59ad1b"),
                Title = "Att göra",
                ListContent =
                {
                    "Städa", "Handla", "Träna"
                },
                UserId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed")
            },
            new UserList
            {
                Id = new Guid("a2e4b812-3429-4e2a-9187-516c07a51d87"),
                Title = "Fixa inför fest",
                ListContent =
                {
                    "Städa", "Handla"
                },
                UserId = new Guid("cf9daebc-30ad-44fd-83bb-fa26cb47c14a")
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
