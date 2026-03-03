using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shiply.Application.DTOs;
using Shiply.Application.Interfaces;
using Shiply.Domain.Models;
using Shiply.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            return "User not found"; 
        }

        string role = user is Customer ? "Customer" : "Driver";

        return GenerateToken(user, role);
    }

    private string GenerateToken(User user, string role)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> RegisterAsync(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return false;

        User newUser;

        if (dto.UserType.ToLower() == "driver")
        {
            newUser = new Driver
            {
                LicenseNumber = dto.LicenseNumber ?? "N/A"
            };
        }
        else
        {
            newUser = new Customer
            {
                Address = dto.Address ?? "N/A"
            };
        }

        newUser.Id = Guid.NewGuid();
        newUser.Email = dto.Email;
        newUser.FirstName = dto.FirstName;
        newUser.LastName = dto.LastName;

        newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return true;
    }
}