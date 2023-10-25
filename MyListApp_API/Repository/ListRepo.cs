using MyListApp_API.Models;

namespace MyListApp_API.Repository;

public class ListRepo
{
    public List<UserList> UserList { get; set; }

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
}
