// مسیر: AnosheCms.Tests.Unit/Services/FormServiceTests.cs
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using AnosheCms.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AnosheCms.Tests.Unit.Services
{
    public class FormServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly FormService _sut; // System Under Test
        private readonly Guid _formId;
        private const string FormSlug = "contact-us";

        public FormServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options, null);

            // (آماده‌سازی داده‌های تست)
            _formId = Guid.NewGuid();
            var form = new Form
            {
                Id = _formId,
                ApiSlug = FormSlug,
                Name = "Contact Us",
                Fields = new List<FormField>
                {
                    new FormField { Id = Guid.NewGuid(), Name = "full_name", Label = "Full Name", FieldType = "Text", IsRequired = true },
                    new FormField { Id = Guid.NewGuid(), Name = "email", Label = "Email", FieldType = "Email", IsRequired = true },
                    new FormField { Id = Guid.NewGuid(), Name = "message", Label = "Message", FieldType = "Textarea", IsRequired = false }
                }
            };
            _context.Forms.Add(form);
            _context.SaveChanges();

            _sut = new FormService(_context);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldFail_WhenRequiredFieldIsMissing()
        {
            // Arrange
            var submissionData = new Dictionary<string, object>
            {
                { "full_name", "Test User" }
                // (فیلد "email" که اجباری است، ارسال نشده)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.ValidationErrors);
            Assert.True(result.ValidationErrors.ContainsKey("email"));
            Assert.Equal("فیلد 'Email' اجباری است.", result.ValidationErrors["email"]);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldFail_WhenRequiredFieldIsNull()
        {
            // Arrange
            var submissionData = new Dictionary<string, object>
            {
                { "full_name", "Test User" },
                { "email", null } // (ارسال شده اما null)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.ValidationErrors);
            Assert.True(result.ValidationErrors.ContainsKey("email"));
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldSucceed_WhenDataIsValid()
        {
            // Arrange
            var submissionData = new Dictionary<string, object>
            {
                { "full_name", "Test User" },
                { "email", "test@example.com" },
                { "message", "Hello!" }
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.ValidationErrors);

            // (بررسی ذخیره در دیتابیس InMemory)
            var submission = await _context.FormSubmissions.FirstOrDefaultAsync();
            Assert.NotNull(submission);
            Assert.Equal(_formId, submission.FormId);
            Assert.Equal("test@example.com", submission.SubmissionData["email"].ToString());
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldSucceed_WhenOptionalFieldIsMissing()
        {
            // Arrange
            var submissionData = new Dictionary<string, object>
            {
                { "full_name", "Test User" },
                { "email", "test@example.com" }
                // (فیلد "message" که اختیاری است، ارسال نشده)
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.ValidationErrors);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}