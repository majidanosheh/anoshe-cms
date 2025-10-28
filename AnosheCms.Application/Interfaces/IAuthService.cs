// File: AnosheCms.Application/Interfaces/IAuthService.cs
using System.Threading.Tasks; // <-- اطمینان از وجود Task

namespace AnosheCms.Application.Interfaces
{
    // DTOs (Data Transfer Objects)
    public record AuthRequest(string Email, string Password);
    public record AuthResponse(bool Succeeded, string Token = "", string ErrorMessage = "");
    public record RegisterRequest(string FirstName, string LastName, string Email, string Password);

    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(AuthRequest request);
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
    }
}