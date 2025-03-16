using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using medicurebackend.Models; 

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
            // You can add logic here to check if the user already exists
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

            if (user == null || !BCrypt.Net.BCrypt.Verify(userDTO.Password, user.Password)) // Verify hashed password
            {
                return Unauthorized(new { Message = "Invalid credentials!" });
            }

            var token = GenerateJwtToken(user);
            return Ok(new AuthResponseDTO { Token = token });
        }

        // Generate JWT Token
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)  // Add the role to the claims
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),  // Token expiry time
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
