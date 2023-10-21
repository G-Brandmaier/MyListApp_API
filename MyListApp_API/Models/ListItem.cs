namespace MyListApp_API.Models;

public class ListItem
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;

    public ListItem()
    {
        Id = Guid.NewGuid();
    }

}
