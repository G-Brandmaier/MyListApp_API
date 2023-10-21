using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.Models
{
    public class UpdatePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        public string NewPassword { get; set; }=null!;
    }
}
