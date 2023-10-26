namespace MyListApp_API.Models;

public class ListItemDto
{
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public string Content { get; set; } = null!;
}
