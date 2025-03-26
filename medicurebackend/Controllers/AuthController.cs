using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using medicurebackend.Models;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace medicurebackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HospitalContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(HospitalContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register a new user (Admin, Doctor, Patient, etc.)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "User already exists!" });
            }

            var user = new User
            {
                Username = userDTO.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password),  // Hash the password before saving
                Role = userDTO.Role  // Assign role from userDTO
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "User registered successfully!" });
        }

        // Login (Generate and return JWT token)
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO userDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == userDTO.Username);

            // Validate credentials
            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password))  // Verify hashed password
            {
                return Unauthorized(new { Message = "Invalid credentials!" });
            }

            // Generate and return the JWT token
            var token = GenerateJwtToken(user);
            return Ok(new AuthResponseDTO { Token = token });
        }

        // Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString() ?? string.Empty),  
                new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? string.Empty)  // Assign role claim
            };

            var key = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("JWT Key is missing from configuration.");
            }

            var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),  // Token expiry time
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
