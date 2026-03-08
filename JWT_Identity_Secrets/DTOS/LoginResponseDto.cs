using System.ComponentModel.DataAnnotations;

namespace JWT_Identity_Secrets.DTOS
{
    public class LoginResponseDto
    {
        [Required(ErrorMessage = "Access Token is required")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "Refresh Token is required")]
        public string RefreshToken { get; set; }

        public string Message { get; set; }
    }
}
