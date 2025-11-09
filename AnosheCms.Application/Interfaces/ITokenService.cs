// File: AnosheCms.Application/Interfaces/ITokenService.cs
using AnosheCms.Domain.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface ITokenService
    {
        // --- (تصحیح شد: پارامترها اضافه شدند) ---
        Task<(string AccessToken, string RefreshToken, Guid JwtId)> GenerateTokensAsync(
            ApplicationUser user,
            string ipAddress,
            string userAgent);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}