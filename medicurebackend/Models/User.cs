public class User
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }  // Store hashed password
    public string? Role { get; set; }  
}