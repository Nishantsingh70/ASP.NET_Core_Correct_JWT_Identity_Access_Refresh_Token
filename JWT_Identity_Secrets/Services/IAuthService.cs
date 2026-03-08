using JWT_Identity_Secrets.DTOS;
using JWT_Identity_Secrets.Models;

namespace JWT_Identity_Secrets.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task LogoutAsync(string username);
        Task<UpdateResponseDto> UpdateAsync(UpdateRequestDto user);
        Task DeleteAsync(string username);
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto token);
    }
}
