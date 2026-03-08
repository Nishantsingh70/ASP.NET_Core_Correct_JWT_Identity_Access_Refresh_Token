using JWT_Identity_Secrets.DTOS;
using JWT_Identity_Secrets.Models;

namespace JWT_Identity_Secrets.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();
    }
}
