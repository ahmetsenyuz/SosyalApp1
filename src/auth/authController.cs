using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SosyalApp1.src.auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings _appSettings;

        public AuthController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            // In a real implementation, this would involve:
            // 1. Validating the input data
            // 2. Hashing the password
            // 3. Saving the user to database
            // 4. Generating a JWT token
            
            // For now, we'll simulate a successful registration
            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            // In a real implementation, this would involve:
            // 1. Validating the input data
            // 2. Verifying credentials against database
            // 3. Generating a JWT token
            
            // For now, we'll simulate a successful login
            var token = GenerateJwtToken(model.Username);
            return Ok(new { token });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // In a real implementation, this would invalidate the session/token
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            // In a real implementation, this would return user profile data
            // based on the authenticated user
            return Ok(new { 
                username = "testuser",
                totalPoints = 100,
                completedTasksCount = 5
            });
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("username", username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
    }
}