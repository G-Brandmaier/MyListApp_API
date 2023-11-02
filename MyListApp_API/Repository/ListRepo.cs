using MyListApp_API.Models;

namespace MyListApp_API.Repository;

public class ListRepo : IListRepo
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
                UserId = new Guid("2cf4e09e-7858-40be-8e26-569117928bed")
            },
            new UserList
            {
                Title = "Fixa inför fest",
                ListContent =
                {
                    "Städa", "Handla"
                },
                UserId = new Guid("cf9daebc-30ad-44fd-83bb-fa26cb47c14a")
            }
        };
    }
}
