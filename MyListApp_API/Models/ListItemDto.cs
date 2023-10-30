using Microsoft.AspNetCore.Http.HttpResults;

namespace MyListApp_API.Models;

public class ListItemDto
{
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public string Content { get; set; } = null!;

    public bool CheckValidAmountOfCharactersForContent(string content)
    {
        if (content.Length > 80)
            return false;
        else
            return true;
    }
}
