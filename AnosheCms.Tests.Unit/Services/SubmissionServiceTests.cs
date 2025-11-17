//// AnosheCms.Tests.Unit/Services/SubmissionServiceTests.cs
//// FULL REWRITE

//using AnosheCms.Application.DTOs.Form; // (DTOs باید import شوند)
//using AnosheCms.Application.Interfaces;
//using AnosheCms.Domain.Entities;
//using AnosheCms.Infrastructure.Persistence.Data;
//using AnosheCms.Infrastructure.Services;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace AnosheCms.Tests.Unit.Services
//{
//    public class SubmissionServiceTests : IDisposable
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly SubmissionService _sut; // System Under Test
//        private readonly Guid _formId;
//        private const string FormSlug = "contact-us";

//        public SubmissionServiceTests()
//        {
//            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
//                .Options;

//            // (ما در اینجا ICurrentUserService را null رد می‌کنیم
//            // چون SubmissionService مستقیماً به آن نیاز ندارد،
//            // اما DbContext برای Auditing به آن نیاز دارد.
//            // در تست‌های Auditing باید Mock شود)
//            _context = new ApplicationDbContext(options, null);

//            _formId = Guid.NewGuid();

//            // (تعریف قوانین اعتبارسنجی پویا برای تست)
//            string phoneValidationRules = @"{
//                ""regexPattern"": ""^09[0-9]{9}$"",
//                ""regexErrorMessage"": ""شماره موبایل باید 11 رقم و با 09 شروع شود.""
//            }";
//            string messageValidationRules = @"{
//                ""minLength"": 10,
//                ""maxLength"": 500
//            }";

//            var form = new Form
//            {
//                Id = _formId,
//                ApiSlug = FormSlug,
//                Name = "Contact Us",
//                IsDeleted = false,
//                Fields = new List<FormField>
//                {
//                    new FormField { Name = "full_name", Label = "Full Name", FieldType = "Text", IsRequired = true, IsDeleted = false },
//                    new FormField { Name = "email", Label = "Email", FieldType = "Email", IsRequired = true, IsDeleted = false },
//                    new FormField { Name = "phone", Label = "Phone", FieldType = "Text", IsRequired = false, ValidationRules = phoneValidationRules, IsDeleted = false },
//                    new FormField { Name = "message", Label = "Message", FieldType = "Textarea", IsRequired = true, ValidationRules = messageValidationRules, IsDeleted = false }
//                }
//            };
//            _context.Forms.Add(form);
//            _context.SaveChanges();

//            _sut = new SubmissionService(_context);
//        }

//        [Fact]
//        public async Task SubmitFormAsync_ShouldFail_WhenRequiredFieldIsMissing()
//        {
//            // Arrange
//            var submissionData = new Dictionary<string, string>
//            {
//                { "full_name", "Test User" },
//                { "email", "test@example.com" }
//                // (فیلد "message" که اجباری است، ارسال نشده)
//            };
//            var request = new PublicFormSubmissionRequest(submissionData);

//            // Act
//            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

//            // Assert
//            Assert.False(result.Succeeded);
//            Assert.True(result.ValidationErrors.ContainsKey("message"));
//        }

//        // --- NEW TESTS (Dynamic Validation) ---

//        [Fact]
//        public async Task SubmitFormAsync_ShouldFail_WhenRegexIsInvalid()
//        {
//            // Arrange
//            var submissionData = new Dictionary<string, string>
//            {
//                { "full_name", "Test User" },
//                { "email", "test@example.com" },
//                { "phone", "12345" }, // (Regex نامعتبر)
//                { "message", "This is a valid message content." }
//            };
//            var request = new PublicFormSubmissionRequest(submissionData);

//            // Act
//            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

//            // Assert
//            Assert.False(result.Succeeded);
//            Assert.True(result.ValidationErrors.ContainsKey("phone"));
//            Assert.Equal("شماره موبایل باید 11 رقم و با 09 شروع شود.", result.ValidationErrors["phone"]);
//        }

//        [Fact]
//        public async Task SubmitFormAsync_ShouldFail_WhenMinLengthIsInvalid()
//        {
//            // Arrange
//            var submissionData = new Dictionary<string, string>
//            {
//                { "full_name", "Test User" },
//                { "email", "test@example.com" },
//                { "message", "Too short" } // (MinLength 10 است)
//            };
//            var request = new PublicFormSubmissionRequest(submissionData);

//            // Act
//            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

//            // Assert
//            Assert.False(result.Succeeded);
//            Assert.True(result.ValidationErrors.ContainsKey("message"));
//            Assert.Contains("حداقل 10 کاراکتر", result.ValidationErrors["message"]);
//        }

//        [Fact]
//        public async Task SubmitFormAsync_ShouldSucceed_WhenDataIsValidAndRegexIsValid()
//        {
//            // Arrange
//            var submissionData = new Dictionary<string, string>
//            {
//                { "full_name", "Test User" },
//                { "email", "test@example.com" },
//                { "phone", "09123456789" }, // (Regex معتبر)
//                { "message", "This is a perfectly valid message with enough length." }
//            };
//            var request = new PublicFormSubmissionRequest(submissionData);

//            // Act
//            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

//            // Assert
//            Assert.True(result.Succeeded);
//            Assert.Null(result.ValidationErrors);

//            var submission = await _context.FormSubmissions
//                .Include(s => s.SubmissionData)
//                .FirstOrDefaultAsync();

//            Assert.NotNull(submission);
//            Assert.Equal("09123456789", submission.SubmissionData.First(d => d.FieldName == "phone").FieldValue);
//        }

//        [Fact]
//        public async Task SubmitFormAsync_ShouldSucceed_WhenOptionalRegexFieldIsEmpty()
//        {
//            // Arrange (فیلد "phone" اختیاری است)
//            var submissionData = new Dictionary<string, string>
//            {
//                { "full_name", "Test User" },
//                { "email", "test@example.com" },
//                { "phone", "" }, // (خالی ارسال شده)
//                { "message", "This is a perfectly valid message with enough length." }
//            };
//            var request = new PublicFormSubmissionRequest(submissionData);

//            // Act
//            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

//            // Assert
//            Assert.True(result.Succeeded);
//            Assert.Null(result.ValidationErrors);
//        }

//        public void Dispose()
//        {
//            _context.Database.EnsureDeleted();
//            _context.Dispose();
//        }
//    }
//}