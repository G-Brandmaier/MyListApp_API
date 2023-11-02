using System.ComponentModel.DataAnnotations;

namespace MyListApp_API.models
{
    public class UpdateUserDto
    {
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
    }
}
