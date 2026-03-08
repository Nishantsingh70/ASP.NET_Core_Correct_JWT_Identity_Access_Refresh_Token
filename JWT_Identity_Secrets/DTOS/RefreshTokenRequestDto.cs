using System.ComponentModel.DataAnnotations;

namespace JWT_Identity_Secrets.DTOS
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}
