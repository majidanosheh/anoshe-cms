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

        public async Task<FormDto> GetFormByIdAsync(Guid id)
        {
            // دریافت انتیتی از دیتابیس به همراه فیلدها
            var form = await _context.Forms
                .AsNoTracking()
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null) return null;

            // 2. تبدیل دستی به DTO (چون AutoMapper شاید کانفیگ نباشد)
            return new FormDto
            {
                Id = form.Id,
                Name = form.Name,
                ApiSlug = form.ApiSlug,
                // لیست فیلدها را هم پر می‌کنیم
                Fields = form.Fields
                            .Where(f => !f.IsDeleted) // فقط فیلدهای حذف نشده
                            .OrderBy(f => f.Order)    // مرتب‌سازی
                            .Select(f => MapToFormFieldDto(f)) // استفاده از هلپر موجود در پایین همین فایل
                            .ToList()
            };
        }

        public async Task<List<FormDto>> GetAllFormsAsync()
        {
            return await _context.Forms
                .AsNoTracking()
                .OrderByDescending(f => f.CreatedDate)
                .Select(f => new FormDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    ApiSlug = f.ApiSlug
                })
                .ToListAsync();
        }

        public async Task<FormDto> CreateFormAsync(FormCreateDto request)
        {
            // (بررسی عدم تکرار ApiSlug)
            bool slugExists = await _context.Forms
                .AnyAsync(f => f.ApiSlug == request.ApiSlug);

            if (slugExists)
            {
                return null;
            }

            var form = new Form
            {
                Name = request.Name,
                ApiSlug = request.ApiSlug,
                
            };

            _context.Forms.Add(form);
            await _context.SaveChangesAsync();

            return MapToFormDto(form);
        }

        public async Task<FormDto> UpdateGeneralSettingsAsync(Guid formId, FormGeneralSettingsDto request)
        {
            var form = await _context.Forms.FindAsync(formId);
            if (form == null) return null;

            if (form.ApiSlug != request.ApiSlug)
            {
                bool slugExists = await _context.Forms.AnyAsync(f => f.ApiSlug == request.ApiSlug && f.Id != formId);
                if (slugExists) return null; // (خطای تداخل اسلاگ)
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

        public async Task<bool> DeleteFormAsync(Guid id)
        {
            var form = await _context.Forms.FindAsync(id);
            if (form == null) return false;

            _context.Forms.Remove(form); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<FormFieldDto> AddFieldToFormAsync(Guid formId, FormFieldCreateDto request)
        {
            var form = await _context.Forms.FindAsync(formId);
            if (form == null) return null;

            //بررسی عدم تکرار این فیلد جاری
            bool nameExists = await _context.FormFields
                .AnyAsync(f => f.FormId == formId && f.Name == request.Name);

            if (nameExists) return null;

            var formField = new FormField
            {
                FormId = formId,
                Label = request.Label,
                Name = request.Name,
                FieldType = request.FieldType,
                IsRequired = request.IsRequired,
                Order = request.Order,
                Placeholder = request.Placeholder,
                HelpText = request.HelpText,
                Settings = request.Settings
            };

            _context.FormFields.Add(formField);
            await _context.SaveChangesAsync();

            return MapToFormFieldDto(formField);
        }

        public async Task<FormFieldDto> UpdateFieldAsync(Guid fieldId, FormFieldUpdateDto request)
        {
            var formField = await _context.FormFields.FindAsync(fieldId);
            if (formField == null) return null;

            bool nameExists = await _context.FormFields
                .AnyAsync(f => f.FormId == formField.FormId && f.Name == request.Name && f.Id != fieldId);
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

        public async Task<bool> DeleteFormFieldAsync(Guid fieldId)
        {
            var field = await _context.FormFields.FindAsync(fieldId);
            if (field == null) return false;

            _context.FormFields.Remove(field); 
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateFieldOrdersAsync(UpdateFieldOrdersRequest request)
        {
            var dbFields = await _context.FormFields
                .Where(f => f.FormId == request.FormId && f.IsDeleted == false)
                .ToListAsync();
            if (!dbFields.Any()) return false;
            var fieldMap = dbFields.ToDictionary(f => f.Id);
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
                        }
                    }
                    else { await transaction.RollbackAsync(); return false; }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception) { await transaction.RollbackAsync(); return false; }
        }

        public async Task<PublicFormDto> GetFormBySlugAsync(string slug)
        {
            // 1. دریافت اطلاعات خام از دیتابیس (بدون مپ کردن)
            var form = await _context.Forms
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(f => f.Fields) // بارگذاری فیلدها
                .Where(f => f.ApiSlug == slug && f.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (form == null) return null;

            // 2. تبدیل به DTO در حافظه (اینجا متد MapToFormFieldDto کار می‌کند)
            return new PublicFormDto
            {
                Id = form.Id,
                Name = form.Name,
                ApiSlug = form.ApiSlug,
                SubmitButtonText = form.SubmitButtonText,
                ConfirmationMessage = form.ConfirmationMessage,
                RedirectUrl = form.RedirectUrl,

                // فیلتر کردن و مرتب‌سازی فیلدها در حافظه
                Fields = form.Fields
                            .Where(field => field.IsDeleted == false)
                            .OrderBy(field => field.Order)
                            .Select(field => MapToFormFieldDto(field)) // ✅ اینجا مجاز است
                            .ToList()
            };
        }

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
            return new FormDto { Id = form.Id, Name = form.Name, ApiSlug = form.ApiSlug };
        }
    }
}