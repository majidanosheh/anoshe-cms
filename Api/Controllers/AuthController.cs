using AnosheCms.Application.DTOs.Auth;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic; 

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager; 

        public AuthController(
            IAuthService authService,
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager) 
        {
            _authService = authService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

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

       
        [HttpGet("rescue")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateRescueAdmin()
        {
            var email = "rescue@system.com";
            var password = "Rescue@123";
            var roleName = "SuperAdmin";

            // 1. اطمینان از وجود نقش
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = roleName, DisplayName = "سوپر ادمین", IsSystemRole = true });
            }

            // 2. پیدا کردن یا ساختن کاربر
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "Rescue",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };
                var createResult = await _userManager.CreateAsync(user, password);
                if (!createResult.Succeeded) return BadRequest(createResult.Errors);
            }
            else
            {
                // اگر بود، فعالش کن و رمزش را ریست کن
                user.IsDeleted = false; // احیا از حذف نرم
                user.IsActive = true;
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, password);
                await _userManager.UpdateAsync(user);
            }

            // 3. تخصیص نقش (مهم‌ترین بخش)
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                var roleResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded) return BadRequest(new { Message = "خطا در انتساب نقش", Errors = roleResult.Errors });
            }

            // 4. بررسی نهایی
            var currentRoles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                Message = "کاربر نجات با موفقیت تنظیم شد.",
                Email = email,
                Password = password,
                RolesInDb = currentRoles // لیست نقش‌هایی که الان در دیتابیس دارد
            });
        }
    }
}