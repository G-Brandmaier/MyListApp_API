namespace MyListApp_API.Models;

public class UpdateListItemDto
{
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public int ContentPosition { get; set; }
    public string NewContent { get; set; } = null!;


    public bool CheckValidAmountOfCharactersForNewContent(string newContent)
    {
        if (newContent.Length > 80)
            return false;
        else
            return true;
    }
    public bool CheckValidContentPosition(int position)
    {
        if (position != 0)
            return true;
        else
            return false;
    }

}
