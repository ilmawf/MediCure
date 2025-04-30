namespace medicurebackend.Models
{
    public class User
    {
        public int UserId { get; set; }          // Unique User identifier
        public string? Username { get; set; }     // Username
        public string? Password { get; set; }     // Password

        // Assuming you have other properties like Role
        public string? Role { get; set; }
    }
}
