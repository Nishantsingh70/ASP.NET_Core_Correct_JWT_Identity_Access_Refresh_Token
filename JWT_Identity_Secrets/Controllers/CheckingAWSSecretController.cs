using JWT_Identity_Secrets.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace JWT_Identity_Secrets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckingAWSSecretController : ControllerBase
    {
        private readonly IOptions<DatabaseSetting> _databaseSetting;
        private readonly IOptions<JWTSettings> _jwtSettings;
        private readonly IOptions<Jwt_Connection_Settings> _Jwt_Connection_Settings;
        public CheckingAWSSecretController(IOptions<DatabaseSetting> databaseSetting, IOptions<JWTSettings> jWTSettings, IOptions<Jwt_Connection_Settings> jwt_Connection_Settings)
        {
             _databaseSetting = databaseSetting;
            _jwtSettings = jWTSettings;
            _Jwt_Connection_Settings = jwt_Connection_Settings;
        }

        [HttpGet("connection")]
        public IActionResult GetConnection()
        {
             var settings = _databaseSetting.Value;
             return Ok(settings);
        }

        [HttpGet("jwt")]
        public IActionResult GetJWT()
        {
            var settings = _jwtSettings.Value;
            return Ok(settings);
        }

        [HttpGet("jwt_connection")]
        public IActionResult GetJWT_Connection()
        {
            var settings = _Jwt_Connection_Settings.Value;
            return Ok(settings);
        }

    }
}
