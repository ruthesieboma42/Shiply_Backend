using Microsoft.AspNetCore.Mvc;
using Shiply.Application.DTOs;
using Shiply.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);

        if (token == null)
            return Unauthorized("Invalid credentials.");

        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var result = await _authService.RegisterAsync(dto);
        if (!result) return BadRequest("User already exists or registration failed.");

        return Ok("User registered successfully.");
    }
}