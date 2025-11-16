// مسیر: AnosheCms.Application/Interfaces/IEmailService.cs
namespace AnosheCms.Application.Interfaces;

public interface IEmailService
{
    /// <summary>
    /// ارسال ایمیل عمومی
    /// </summary>
    Task SendEmailAsync(string toEmail, string subject, string htmlBody);

    /// <summary>
    /// ارسال ایمیل حاوی لینک بازیابی رمز عبور
    /// </summary>
    Task SendPasswordResetEmailAsync(string toEmail, string firstName, string resetUrl);

    /// <summary>
    /// ارسال ایمیل اطلاع‌رسانی پس از تغییر موفقیت‌آمیز رمز عبور
    /// </summary>
    Task SendPasswordChangedEmailAsync(string toEmail, string firstName);

    /// <summary>
    /// ارسال ایمیل حاوی لینک فعال‌سازی حساب کاربری
    /// </summary>
    Task SendEmailVerificationEmailAsync(string toEmail, string firstName, string verificationUrl);
}