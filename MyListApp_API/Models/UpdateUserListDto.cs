using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.Models;

public class UpdateUserListDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid UserListId { get; set; }
    [Required]
    public string NewTitle { get; set; } = null!;

    public bool CheckValidAmountOfCharactersForTitle(string title)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            if (title.Length <= 25)
                return true;
        }
        return false;
    }
}
