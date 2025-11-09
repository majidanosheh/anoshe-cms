// File: Api/Controllers/AuthController.cs
using AnosheCms.Application.DTOs.Auth;
using AnosheCms.Application.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService; // (برای IP و UserAgent)

        public AuthController(IAuthService authService, ICurrentUserService currentUserService)
        {
            _authService = authService;
            _currentUserService = currentUserService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(
                request,
                _currentUserService.RemoteIpAddress,
                _currentUserService.UserAgent
            );

            if (!result.Succeeded)
            {
                return Unauthorized(new { Errors = result.Errors });
            }

            return Ok(result);
        }

        // POST: api/auth/refresh
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(
                request,
                _currentUserService.RemoteIpAddress,
                _currentUserService.UserAgent
            );

            if (!result.Succeeded)
            {
                return Unauthorized(new { Errors = result.Errors
    });
            }

return Ok(result);
        }

        // (توجه: متد Register حذف شد، زیرا IAuthService جدید فعلاً آن را ندارد)
    }
}