namespace MyListApp_API.Models;

public class UserList
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public List<ListItem> List { get; set; } = null!;

    public UserList()
    {
        Id = Guid.NewGuid();
        List = new List<ListItem>();
    }
}
