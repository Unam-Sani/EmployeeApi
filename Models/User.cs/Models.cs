namespace EmployeeApi.Models;

public class User
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // Hashed password, never store plain text
    public string PasswordHash { get; set; } = string.Empty;

    // Example: "Admin", "User"
    public string Role { get; set; } = "User";
}

