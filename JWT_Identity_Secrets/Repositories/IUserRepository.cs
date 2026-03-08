using JWT_Identity_Secrets.Models;

namespace JWT_Identity_Secrets.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserIdAsync(int  userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken> GetRefreshTokenAsync(string token);
        Task<ICollection<RefreshToken>> GetRefreshTokenByUserIdAsync(int userId);
        Task UpdateRefreshTokenAsync(RefreshToken token);
        Task RemoveRefreshTokenForUserAsync(ICollection<RefreshToken> token);
    }
}
