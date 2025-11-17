// AnosheCms.Tests.Unit/Services/SubmissionServiceTests.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using AnosheCms.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnosheCms.Tests.Unit.Services
{
    public class SubmissionServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly SubmissionService _sut;
        private readonly Guid _formId;
        private const string FormSlug = "conditional-form";

        public SubmissionServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options, null);

            _formId = Guid.NewGuid();

            // (تعریف منطق شرطی: "Show 'other_reason' if 'category' Equals 'Other'")
            string conditionalLogic = @"{
                ""action"": ""Show"",
                ""condition"": ""All"",
                ""rules"": [{ ""field"": ""category"", ""operator"": ""Equals"", ""value"": ""Other"" }]
            }";

            var form = new Form
            {
                Id = _formId,
                ApiSlug = FormSlug,
                Name = "Conditional Form",
                IsDeleted = false,
                Fields = new List<FormField>
                {
                    // (فیلد انتخاب دسته‌بندی)
                    new FormField { Name = "category", Label = "Category", FieldType = "Dropdown", IsRequired = true, IsDeleted = false },
                    
                    // (فیلد دلیل که اجباری است اما شرطی)
                    new FormField { Name = "other_reason", Label = "Reason", FieldType = "Text",
                                    IsRequired = true, // (اجباری، اما فقط وقتی قابل مشاهده است)
                                    ConditionalLogic = conditionalLogic,
                                    IsDeleted = false },

                    // (فیلد همیشه اجباری)
                    new FormField { Name = "email", Label = "Email", FieldType = "Email", IsRequired = true, IsDeleted = false }
                }
            };
            _context.Forms.Add(form);
            _context.SaveChanges();

            _sut = new SubmissionService(_context);
        }

        // --- تست حفره اعتبارسنجی (Validation Gap) ---
        [Fact]
        public async Task SubmitFormAsync_ShouldSucceed_WhenConditionalRequiredFieldIsHidden()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
            {
                { "category", "Support" }, // (منطق شرطی را فعال نمی‌کند)
                { "email", "test@example.com" }
                // (فیلد 'other_reason' که اجباری بود، ارسال نشده، اما چون مخفی است، نباید خطا دهد)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.True(result.Succeeded, "ارسال باید موفقیت‌آمیز باشد چون فیلد اجباری مخفی بود.");
            Assert.Null(result.ValidationErrors);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldFail_WhenConditionalRequiredFieldIsVisibleAndMissing()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
            {
                { "category", "Other" }, // (منطق شرطی را فعال می‌کند)
                { "email", "test@example.com" }
                // (فیلد 'other_reason' اکنون قابل مشاهده و اجباری است، اما ارسال نشده)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.False(result.Succeeded);
            Assert.True(result.ValidationErrors.ContainsKey("other_reason"));
        }

        // --- تست حفره امنیتی (Security Gap - Data Stuffing) ---
        [Fact]
        public async Task SubmitFormAsync_ShouldNotSaveData_ForHiddenFields()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
            {
                { "category", "Support" }, // (فیلد 'other_reason' مخفی است)
                { "email", "test@example.com" },
                { "other_reason", "HACKER_DATA" } // (تلاش برای ثبت داده در فیلد مخفی)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.True(result.Succeeded); // (ارسال موفق است)

            // (بررسی دیتابیس برای اطمینان از عدم ذخیره داده اضافی)
            var submission = await _context.FormSubmissions
                .Include(s => s.SubmissionData)
                .FirstOrDefaultAsync();

            Assert.NotNull(submission);
            Assert.Equal(2, submission.SubmissionData.Count); // (فقط باید 'category' و 'email' ذخیره شده باشند)
            Assert.Null(submission.SubmissionData.FirstOrDefault(d => d.FieldName == "other_reason"));
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldSaveData_WhenConditionalFieldIsVisible()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
            {
                { "category", "Other" }, // (قابل مشاهده)
                { "email", "test@example.com" },
                { "other_reason", "VALID_DATA" } // (داده معتبر)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.True(result.Succeeded);

            var submission = await _context.FormSubmissions
                .Include(s => s.SubmissionData)
                .FirstOrDefaultAsync();

            Assert.NotNull(submission);
            Assert.Equal(3, submission.SubmissionData.Count); // (همه 3 فیلد باید ذخیره شده باشند)
            Assert.Equal("VALID_DATA", submission.SubmissionData.First(d => d.FieldName == "other_reason").FieldValue);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}