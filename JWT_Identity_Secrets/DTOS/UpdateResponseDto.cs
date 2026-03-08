using System.ComponentModel.DataAnnotations;

namespace JWT_Identity_Secrets.DTOS
{
    public class UpdateResponseDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role { get; set; }

        public string Message { get; set; }
    }
}
