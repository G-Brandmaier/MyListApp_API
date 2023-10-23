namespace MyListApp_API.Models;

public class UserListDto
{
    //Vet ej om UserId kommer behövas innan Account delen är klar samt om annan typ (int, string)
    public Guid UserId { get; set; }
    public string Title { get; set; } = null!;
}
