using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWT_Identity_Secrets.Models

{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage="Please enter username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Please enter role")]
        public string Role { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
