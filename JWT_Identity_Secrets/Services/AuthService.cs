using Amazon.Runtime.Internal;
using Azure.Core;
using JWT_Identity_Secrets.DTOS;
using JWT_Identity_Secrets.Models;
using JWT_Identity_Secrets.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace JWT_Identity_Secrets.Services
{
    public class AuthService : IAuthService
    {
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }
        public async Task RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (existingUser != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                Username = request.Username,
                Role = request.Role
            };

            // Hash password using ASP.NET Identity PasswordHasher
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            await _userRepository.AddUserAsync(user);
        }
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (user == null)
                throw new Exception("Invalid user");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid password");

            // Fetching all old refresh token information for particular user
            var oldTokens = await _userRepository.GetRefreshTokenByUserIdAsync(user.Id);

            // Deleting all the refresh tokens for that particular user
            await _userRepository.RemoveRefreshTokenForUserAsync(oldTokens);

            var accessToken = _tokenService.GenerateAccessToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();

            var refresh = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _userRepository.SaveRefreshTokenAsync(refresh);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Message = "User logged in successfully"
            };
        }

        public async Task LogoutAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);

            if (user == null) 
                throw new Exception("User does not exist");

            // check all the assigned tokens to the particular user.
            ICollection<RefreshToken> tokens = await _userRepository.GetRefreshTokenByUserIdAsync(user.Id);

            // Delete all the tokens from the database.
            await _userRepository.RemoveRefreshTokenForUserAsync(tokens);
        }

        public async Task<UpdateResponseDto> UpdateAsync(UpdateRequestDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User object cannot be null.");

            var existingUser = await _userRepository.GetUserByUsernameAsync(user.Username);

            if (existingUser == null)
                throw new Exception("User does not exist.");

            existingUser.Username = user.Username;
            existingUser.Role = user.Role;

            // Hash password using ASP.NET Identity PasswordHasher
            var passwordHash = _passwordHasher.HashPassword(existingUser, user.Password);

            if(passwordHash != existingUser.PasswordHash)
            {
                existingUser.PasswordHash = passwordHash;
            }


            //Need to clear all the tokens linked with particular user.
            // check all the assigned tokens to the particular user.
            var tokens = await _userRepository.GetRefreshTokenByUserIdAsync(existingUser.Id);

            // Delete all the tokens from the database.
            await _userRepository.RemoveRefreshTokenForUserAsync(tokens);

            await _userRepository.UpdateUserAsync(existingUser);

            return new UpdateResponseDto
            {
                Username = existingUser.Username,
                Role = existingUser.Role,
                Message = "User updated successfully"
            };

        }
        public async Task DeleteAsync(string username)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(username);

            if (existingUser == null)
            {
                throw new Exception("User does not exist or already deleted");
            }

            var tokens = await _userRepository.GetRefreshTokenByUserIdAsync(existingUser.Id);

            await _userRepository.RemoveRefreshTokenForUserAsync(tokens);

            await _userRepository.DeleteUserAsync(existingUser);
        }

        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto token)
        {
            var refreshToken = await _userRepository.GetRefreshTokenAsync(token.RefreshToken);

            if (refreshToken == null || refreshToken.IsRevoked || refreshToken.ExpiryDate < DateTime.UtcNow)
                throw new SecurityTokenException("Invalid refresh token");

            refreshToken.IsRevoked = true;

            await _userRepository.UpdateRefreshTokenAsync(refreshToken);

            var user = refreshToken.User;

            if (refreshToken.IsRevoked)
            {
                // Fetching old refresh token information for particular user
                var oldTokens = await _userRepository.GetRefreshTokenByUserIdAsync(user.Id);

                // Deleting all the refresh tokens for that particular user
                await _userRepository.RemoveRefreshTokenForUserAsync(oldTokens);
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            var refresh = new RefreshToken
            {
                Token = newRefreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            await _userRepository.SaveRefreshTokenAsync(refresh);

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
