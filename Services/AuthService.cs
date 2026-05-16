using EmployeeApi.Data;
using EmployeeApi.DTOs.Auth;
using EmployeeApi.Exceptions;
using EmployeeApi.Helpers;
using EmployeeApi.Models;
using EmployeeApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
//using EmployeeApi.Services.Interfaces;

namespace EmployeeApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthService(ApplicationDbContext context, JwtTokenGenerator tokenGenerator)
    {
        _context = context;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // Check if email exists
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists)
            throw new BadRequestException("Email already registered.");

        // Hash password
        var passwordHash = HashPassword(dto.Password);

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = "User" // default role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Generate token
        var token = _tokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            throw new NotFoundException("Invalid email or password.");

        if (!VerifyPassword(dto.Password, user.PasswordHash))
            throw new BadRequestException("Invalid email or password.");

        var token = _tokenGenerator.GenerateToken(user);

        return new AuthResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = user.Role
        };
    }

    // -------------------------
    // Password Hashing Helpers
    // -------------------------

    private string HashPassword(string password)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        return hash == storedHash;
    }
}
