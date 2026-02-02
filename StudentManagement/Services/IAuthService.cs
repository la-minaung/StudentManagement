using StudentManagement.DTOs;

namespace StudentManagement.Services
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(RegisterDto registerDto);
        Task<AuthResultDto> Login(LoginDto loginDto);
        Task<AuthResultDto> VerifyAndGenerateToken(TokenRequestDto tokenRequestDto);
    }
}
