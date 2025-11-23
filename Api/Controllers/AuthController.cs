// File: AnosheCms/Api/Controllers/AuthController.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Auth;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities; // اضافه شد
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity; // اضافه شد
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager; // اضافه شد برای تعمیر

        public AuthController(
            IAuthService authService,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager) // تزریق شد
        {
            _authService = authService;
            _currentUserService = currentUserService;
            _userManager = userManager;
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
                return Unauthorized(new { Errors = result.Errors });
            }

            return Ok(result);
        }

        // --- متد اضطراری برای تعمیر ادمین ---
        // GET: api/auth/fix-admin
        [HttpGet("fix-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> FixAdmin()
        {
            var email = "admin@system.com";
            var password = "Admin@123";

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                // اگر نیست، بساز
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "System",
                    LastName = "Admin",
                    IsActive = true,
                    IsDeleted = false,
                    EmailConfirmed = true
                };
                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded) return BadRequest(createResult.Errors);
            }
            else
            {
                // اگر هست، رمز را ریست کن
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, password);
                if (!resetResult.Succeeded) return BadRequest(resetResult.Errors);

                // مطمئن شو فعال است
                user.IsActive = true;
                user.IsDeleted = false;
                await _userManager.UpdateAsync(user);
            }

            return Ok($"User {email} fixed with password: {password}");
        }
    }
}