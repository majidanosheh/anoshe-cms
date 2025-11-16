//// مسیر: AnosheCms.Tests.Unit/Services/AccountServiceTests.cs

//using AnosheCms.Application.Interfaces;
//using AnosheCms.Domain.Entities;
//using AnosheCms.Infrastructure.Persistence.Data;
//using AnosheCms.Infrastructure.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using Moq;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Timers;
//using Xunit;

//namespace AnosheCms.Tests.Unit.Services;

//// (تست بر اساس قوانین تجربه.txt)
//public class AccountServiceTests : IDisposable
//{
//    private readonly ApplicationDbContext _context;
//    private readonly AccountService _sut; // System Under Test
//    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
//    private readonly Mock<IEmailService> _mockEmailService;
//    private readonly Guid _testUserId = Guid.NewGuid();

//    public AccountServiceTests()
//    {
//        // ۱. راه‌اندازی دیتابیس در حافظه (InMemory)
//        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//            .Options;

//        // (ICurrentUserService برای DbContext مورد نیاز است اما در این تست‌ها استفاده نمی‌شود)
//        _context = new ApplicationDbContext(options, null);

//        // ۲. Mock کردن (شبیه‌سازی) وابستگی‌ها
//        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
//            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

//        _mockEmailService = new Mock<IEmailService>();

//        var logger = Mock.Of<ILogger<AccountService>>();

//        // ۳. ساختن سرویس مورد تست (SUT)
//        _sut = new AccountService(_mockUserManager.Object, _context, _mockEmailService.Object, logger);

//        // ۴. آماده‌سازی داده‌های اولیه
//        var user = new ApplicationUser
//        {
//            Id = _testUserId,
//            Email = "test@example.com",
//            EmailConfirmed = false,
//            FirstName = "Test",
//            LastName = "User"
//        };
//        _context.Users.Add(user);
//        _context.SaveChanges();

//        // (تنظیم رفتار پیش‌فرض Mock UserManager)
//        _mockUserManager.Setup(m => m.FindByEmailAsync("test@example.com"))
//            .ReturnsAsync(user);
//        _mockUserManager.Setup(m => m.FindByIdAsync(_testUserId.ToString()))
//            .ReturnsAsync(user);
//        _mockUserManager.Setup(m => m.ResetPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
//            .ReturnsAsync(IdentityResult.Success);
//    }

//    [Fact]
//    public async Task RequestPasswordResetAsync_ShouldGenerateTokenAndSendEmail()
//    {
//        // Act
//        var result = await _sut.RequestPasswordResetAsync("test@example.com", "http://reset.url/{0}");

//        // Assert
//        Assert.True(result.Succeeded);
//        Assert.Null(result.ErrorMessage);

//        // (بررسی ارسال ایمیل)
//        _mockEmailService.Verify(s => s.SendPasswordResetEmailAsync(
//            "test@example.com",
//            "Test",
//            It.IsAny<string>()), // (چون URL کامل ساخته می‌شود، توکن را چک نمی‌کنیم)
//            Times.Once);

//        // (بررسی ذخیره توکن در دیتابیس)
//        var token = await _context.UserTokens.FirstOrDefaultAsync();
//        Assert.NotNull(token);
//        Assert.Equal(_testUserId, token.UserId);
//        Assert.Equal("PasswordReset", token.LoginProvider);
//        Assert.Equal("ResetToken", token.Name);
//        Assert.True(token.ExpiryDate > DateTime.UtcNow);
//    }

//    [Fact]
//    public async Task ResetPasswordAsync_ShouldFail_WhenTokenIsInvalid()
//    {
//        // Act
//        var result = await _sut.ResetPasswordAsync("invalid-token", "NewPassword123!");

//        // Assert
//        Assert.False(result.Succeeded);
//        Assert.Equal("توکن نامعتبر یا منقضی شده است.", result.ErrorMessage);
//    }

//    [Fact]
//    public async Task ResetPasswordAsync_ShouldSucceed_WhenTokenIsValid()
//    {
//        // Arrange (اول یک توکن معتبر ایجاد می‌کنیم)
//        var tokenResult = await _sut.RequestPasswordResetAsync("test@example.com", "http://reset.url/{0}");
//        var validToken = (await _context.UserTokens.FirstAsync()).Value;

//        // Act
//        var result = await _sut.ResetPasswordAsync(validToken, "NewPassword123!");

//        // Assert
//        Assert.True(result.Succeeded);
//        Assert.Null(result.ErrorMessage);

//        // (بررسی فراخوانی متد اصلی Identity)
//        _mockUserManager.Verify(m => m.ResetPasswordAsync(
//            It.Is<ApplicationUser>(u => u.Id == _testUserId),
//            validToken,
//            "NewPassword123!"),
//            Times.Once);

//        // (بررسی ارسال ایمیل تاییدیه)
//        _mockEmailService.Verify(s => s.SendPasswordChangedEmailAsync(
//            "test@example.com",
//            "Test"),
//            Times.Once);
//    }

//    [Fact]
//    public async Task ResetPasswordAsync_ShouldFail_WhenTokenIsExpired()
//    {
//        // Arrange (ایجاد توکن منقضی شده دستی)
//        var expiredToken = new IdentityUserToken<Guid>
//        {
//            UserId = _testUserId,
//            LoginProvider = "PasswordReset",
//            Name = "ResetToken",
//            Value = "expired-token",
//            ExpiryDate = DateTime.UtcNow.AddMinutes(-10) // (منقضی شده در ۱۰ دقیقه قبل)
//        };
//        _context.UserTokens.Add(expiredToken);
//        await _context.SaveChangesAsync();

//        // Act
//        var result = await _sut.ResetPasswordAsync("expired-token", "NewPassword123!");

//        // Assert
//        Assert.False(result.Succeeded);
//        Assert.Equal("توکن نامعتبر یا منقضی شده است.", result.ErrorMessage);
//    }

//    public void Dispose()
//    {
//        // پاکسازی دیتابیس InMemory پس از هر تست
//        _context.Database.EnsureDeleted();
//        _context.Dispose();
//    }
//}