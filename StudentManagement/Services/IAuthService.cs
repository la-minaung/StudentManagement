using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(RegisterDto registerDto);
        Task<string?> Login(LoginDto loginDto);
    }
}
