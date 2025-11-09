// File: AnosheCms.Infrastructure/Services/CurrentUserService.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

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
                var principal = _httpContextAccessor.HttpContext?.User;
                if (principal == null)
                    return null;

                var userIdClaim = principal.FindFirstValue(ClaimTypes.NameIdentifier) ??
                                  principal.FindFirstValue("sub");

                if (userIdClaim == null)
                    return null;

                if (Guid.TryParse(userIdClaim, out Guid userId))
                {
                    return userId;
                }

                return null;
            }
        }

        public string RemoteIpAddress
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            }
        }

        public string UserAgent
        {
            get
            {
                return _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString();
            }
        }
    }
}