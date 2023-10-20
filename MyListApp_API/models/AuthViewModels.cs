using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.Models
{
    public class AuthViewModels
    {
        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            public string? Password { get; set; }
        }

        public class LoginViewModel
        {
            [Required]
            [EmailAddress]
            public string? Email { get; set; }

            [Required]
            public string? Password { get; set; }
        }
    }
}

