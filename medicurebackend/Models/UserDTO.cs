
public class UserDTO
{
    public string? Username { get; set; }  // Username for the user
    public string? Password { get; set; }  // User's password (should be hashed)
    public string? Role { get; set; }      // Role of the user 
}

public class AuthResponseDTO
{
    public string? Token { get; set; }  // JWT token that will be returned after successful login
}
