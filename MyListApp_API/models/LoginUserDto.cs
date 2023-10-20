using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.Models
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
