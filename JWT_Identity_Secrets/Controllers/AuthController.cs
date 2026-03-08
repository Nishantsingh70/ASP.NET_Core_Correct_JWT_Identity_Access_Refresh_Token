using JWT_Identity_Secrets.DTOS;
using JWT_Identity_Secrets.Models;
using JWT_Identity_Secrets.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Identity_Secrets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            await _authService.RegisterAsync(request);

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string username)
        {

            await _authService.LogoutAsync(username);
            return Ok("User logged out successfully");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(UpdateRequestDto request)
        {
            var result = await _authService.UpdateAsync(request);
            return Ok(result);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string username)
        {
            await _authService.DeleteAsync(username);
            return Ok("User deleted successfully");
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);

            return Ok(result);
        }

    }
}
