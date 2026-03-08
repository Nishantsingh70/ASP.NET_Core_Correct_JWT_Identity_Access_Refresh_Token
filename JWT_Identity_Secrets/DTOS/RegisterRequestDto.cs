using System.ComponentModel.DataAnnotations;

namespace JWT_Identity_Secrets.DTOS
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; } = "User";
    }
}
