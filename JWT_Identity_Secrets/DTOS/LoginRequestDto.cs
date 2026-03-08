using System.ComponentModel.DataAnnotations;

namespace JWT_Identity_Secrets.DTOS
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
