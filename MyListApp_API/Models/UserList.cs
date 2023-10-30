using Microsoft.AspNetCore.Http.HttpResults;

namespace MyListApp_API.Models;

public class UserList
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public List<string> ListContent { get; set; } = new List<string>();
    public Guid UserId { get; set; }
}
