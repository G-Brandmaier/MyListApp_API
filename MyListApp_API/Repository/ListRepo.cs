using MyListApp_API.Models;

namespace MyListApp_API.Repository;

// Fake database for lists all users create
public class ListRepo
{
    internal static readonly List<UserList> UserLists;

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
                UserId = Guid.NewGuid() //vet inte än om det ska vara en string eller inte eller likanande  
            },
            new UserList
            {
                Title = "Fixa inför fest",
                ListContent =
                {
                    "Städa", "Handla"
                },
                UserId = Guid.NewGuid() //vet inte än om det ska vara en string eller inte eller likanande  
            }
        };
    }
}
