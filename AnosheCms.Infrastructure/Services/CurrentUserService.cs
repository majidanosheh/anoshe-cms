// File: AnosheCms.Infrastructure/Services/CurrentUserService.cs

// --- شروع Using Directives ---
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System; // برای Guid.Parse
using System.Security.Claims;
// --- پایان Using Directives ---

namespace AnosheCms.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                // ما Claim "uid" را در زمان ساخت توکن (در AuthService) اضافه کردیم
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
                return claim != null ? Guid.Parse(claim) : null;
            }
        }

        public string? UserEmail
        {
            get
            {
                // این یک Claim استاندارد است
                return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            }
        }
    }
}