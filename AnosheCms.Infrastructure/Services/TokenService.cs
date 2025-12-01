// File: AnosheCms.Infrastructure/Services/TokenService.cs
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SymmetricSecurityKey _jwtKey;

        public TokenService(
            IConfiguration config,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            RoleManager<ApplicationRole> roleManager)
        {
            _config = config;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _jwtKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Secret"]));
        }

        public async Task<(string AccessToken, string RefreshToken, Guid JwtId)> GenerateTokensAsync(
            ApplicationUser user,
            string ipAddress,
            string userAgent)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
                new Claim("given_name", user.FirstName ?? ""),
                new Claim("family_name", user.LastName ?? "")
            };

            // این خط باعث می‌شود اگر به کاربری دسترسی خاص دادید، اعمال شود
            var userDirectClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userDirectClaims.Where(c => c.Type == "Permission"));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims.Where(c => c.Type == "Permission"));
                }
            }

            // ---*** بلوک اصلاح شده ***---
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(Convert.ToDouble(_config["JwtSettings:TokenLifetimeMinutes"] ?? "15"))),
                // ۱. الگوریتم صحیح (256) و پرانتز بسته اضافه شد
                SigningCredentials = new SigningCredentials(_jwtKey, SecurityAlgorithms.HmacSha256),
                // ۲. Issuer و Audience به درستی در خارج از SigningCredentials قرار گرفتند
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };
            // ---*** پایان اصلاح ***---

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                IsUsed = false,
                IsRevoked = false,
                ExpiryDate = DateTime.UtcNow.Add(TimeSpan.FromDays(Convert.ToDouble(_config["JwtSettings:RefreshTokenLifetimeDays"] ?? "7"))),
                IpAddress = ipAddress,
                UserAgent = userAgent
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return (accessToken, refreshToken.Token, Guid.Parse(token.Id));
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _jwtKey,
                ValidateIssuer = true,
                ValidIssuer = _config["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JwtSettings:Audience"],
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }
    }
}