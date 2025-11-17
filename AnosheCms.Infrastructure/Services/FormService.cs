// AnosheCms/Infrastructure/Services/FormService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class FormService : IFormService
    {
        private readonly ApplicationDbContext _context;

        public FormService(ApplicationDbContext context)
        {
            _context = context;
        }

        // RENAMED and IMPLEMENTED
        public async Task<PublicFormDto> GetFormBySlugAsync(string slug)
        {
            // (باگ IsDeleted: ما باید IgnoreQueryFilters را اعمال کنیم 
            // همانطور که در بسته بازنشانی برای صفحات عمومی ذکر شد)
            var form = await _context.Forms
                .AsNoTracking()
                .IgnoreQueryFilters() // (رفع باگ IsDeleted برای صفحات عمومی)
                .Where(f => f.ApiSlug == slug && f.IsDeleted == false)
                .Select(f => new PublicFormDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    ApiSlug = f.ApiSlug,
                    SubmitButtonText = f.SubmitButtonText,
                    ConfirmationMessage = f.ConfirmationMessage,
                    RedirectUrl = f.RedirectUrl,
                    Fields = f.Fields
                                .Where(field => field.IsDeleted == false) // (فقط فیلدهای حذف نشده)
                                .OrderBy(field => field.Order)
                                .Select(field => new FormFieldDto
                                {
                                    Id = field.Id,
                                    Label = field.Label,
                                    Name = field.Name,
                                    FieldType = field.FieldType,
                                    IsRequired = field.IsRequired,
                                    Order = field.Order,
                                    Placeholder = field.Placeholder,
                                    HelpText = field.HelpText,
                                    Settings = field.Settings,
                                    // (ValidationRules و ConditionalLogic برای استفاده فرانت‌اند ارسال می‌شوند)
                                    ValidationRules = field.ValidationRules,
                                    ConditionalLogic = field.ConditionalLogic
                                })
                                .ToList()
                })
                .FirstOrDefaultAsync();

            return form;
        }


        // --- Helper Mappers ---

        private FormFieldDto MapToFormFieldDto(FormField field)
        {
            return new FormFieldDto
            {
                Id = field.Id,
                Label = field.Label,
                Name = field.Name,
                FieldType = field.FieldType,
                IsRequired = field.IsRequired,
                Order = field.Order,
                Settings = field.Settings,
                Placeholder = field.Placeholder,
                HelpText = field.HelpText,
                ValidationRules = field.ValidationRules,
                ConditionalLogic = field.ConditionalLogic
            };
        }

        private FormDto MapToFormDto(Form form)
        {
            return new FormDto
            {
                Id = form.Id,
                Name = form.Name,
                ApiSlug = form.ApiSlug,
            };
        }

        // --- Other Implemented Methods (Refactored in previous packages) ---

        public async Task<FormDto> UpdateGeneralSettingsAsync(Guid formId, FormGeneralSettingsDto request)
        {
            var form = await _context.Forms.FirstOrDefaultAsync(f => f.Id == formId && !f.IsDeleted);
            if (form == null) return null;

            if (form.ApiSlug != request.ApiSlug)
            {
                bool slugExists = await _context.Forms.AnyAsync(f => f.ApiSlug == request.ApiSlug && f.Id != formId && !f.IsDeleted);
                if (slugExists) return null;
            }

            form.Name = request.Name;
            form.ApiSlug = request.ApiSlug;
            form.SubmitButtonText = request.SubmitButtonText;
            form.ConfirmationMessage = request.ConfirmationMessage;
            form.RedirectUrl = request.RedirectUrl;
            form.SendEmailNotification = request.SendEmailNotification;
            form.NotificationEmailRecipient = request.NotificationEmailRecipient;

            _context.Forms.Update(form);
            await _context.SaveChangesAsync();

            return MapToFormDto(form);
        }

        public async Task<FormFieldDto> UpdateFieldAsync(Guid fieldId, FormFieldUpdateDto request)
        {
            var formField = await _context.FormFields
                .FirstOrDefaultAsync(f => f.Id == fieldId && f.IsDeleted == false);

            if (formField == null) return null;

            bool nameExists = await _context.FormFields
                .AnyAsync(f => f.FormId == formField.FormId && f.Name == request.Name && f.Id != fieldId && !f.IsDeleted);

            if (nameExists) return null;

            formField.Label = request.Label;
            formField.Name = request.Name;
            formField.FieldType = request.FieldType;
            formField.IsRequired = request.IsRequired;
            formField.Order = request.Order;
            formField.Settings = request.Settings;
            formField.Placeholder = request.Placeholder;
            formField.HelpText = request.HelpText;
            formField.ValidationRules = request.ValidationRules;
            formField.ConditionalLogic = request.ConditionalLogic;

            _context.FormFields.Update(formField);
            await _context.SaveChangesAsync();

            return MapToFormFieldDto(formField);
        }

        public async Task<bool> UpdateFieldOrdersAsync(UpdateFieldOrdersRequest request)
        {
            var dbFields = await _context.FormFields
                .Where(f => f.FormId == request.FormId && f.IsDeleted == false)
                .ToListAsync();

            if (!dbFields.Any()) return false;

            var fieldMap = dbFields.ToDictionary(f => f.Id);
            bool changesMade = false;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var fieldDto in request.Fields)
                {
                    if (fieldMap.TryGetValue(fieldDto.FieldId, out var dbField))
                    {
                        if (dbField.Order != fieldDto.Order)
                        {
                            dbField.Order = fieldDto.Order;
                            _context.FormFields.Update(dbField);
                            changesMade = true;
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
                if (changesMade) await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


        #region Unimplemented Methods
        // (متدهای زیر در Golden Code موجودند اما پیاده‌سازی آن‌ها در اولویت بعدی است)
        public Task<FormDto> GetFormByIdAsync(Guid id) { throw new NotImplementedException(); }
        public Task<List<FormDto>> GetAllFormsAsync() { throw new NotImplementedException(); }
        public Task<FormDto> CreateFormAsync(FormCreateDto request) { throw new NotImplementedException(); }
        public Task<bool> DeleteFormAsync(Guid id) { throw new NotImplementedException(); }
        public Task<FormFieldDto> AddFieldToFormAsync(Guid formId, FormFieldCreateDto request) { throw new NotImplementedException(); }
        public Task<bool> DeleteFormFieldAsync(Guid fieldId) { throw new NotImplementedException(); }
        #endregion
    }
}