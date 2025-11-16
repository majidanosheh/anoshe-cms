// مسیر: AnosheCms.Application/DTOs/Account/AccountDtos.cs
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Account;

// (یک DTO نتیجه استاندارد برای تمام عملیات‌های اکانت)
public class AccountResult
{
    public bool Succeeded { get; protected set; }
    public string ErrorMessage { get; protected set; }

    public static AccountResult Success() => new() { Succeeded = true };
    public static AccountResult Fail(string error) => new() { Succeeded = false, ErrorMessage = error };
}

// (DTOهای مورد نیاز کنترلرهای جدید)

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string ResetUrlTemplate { get; set; }
}

public class ResetPasswordRequest
{
    [Required]
    public string Token { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}

public class ResendVerificationRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string VerificationUrlTemplate { get; set; }
}

public class VerifyEmailRequest
{
    [Required]
    public string Token { get; set; }
}