namespace MyListApp_API.Models;

public class ListItemDto
{
    //Vet ej om UserId kommer behövas innan Account delen är klar samt om annan typ (int, string)
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public string Content { get; set; } = null!;
}
