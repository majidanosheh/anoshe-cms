// مسیر: AnosheCms.Infrastructure/Services/LoggingEmailService.cs
using AnosheCms.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services;

/// <summary>
/// (پیاده‌سازی آزمایشی)
/// این سرویس ایمیل‌ها را ارسال نمی‌کند، بلکه آن‌ها را در کنسول لاگ می‌کند.
/// برای استفاده واقعی، باید با یک سرویس واقعی (مانند SendGrid, SMTP) جایگزین شود.
/// </summary>
public class LoggingEmailService : IEmailService
{
    private readonly ILogger<LoggingEmailService> _logger;

    public LoggingEmailService(ILogger<LoggingEmailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        _logger.LogWarning("--- شروع شبیه‌سازی ارسال ایمیل ---");
        _logger.LogInformation($"To: {toEmail}");
        _logger.LogInformation($"Subject: {subject}");
        _logger.LogInformation($"Body (HTML): \n{htmlBody}");
        _logger.LogWarning("--- پایان شبیه‌سازی ارسال ایمیل ---");

        return Task.CompletedTask;
    }

    // (الگوهای HTML ساده برای تست)

    public Task SendPasswordResetEmailAsync(string toEmail, string firstName, string resetUrl)
    {
        var subject = "بازیابی رمز عبور Anoshe CMS";
        var body = new StringBuilder();
        body.AppendLine($"<div dir='rtl' style='font-family:tahoma;'>");
        body.AppendLine($"<h2>سلام {firstName}،</h2>");
        body.AppendLine($"<p>درخواست بازیابی رمز عبور برای حساب شما دریافت شد.</p>");
        body.AppendLine($"<p>برای تنظیم رمز عبور جدید، لطفاً روی لینک زیر کلیک کنید. این لینک تا 1 ساعت آینده معتبر است.</p>");
        body.AppendLine($"<a href='{resetUrl}' style='padding:10px 20px; background-color:#0d6efd; color:white; text-decoration:none; border-radius:5px;'>تنظیم رمز عبور جدید</a>");
        body.AppendLine($"<hr style='margin-top:20px;'>");
        body.AppendLine($"<p style='font-size:0.8em;'>اگر شما این درخواست را نداده‌اید، این ایمیل را نادیده بگیرید.</p>");
        body.AppendLine($"</div>");

        return SendEmailAsync(toEmail, subject, body.ToString());
    }

    public Task SendPasswordChangedEmailAsync(string toEmail, string firstName)
    {
        var subject = "تاییدیه تغییر رمز عبور";
        var body = new StringBuilder();
        body.AppendLine($"<div dir='rtl' style='font-family:tahoma;'>");
        body.AppendLine($"<h2>سلام {firstName}،</h2>");
        body.AppendLine($"<p>رمز عبور حساب کاربری شما با موفقیت تغییر کرد.</p>");
        body.AppendLine($"<p style='font-size:0.8em;'>اگر شما این تغییر را انجام نداده‌اید، لطفاً فوراً با پشتیبانی تماس بگیرید.</p>");
        body.AppendLine($"</div>");

        return SendEmailAsync(toEmail, subject, body.ToString());
    }

    public Task SendEmailVerificationEmailAsync(string toEmail, string firstName, string verificationUrl)
    {
        var subject = "فعال‌سازی حساب کاربری Anoshe CMS";
        var body = new StringBuilder();
        body.AppendLine($"<div dir='rtl' style='font-family:tahoma;'>");
        body.AppendLine($"<h2>سلام {firstName}، به Anoshe CMS خوش آمدید!</h2>");
        body.AppendLine($"<p>فقط یک قدم دیگر باقی مانده است. لطفاً برای فعال‌سازی حساب خود روی لینک زیر کلیک کنید.</p>");
        body.AppendLine($"<a href='{verificationUrl}' style='padding:10px 20px; background-color:#0d6efd; color:white; text-decoration:none; border-radius:5px;'>فعال‌سازی حساب</a>");
        body.AppendLine($"</div>");

        return SendEmailAsync(toEmail, subject, body.ToString());
    }
}