// File: AnosheCms.Application/Interfaces/IAuthService.cs
using AnosheCms.Application.DTOs.Auth;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginAsync(LoginRequest request, string ipAddress, string userAgent);
        Task<AuthenticationResult> RefreshTokenAsync(TokenRequest request, string ipAddress, string userAgent);
        Task<bool> RevokeTokenAsync(string token, string ipAddress, string userAgent);

        // (ما RegisterAsync را بعداً اضافه خواهیم کرد، فعلاً روی لاگین تمرکز می‌کنیم)
        // Task<AuthenticationResult> RegisterAsync(RegisterRequest request, string origin);
    }
}