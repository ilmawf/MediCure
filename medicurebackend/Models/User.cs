using System.ComponentModel.DataAnnotations;

namespace medicurebackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }  // Store password as hashed value for security
        public required string Role { get; set; }  // E.g., admin, patient, staff
    }
}
