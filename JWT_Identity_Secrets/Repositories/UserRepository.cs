using JWT_Identity_Secrets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace JWT_Identity_Secrets.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DevDbContext _context;

        public UserRepository(DevDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUserIdAsync(int userId)
        {
            var existinguser = await _context.Users.FindAsync(userId);
            return existinguser;
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            var existinguser = await GetUserByUserIdAsync(user.Id);
            _context.Users.Remove(existinguser);
            await _context.SaveChangesAsync();
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<ICollection<RefreshToken>> GetRefreshTokenByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
        }
        public async Task UpdateRefreshTokenAsync(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRefreshTokenForUserAsync(ICollection<RefreshToken> token)
        {
            _context.RefreshTokens.RemoveRange(token);
            await _context.SaveChangesAsync();
        }
    }
}
