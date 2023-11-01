namespace MyListApp_API.Models;

public class DeleteListItemDto
{
    public Guid UserId { get; set; }
    public Guid UserListId { get; set; }
    public int ContentPosition { get; set; }

    public bool CheckValidContentPosition(int position, int existingListPositions)
    {
        if(existingListPositions != 0 && !int.IsNegative(existingListPositions))
        {
            if (!int.IsNegative(position) && position <= existingListPositions)
            {
                if (position != 0)
                    return true;
            }
        }
        return false;
    }
}
