//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CampusFlow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            if(loginModel.Username == "admin" &&  loginModel.Password == "admin")
            {
                var token = GenerateJwtToken("admin");
                return Ok(new { token });
            }
            else if (loginModel.Username == "teacher" && loginModel.Password == "teacher")
            {
                var token = GenerateJwtToken("teacher");
                return Ok(new { token });
            }
            else if (loginModel.Username == "student" && loginModel.Password == "student")
            {
                var token = GenerateJwtToken("student");
                return Ok(new { token });
            }

            return Unauthorized("Invalid Credentials or user");
        }

        private string GenerateJwtToken(string role)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, role),
            new Claim(ClaimTypes.Role, role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginModel
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
