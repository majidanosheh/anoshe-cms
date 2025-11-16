// مسیر: Api/Controllers/EmailVerificationController.cs
using AnosheCms.Application.DTOs.Account;
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers;

[ApiController]
[Route("api/account/email-verification")]
[AllowAnonymous] // (این کنترلر باید عمومی باشد)
public class EmailVerificationController : ControllerBase
{
    private readonly IAccountService _accountService;

    public EmailVerificationController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// درخواست ارسال مجدد ایمیل فعال‌سازی
    /// </summary>
    [HttpPost("resend")]
    public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationRequest request)
    {
        var result = await _accountService.RequestEmailVerificationAsync(request.Email, request.VerificationUrlTemplate);

        if (!result.Succeeded && !string.IsNullOrEmpty(result.ErrorMessage))
        {
            return BadRequest(new { Errors = new[] { result.ErrorMessage } });
        }

        // (در هر صورت OK برمی‌گردانیم)
        return Ok(new { Message = "اگر ایمیل شما نیاز به تایید داشته باشد، لینک فعال‌سازی ارسال خواهد شد." });
    }

    /// <summary>
    /// تایید ایمیل با استفاده از توکن
    /// </summary>
    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var result = await _accountService.VerifyEmailAsync(request.Token);

        if (!result.Succeeded)
        {
            return BadRequest(new { Errors = new[] { result.ErrorMessage } });
        }

        return Ok(new { Message = "ایمیل شما با موفقیت تایید شد." });
    }
}