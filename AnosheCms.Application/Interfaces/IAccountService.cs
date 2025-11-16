// مسیر: AnosheCms.Application/Interfaces/IAccountService.cs
using AnosheCms.Application.DTOs.Account;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces;

public interface IAccountService
{
    /// <summary>
    /// شروع فرآیند بازیابی رمز عبور. یک توکن ایجاد کرده و ایمیل ارسال می‌کند.
    /// </summary>
    /// <param name="email">ایمیل کاربر</param>
    /// <param name="resetUrlTemplate">الگوی URL که توکن در آن قرار می‌گیرد (مثال: "http://my-app.com/reset?token={0}")</param>
    Task<AccountResult> RequestPasswordResetAsync(string email, string resetUrlTemplate);

    /// <summary>
    /// تنظیم مجدد رمز عبور با استفاده از توکن دریافتی
    /// </summary>
    Task<AccountResult> ResetPasswordAsync(string token, string newPassword);

    /// <summary>
    /// شروع فرآیند تایید ایمیل. یک توکن ایجاد کرده و ایمیل ارسال می‌کند.
    /// </summary>
    /// <param name="email">ایمیل کاربر</param>
    /// <param name="verificationUrlTemplate">الگوی URL که توکن در آن قرار می‌گیرد</param>
    Task<AccountResult> RequestEmailVerificationAsync(string email, string verificationUrlTemplate);

    /// <summary>
    /// ایمیل کاربر را با استفاده از توکن دریافتی تایید می‌کند.
    /// </summary>
    Task<AccountResult> VerifyEmailAsync(string token);
}