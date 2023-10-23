namespace MyListApp_API.Models;

public class UserList
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public List<string> ListContent { get; set; } = null!;
    public Guid UserId { get; set; }
}
