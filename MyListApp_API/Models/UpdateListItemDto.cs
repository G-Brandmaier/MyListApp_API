namespace MyListApp_API.Models;

public class UpdateListItemDto
{
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public int ContentPosition { get; set; }
    public string NewContent { get; set; } = null!;

}
