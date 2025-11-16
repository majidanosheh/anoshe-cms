// مسیر: AnosheCms.Infrastructure/Services/AccountService.cs
using AnosheCms.Application.DTOs.Account;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq; // اضافه شده برای رفع خطای احتمالی Select
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountService> _logger;

    private const string PasswordResetProvider = "PasswordReset";
    private const string PasswordResetTokenName = "ResetToken";
    private const string EmailVerificationProvider = "EmailVerification";
    private const string EmailVerificationTokenName = "VerifyToken";

    public AccountService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context,
        IEmailService emailService,
        ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _context = context;
        _emailService = emailService;
        _logger = logger;
    }

    #region Password Reset

    public async Task<AccountResult> RequestPasswordResetAsync(string email, string resetUrlTemplate)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.IsDeleted)
        {
            _logger.LogWarning($"درخواست بازیابی رمز عبور برای ایمیل ناموجود: {email}");
            return AccountResult.Success();
        }

        try
        {
            await InvalidateTokensAsync(user.Id, PasswordResetProvider, PasswordResetTokenName);
            var (tokenValue, expiryDate) = GenerateSecureToken();

            var tokenEntity = new IdentityUserToken<Guid>
            {
                UserId = user.Id,
                LoginProvider = PasswordResetProvider,
                Name = PasswordResetTokenName,
                Value = tokenValue
            };

            _context.UserTokens.Add(tokenEntity);

            // --- نکته مهم: تنظیم ویژگی پنهان ExpiryDate ---
            _context.Entry(tokenEntity).Property<DateTime?>("ExpiryDate").CurrentValue = expiryDate;

            await _context.SaveChangesAsync();

            var resetUrl = string.Format(resetUrlTemplate, tokenValue);
            await _emailService.SendPasswordResetEmailAsync(user.Email, user.FirstName, resetUrl);

            return AccountResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"خطا در هنگام درخواست بازیابی رمز برای {email}");
            return AccountResult.Fail("خطای سرور در هنگام ارسال ایمیل بازیابی.");
        }
    }

    public async Task<AccountResult> ResetPasswordAsync(string token, string newPassword)
    {
        // --- نکته مهم: خواندن ویژگی پنهان ExpiryDate در کوئری ---
        var storedToken = await _context.UserTokens
            .FirstOrDefaultAsync(t =>
                t.Value == token &&
                t.LoginProvider == PasswordResetProvider &&
                t.Name == PasswordResetTokenName &&
                EF.Property<DateTime?>(t, "ExpiryDate") > DateTime.UtcNow);

        if (storedToken == null)
        {
            return AccountResult.Fail("توکن نامعتبر یا منقضی شده است.");
        }

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user == null)
        {
            return AccountResult.Fail("کاربر مرتبط با این توکن یافت نشد.");
        }

        var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetResult = await _userManager.ResetPasswordAsync(user, identityToken, newPassword);

        if (!resetResult.Succeeded)
        {
            return AccountResult.Fail(string.Join(", ", resetResult.Errors.Select(e => e.Description)));
        }

        _context.UserTokens.Remove(storedToken);
        await _context.SaveChangesAsync();

        await _emailService.SendPasswordChangedEmailAsync(user.Email, user.FirstName);

        return AccountResult.Success();
    }

    #endregion

    #region Email Verification

    public async Task<AccountResult> RequestEmailVerificationAsync(string email, string verificationUrlTemplate)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || user.IsDeleted)
        {
            _logger.LogWarning($"درخواست تایید ایمیل برای ایمیل ناموجود: {email}");
            return AccountResult.Success();
        }
        if (user.EmailConfirmed)
        {
            return AccountResult.Fail("این ایمیل قبلاً تأیید شده است.");
        }

        try
        {
            await InvalidateTokensAsync(user.Id, EmailVerificationProvider, EmailVerificationTokenName);
            var (tokenValue, expiryDate) = GenerateSecureToken(hoursValidity: 24);

            var tokenEntity = new IdentityUserToken<Guid>
            {
                UserId = user.Id,
                LoginProvider = EmailVerificationProvider,
                Name = EmailVerificationTokenName,
                Value = tokenValue
            };

            _context.UserTokens.Add(tokenEntity);

            // --- تنظیم ویژگی پنهان ---
            _context.Entry(tokenEntity).Property<DateTime?>("ExpiryDate").CurrentValue = expiryDate;

            await _context.SaveChangesAsync();

            var verificationUrl = string.Format(verificationUrlTemplate, tokenValue);
            await _emailService.SendEmailVerificationEmailAsync(user.Email, user.FirstName, verificationUrl);

            return AccountResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"خطا در هنگام ارسال ایمیل تایید برای {email}");
            return AccountResult.Fail("خطای سرور در هنگام ارسال ایمیل تایید.");
        }
    }

    public async Task<AccountResult> VerifyEmailAsync(string token)
    {
        // --- خواندن ویژگی پنهان ---
        var storedToken = await _context.UserTokens
            .FirstOrDefaultAsync(t =>
                t.Value == token &&
                t.LoginProvider == EmailVerificationProvider &&
                t.Name == EmailVerificationTokenName &&
                EF.Property<DateTime?>(t, "ExpiryDate") > DateTime.UtcNow);

        if (storedToken == null)
        {
            return AccountResult.Fail("توکن فعال‌سازی نامعتبر یا منقضی شده است.");
        }

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());
        if (user == null)
        {
            return AccountResult.Fail("کاربر مرتبط با این توکن یافت نشد.");
        }
        if (user.EmailConfirmed)
        {
            _context.UserTokens.Remove(storedToken);
            await _context.SaveChangesAsync();
            return AccountResult.Fail("این ایمیل قبلاً تأیید شده است.");
        }

        user.EmailConfirmed = true;
        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            return AccountResult.Fail(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
        }

        _context.UserTokens.Remove(storedToken);
        await _context.SaveChangesAsync();

        return AccountResult.Success();
    }

    #endregion

    #region Helpers

    private async Task InvalidateTokensAsync(Guid userId, string loginProvider, string tokenName)
    {
        var oldTokens = await _context.UserTokens
            .Where(t => t.UserId == userId && t.LoginProvider == loginProvider && t.Name == tokenName)
            .ToListAsync();

        if (oldTokens.Any())
        {
            _context.UserTokens.RemoveRange(oldTokens);
        }
    }

    private (string Token, DateTime ExpiryDate) GenerateSecureToken(int hoursValidity = 1)
    {
        var tokenValue = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");
        var expiryDate = DateTime.UtcNow.AddHours(hoursValidity);
        return (tokenValue, expiryDate);
    }

    #endregion
}