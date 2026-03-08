using JWT_Identity_Secrets.DTOS;
using JWT_Identity_Secrets.Models;
using JWT_Identity_Secrets.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWT_Identity_Secrets.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly DevDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user)
        {
            // If you are using JWTSettings secret key then use below code. 
            //var Issuer = _configuration["JwtSettings:ValidateIssuer"];
            //var Audience = _configuration["JWTSettings:ValidateAudience"];
            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));

            // If you are using Jwt_ConnectionString_Settings secret token then use below code.
            var Issuer = _configuration["Jwt_ConnectionString_Settings:ValidateIssuer"];
            var Audience = _configuration["Jwt_ConnectionString_Settings:ValidateAudience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt_ConnectionString_Settings:SecretKey"]));

            // for giving default role in case of if we are passing null in role.
            var role = user.Role ?? "User";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

    
    }
}
