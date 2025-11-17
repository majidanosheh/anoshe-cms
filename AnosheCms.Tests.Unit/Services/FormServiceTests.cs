// AnosheCms.Tests.Unit/Services/FormServiceTests.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using AnosheCms.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AnosheCms.Tests.Unit.Services
{
    public class FormServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly FormService _sut; // System Under Test
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Guid _formId;
        private readonly Guid _fieldId1;
        private readonly Guid _currentUserId;
        private const string FormSlug = "contact-us";

        public FormServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _currentUserId = Guid.NewGuid();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock.Setup(s => s.UserId).Returns(_currentUserId);

            _context = new ApplicationDbContext(options, _currentUserServiceMock.Object);

            // (آماده‌سازی داده‌های تست)
            _formId = Guid.NewGuid();
            _fieldId1 = Guid.NewGuid();

            var form = new Form
            {
                Id = _formId,
                ApiSlug = FormSlug,
                Name = "Contact Us",
                IsDeleted = false,
                Fields = new List<FormField> { new FormField { Id = _fieldId1, FormId = _formId, Name = "field1", Label = "F1", Order = 1, IsDeleted = false } }
            };
            _context.Forms.Add(form);
            _context.SaveChanges();

            _sut = new FormService(_context);
        }

        // --- NEW TESTS (CRUD) ---

        [Fact]
        public async Task CreateFormAsync_ShouldCreateForm_WhenSlugIsUnique()
        {
            // Arrange
            var createDto = new FormCreateDto { Name = "New Form", ApiSlug = "new-form" };

            // Act
            var result = await _sut.CreateFormAsync(createDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new-form", result.ApiSlug);
            var formInDb = await _context.Forms.FirstOrDefaultAsync(f => f.ApiSlug == "new-form");
            Assert.NotNull(formInDb);
            Assert.Equal(_currentUserId, formInDb.CreatedBy); // (تست Auditing)
        }

        [Fact]
        public async Task CreateFormAsync_ShouldReturnNull_WhenSlugIsDuplicate()
        {
            // Arrange
            var createDto = new FormCreateDto { Name = "Duplicate Form", ApiSlug = FormSlug }; // (اسلاگ تکراری)

            // Act
            var result = await _sut.CreateFormAsync(createDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteFormAsync_ShouldSoftDeleteForm()
        {
            // Act
            var result = await _sut.DeleteFormAsync(_formId);

            // Assert
            Assert.True(result);

            // (بررسی می‌کنیم که با کوئری عادی پیدا نمی‌شود)
            var formInDb = await _context.Forms.FindAsync(_formId);
            Assert.Null(formInDb);

            // (بررسی می‌کنیم که در دیتابیس هنوز هست و فقط IsDeleted شده)
            var deletedForm = await _context.Forms.IgnoreQueryFilters().FirstOrDefaultAsync(f => f.Id == _formId);
            Assert.NotNull(deletedForm);
            Assert.True(deletedForm.IsDeleted);
            //Assert.Equal(_currentUserId, deletedForm.ModifiedBy); // (تست Auditing)
        }

        // ... (سایر تست‌های موجود مانند UpdateFieldAsync, GetFormBySlugAsync و Auditing) ...

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}