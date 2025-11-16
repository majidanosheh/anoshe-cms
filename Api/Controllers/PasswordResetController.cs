// مسیر: Api/Controllers/PasswordResetController.cs
using AnosheCms.Application.DTOs.Account;
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers;

[ApiController]
[Route("api/account/password-reset")]
[AllowAnonymous] // (این کنترلر باید عمومی باشد)
public class PasswordResetController : ControllerBase
{
    private readonly IAccountService _accountService;

    public PasswordResetController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// درخواست ارسال ایمیل بازیابی رمز عبور
    /// </summary>
    [HttpPost("request")]
    public async Task<IActionResult> RequestPasswordReset([FromBody] ForgotPasswordRequest request)
    {
        var result = await _accountService.RequestPasswordResetAsync(request.Email, request.ResetUrlTemplate);

        // (همیشه OK برمی‌گردانیم تا از افشای اطلاعات ایمیل جلوگیری شود)
        return Ok(new { Message = "اگر ایمیل شما در سیستم موجود باشد، لینک بازیابی ارسال خواهد شد." });
    }

    /// <summary>
    /// تنظیم مجدد رمز عبور با استفاده از توکن
    /// </summary>
    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _accountService.ResetPasswordAsync(request.Token, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new { Errors = new[] { result.ErrorMessage } });
        }

        return Ok(new { Message = "رمز عبور با موفقیت تغییر کرد." });
    }
}