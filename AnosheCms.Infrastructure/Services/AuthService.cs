// File: AnosheCms.Infrastructure/Services/AuthService.cs

// --- شروع Using Directives ---
using AnosheCms.Application.Interfaces; // <-- حیاتی: برای IAuthService
using AnosheCms.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System; // برای DateTime, Convert, InvalidOperationException
using System.Collections.Generic; // برای List
using System.IdentityModel.Tokens.Jwt;
using System.Linq; // برای .Select
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks; // برای Task
// --- پایان Using Directives ---

namespace AnosheCms.Infrastructure.Services
{
    // --- اطمینان از پیاده‌سازی اینترفیس ---
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponse> LoginAsync(AuthRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new AuthResponse(false, ErrorMessage: "Invalid email or password.");
            }

            if (!user.IsActive || user.IsDeleted)
            {
                return new AuthResponse(false, ErrorMessage: "User account is inactive.");
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // GenerateJwtToken اکنون async است چون نقش‌ها را می‌خواند
            var token = await GenerateJwtToken(user);
            return new AuthResponse(true, Token: token);
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse(false, ErrorMessage: "Email already in use.");
            }

            var newUser = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponse(false, ErrorMessage: errors);
            }

            // TODO: اختصاص نقش پیش‌فرض به کاربر (مثلاً "User")
            // await _userManager.AddToRoleAsync(newUser, "User");

            return new AuthResponse(true);
        }

        // --- متد اصلاح شده برای خواندن نقش‌ها ---
        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new Claim("uid", user.Id.ToString())
            };

            // --- (جدید) افزودن نقش‌های کاربر به Claims ---
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            // --- پایان بخش جدید ---

            var secretKey = _configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("JWT Secret key is not configured in user-secrets.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:DurationInMinutes"] ?? "60"));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}