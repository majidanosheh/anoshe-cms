// File: AnosheCms.Infrastructure/Services/AuthService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Auth;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            ApplicationDbContext context,
            IConfiguration config)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _context = context;
            _config = config;
        }

        public async Task<AuthenticationResult> LoginAsync(LoginRequest request, string ipAddress, string userAgent)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !user.IsActive || user.IsDeleted)
            {
                return FailResult("ایمیل یا رمز عبور نامعتبر است.");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                await LogLoginHistory(user.Id, ipAddress, userAgent, false, "Invalid password");
                return FailResult("ایمیل یا رمز عبور نامعتبر است.");
            }

            user.LastLoginDate = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var (accessToken, refreshToken, jwtId) = await _tokenService.GenerateTokensAsync(user, ipAddress, userAgent);

            await LogLoginHistory(user.Id, ipAddress, userAgent, true, null);
            await CreateSession(user.Id, jwtId.ToString(), refreshToken, ipAddress, userAgent);

            return await SuccessResult(user, accessToken, refreshToken);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(TokenRequest request, string ipAddress, string userAgent)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            if (principal == null)
            {
                return FailResult("توکن نامعتبر است.");
            }

            var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (jti == null)
            {
                return FailResult("JTI (شناسه توکن) یافت نشد.");
            }

            var storedRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (storedRefreshToken == null)
                return FailResult("Refresh token وجود ندارد.");
            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
                return FailResult("Refresh token منقضی شده است.");
            if (storedRefreshToken.IsRevoked)
                return FailResult("Refresh token باطل شده است.");
            if (storedRefreshToken.IsUsed)
                return FailResult("Refresh token قبلاً استفاده شده است.");
            if (storedRefreshToken.JwtId != jti)
                return FailResult("توکن‌ها با هم مطابقت ندارند.");

            storedRefreshToken.IsUsed = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(principal.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var (accessToken, newRefreshToken, newJwtId) = await _tokenService.GenerateTokensAsync(user, ipAddress, userAgent);

            await CreateSession(user.Id, newJwtId.ToString(), newRefreshToken, ipAddress, userAgent);

            return await SuccessResult(user, accessToken, newRefreshToken);
        }

        public async Task<bool> RevokeTokenAsync(string token, string ipAddress, string userAgent)
        {
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
            if (refreshToken == null || refreshToken.IsRevoked)
                return false;

            refreshToken.IsRevoked = true;
            var session = await _context.UserSessions.FirstOrDefaultAsync(s => s.RefreshToken == token);
            if (session != null)
            {
                session.IsActive = false;
                session.ExpiresAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // --- متدهای کمکی (Helper Methods) ---

        private async Task LogLoginHistory(Guid userId, string ipAddress, string userAgent, bool isSuccessful, string failureReason)
        {
            var history = new UserLoginHistory
            {
                UserId = userId,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsSuccessful = isSuccessful,
                FailureReason = failureReason ?? string.Empty,
                Browser = "Unknown",
                OS = "Unknown",
                Device = "Unknown"
            };
            await _context.UserLoginHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }

        private async Task CreateSession(Guid userId, string jwtId, string refreshToken, string ipAddress, string userAgent)
        {
            var session = new UserSession
            {
                UserId = userId,
                SessionId = jwtId,
                RefreshToken = refreshToken,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsActive = true,
                LastActivityAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.Add(TimeSpan.FromDays(Convert.ToDouble(_config["JwtSettings:RefreshTokenLifetimeDays"] ?? "7")))
            };
            await _context.UserSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        private async Task<AuthenticationResult> SuccessResult(ApplicationUser user, string accessToken, string refreshToken)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthenticationResult
            {
                Succeeded = true,
                // (اصلاح شد: استفاده از AccessToken)
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email,
                FullName = $"{user.FirstName} {user.LastName}",
                Roles = roles.ToList()
            };
        }

        private AuthenticationResult FailResult(string error)
        {
            return new AuthenticationResult
            {
                Succeeded = false,
                Errors = new[] { error }
            };
        }
    }
}