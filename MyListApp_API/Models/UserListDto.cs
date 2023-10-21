namespace MyListApp_API.Models;

public class UserListDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
}
