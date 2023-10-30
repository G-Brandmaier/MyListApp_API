namespace MyListApp_API.Models;

public class UpdateListItemDto
{
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public int ContentPosition { get; set; }
    public string NewContent { get; set; } = null!;


    public bool CheckValidAmountOfCharactersForNewContent(string newContent)
    {
        if(!string.IsNullOrEmpty(newContent))
        {
            if (newContent.Length <= 80)
                return true;
        }
        return false;
    }
    public bool CheckValidContentPosition(int position, int existingListPositions)
    {
        if (!int.IsNegative(position) && position <= existingListPositions)
        {
            if (position != 0)
                return true;
        }
        return false;
    }

}
