using System.ComponentModel.DataAnnotations;

namespace medicurebackend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }  // Store password as hashed value for security
        public string Role { get; set; }  // E.g., admin, patient, staff
    }
}
