using Microsoft.AspNetCore.Mvc;
using StudentManagement.DTOs;
using StudentManagement.Services;

namespace StudentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterUser(dto);
            if (result)
            {
                return Ok("User registered successfully");
            }
            return BadRequest("Registration failed. User might already exist.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var authResult = await _authService.Login(dto);

            if (!authResult.Success)
            {
                // Return the errors (e.g. "Invalid password")
                return BadRequest(authResult.Errors);
            }

            // Return the Token AND the Refresh Token
            return Ok(new
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.VerifyAndGenerateToken(tokenRequestDto);

                if (result == null)
                {
                    return BadRequest(new AuthResultDto
                    {
                        Success = false,
                        Errors = new List<string> { "Invalid tokens" }
                    });
                }

                if (result.Success)
                {
                    return Ok(new
                    {
                        Token = result.Token,
                        RefreshToken = result.RefreshToken
                    });
                }

                return BadRequest(result);
            }

            return BadRequest(new AuthResultDto
            {
                Success = false,
                Errors = new List<string> { "Invalid payload" }
            });
        }
    }
}
