// مسیر: AnosheCms.Tests.Unit/Services/SubmissionServiceTests.cs
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
    public class SubmissionServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly SubmissionService _sut; // System Under Test
        private readonly Guid _formId;
        private const string FormSlug = "contact-us";

        public SubmissionServiceTests()
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
                IsDeleted = false, // (تست باگ IsDeleted)
                Fields = new List<FormField>
                {
                    new FormField { Name = "full_name", Label = "Full Name", FieldType = "Text", IsRequired = true, IsDeleted = false },
                    new FormField { Name = "email", Label = "Email", FieldType = "Email", IsRequired = true, IsDeleted = false },
                    new FormField { Name = "message", Label = "Message", FieldType = "Textarea", IsRequired = false, IsDeleted = false }
                }
            };
            _context.Forms.Add(form);
            _context.SaveChanges();

            _sut = new SubmissionService(_context);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldFail_WhenRequiredFieldIsMissing()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
            {
                { "full_name", "Test User" }
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
        public async Task SubmitFormAsync_ShouldFail_WhenFieldIsInvalidEmail()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
            {
                { "full_name", "Test User" },
                { "email", "not-an-email" }
            };
            var request = new PublicFormSubmissionRequest(submissionData);

            // Act
            var result = await _sut.SubmitFormAsync(FormSlug, request, "127.0.0.1", "TestAgent");

            // Assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.ValidationErrors);
            Assert.True(result.ValidationErrors.ContainsKey("email"));
            Assert.Equal("فیلد 'Email' یک ایمیل معتبر نیست.", result.ValidationErrors["email"]);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldSucceed_WhenDataIsValid()
        {
            // Arrange
            var submissionData = new Dictionary<string, string>
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

            var submission = await _context.FormSubmissions
                .Include(s => s.SubmissionData)
                .FirstOrDefaultAsync();

            Assert.NotNull(submission);
            Assert.Equal(_formId, submission.FormId);
            Assert.Equal("test@example.com", submission.SubmissionData.First(d => d.FieldName == "email").FieldValue);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}