using Shiply.Application.DTOs;


namespace Shiply.Application.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginDto loginDto);

        Task<bool> RegisterAsync(RegisterDto registerDto);
    }
}
