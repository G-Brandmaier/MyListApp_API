using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.Models
{
    public class UpdatePasswordDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        public string NewPassword { get; set; } = null!;
       
     
        public bool CheckPasswordStrength()
        {
            // Ett enkelt exempel, du skulle använda en mycket mer robust lösning i verkligheten
            return NewPassword.Length >= 8 && NewPassword.Any(char.IsDigit);
        }

        public bool ConfirmPasswordChange()
        {
            return !string.Equals(CurrentPassword, NewPassword, StringComparison.Ordinal);
        }

    }
}
