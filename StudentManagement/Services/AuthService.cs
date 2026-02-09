using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagement.Data;
using StudentManagement.DTOs;
using StudentManagement.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagement.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(UserManager<IdentityUser> userManager, IConfiguration configuration, ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<AuthResultDto> Login(LoginDto loginDto)
        {
            // 1. Find User
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new List<string> { "Invalid login request" }
                };
            }

            return await GenerateJwtToken(user);
        }

        public async Task<bool> RegisterUser(RegisterDto registerDto)
        {
            var user = new IdentityUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                var role = registerDto.Role;
                if (role != "Admin" && role != "User")
                {
                    role = "User";
                }

                await _userManager.AddToRoleAsync(user, role);

                return true;
            }

            // If result.Succeeded is true, the user is saved in SQL DB
            return result.Succeeded;
        }

        private async Task<AuthResultDto> GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            // 1. Create the Refresh Token
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id, // Link it to the Access Token
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                Token = RandomString(35) + Guid.NewGuid()
            };

            // 2. Save to Database
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            // 3. Return Both
            return new AuthResultDto
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                Success = true
            };
        }

        private string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public async Task<AuthResultDto> VerifyAndGenerateToken(TokenRequestDto tokenRequestDto)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // 1. Validation 1: Check JWT format
                var tokenVerification = jwtTokenHandler.ValidateToken(tokenRequestDto.Token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false, // allow expired tokens here!
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                // 2. Validation 2: Encryption Algo Check
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
                    if (!result) return new AuthResultDto { Success = false, Errors = new List<string> { "Invalid tokens" } };
                }

                // 3. Validation 3: Check Expiry Date of the JWT
                var utcExpiryDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // Convert Unix timestamp to DateTime
                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(utcExpiryDate).UtcDateTime;

                if (expiryDate > DateTime.UtcNow)
                {
                    return new AuthResultDto { Success = false, Errors = new List<string> { "This token has not expired yet" } };
                }

                // 4. Validation 4: Check if Refresh Token exists in DB
                var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestDto.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResultDto { Success = false, Errors = new List<string> { "Invalid refresh token" } };
                }

                // 5. Validation 5: Check if used or revoked
                if (storedToken.IsUsed)
                {
                    return new AuthResultDto { Success = false, Errors = new List<string> { "Invalid refresh token" } };
                }

                if (storedToken.IsRevoked)
                {
                    return new AuthResultDto { Success = false, Errors = new List<string> { "Invalid refresh token" } };
                }

                // 6. Validation 6: Check ID match (Jti)
                var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (storedToken.JwtId != jti)
                {
                    return new AuthResultDto { Success = false, Errors = new List<string> { "Invalid refresh token" } };
                }

                // --- VALIDATION PASSED ---

                // 7. Mark the old token as USED
                storedToken.IsUsed = true;
                _context.RefreshTokens.Update(storedToken);
                await _context.SaveChangesAsync();

                // 8. Generate NEW tokens
                var dbUser = await _userManager.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);
            }
            catch (Exception ex)
            {
                return new AuthResultDto { Success = false, Errors = new List<string> { "Server error" } };
            }
        }
    }
}
