// AnosheCms.Tests.Unit/Services/FormServiceTests.cs
// FULL REWRITE (ادغام تست‌های قبلی Auditing با تست جدید GetFormBySlug)

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
        private readonly Guid _fieldId1, _fieldId2, _fieldDeleted;
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
            _fieldId2 = Guid.NewGuid();
            _fieldDeleted = Guid.NewGuid();

            var form = new Form
            {
                Id = _formId,
                ApiSlug = FormSlug,
                Name = "Contact Us",
                IsDeleted = false, // (برای تست باگ IsDeleted)
                Fields = new List<FormField>
                {
                    new FormField { Id = _fieldId1, FormId = _formId, Name = "field1", Label = "F1", Order = 1, IsDeleted = false, Placeholder = "P1" },
                    new FormField { Id = _fieldId2, FormId = _formId, Name = "field2", Label = "F2", Order = 2, IsDeleted = false },
                    new FormField { Id = _fieldDeleted, FormId = _formId, Name = "field_deleted", Label = "F_Del", Order = 3, IsDeleted = true }
                }
            };

            var deletedForm = new Form
            {
                Id = Guid.NewGuid(),
                ApiSlug = "deleted-form",
                Name = "Deleted Form",
                IsDeleted = true // (این فرم نباید واکشی شود)
            };

            _context.Forms.Add(form);
            _context.Forms.Add(deletedForm);
            _context.SaveChanges();

            _sut = new FormService(_context);
        }

        // --- NEW TEST (GetFormBySlugAsync) ---

        [Fact]
        public async Task GetFormBySlugAsync_ShouldReturnForm_WhenSlugExistsAndNotDeleted()
        {
            // Act
            var result = await _sut.GetFormBySlugAsync(FormSlug);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(_formId, result.Id);
            Assert.Equal("P1", result.Fields[0].Placeholder);
        }

        [Fact]
        public async Task GetFormBySlugAsync_ShouldReturnNull_WhenFormIsDeleted()
        {
            // Act
            // (این تست به لطف .IgnoreQueryFilters() در سرویس، فرم حذف شده را پیدا *نمی‌کند*)
            var result = await _sut.GetFormBySlugAsync("deleted-form");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFormBySlugAsync_ShouldNotIncludeDeletedFields()
        {
            // Act
            var result = await _sut.GetFormBySlugAsync(FormSlug);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Fields.Count); // (فیلد _fieldDeleted نباید شمارش شود)
            Assert.DoesNotContain(result.Fields, f => f.Id == _fieldDeleted);
        }

        // --- (تست‌های موجود از پکیج‌های قبلی) ---

        [Fact]
        public async Task UpdateFieldAsync_ShouldUpdateAllFields_WhenDataIsValid()
        {
            // Arrange
            var updateDto = new FormFieldUpdateDto
            {
                Label = "Updated Label",
                Name = "updated_name",
                FieldType = "Textarea",
                IsRequired = false,
                Order = 10,
                Placeholder = "New Placeholder",
                HelpText = "New Help Text",
                ValidationRules = "{\"rule\":\"new\"}",
                ConditionalLogic = "{\"logic\":\"new\"}"
            };

            // Act
            await _sut.UpdateFieldAsync(_fieldId1, updateDto);

            // Assert
            var fieldInDb = await _context.FormFields.FindAsync(_fieldId1);
            Assert.Equal("New Placeholder", fieldInDb.Placeholder);
            Assert.Equal("New Help Text", fieldInDb.HelpText);
            Assert.Equal("{\"rule\":\"new\"}", fieldInDb.ValidationRules);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldSetCreatedByAndDate_OnAdd()
        {
            // Arrange
            var newField = new FormField { FormId = _formId, Name = "new_field", Label = "New", FieldType = "Text", Order = 4 };

            // Act
            _context.FormFields.Add(newField);
            await _context.SaveChangesAsync();

            // Assert
            Assert.Equal(_currentUserId, newField.CreatedBy);
            Assert.True(newField.CreatedDate > DateTime.UtcNow.AddMinutes(-1));
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}