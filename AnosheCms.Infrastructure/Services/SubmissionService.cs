// AnosheCms/Infrastructure/Services/SubmissionService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.DTOs.Form.Rules;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ApplicationDbContext _context;

        public SubmissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FormSubmitResult> SubmitFormAsync(string formSlug, PublicFormSubmissionRequest request, string ipAddress, string userAgent)
        {
            var validationErrors = new Dictionary<string, string>();

            var form = await _context.Forms
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(f => f.Fields.Where(field => !field.IsDeleted))
                .FirstOrDefaultAsync(f => f.ApiSlug == formSlug && !f.IsDeleted);

            if (form == null)
            {
                var errors = new Dictionary<string, string> { { "form", "Form not found." } };
                // (اصلاح شد: ارسال پیام در Failure)
                return FormSubmitResult.Failure(errors, "فرم مورد نظر یافت نشد.");
            }

            // (پردازش اعتبارسنجی - بدون تغییر)
            foreach (var field in form.Fields)
            {
                request.SubmissionData.TryGetValue(field.Name, out var submittedValue);
                submittedValue = submittedValue?.Trim();

                if (field.IsRequired && string.IsNullOrWhiteSpace(submittedValue))
                {
                    validationErrors[field.Name] = $"فیلد '{field.Label}' اجباری است.";
                    continue;
                }
                if (string.IsNullOrWhiteSpace(submittedValue)) continue;

                if (field.FieldType.Equals("Email", StringComparison.OrdinalIgnoreCase))
                {
                    if (!new EmailAddressAttribute().IsValid(submittedValue))
                        validationErrors[field.Name] = $"فیلد '{field.Label}' یک ایمیل معتبر نیست.";
                }

                if (!string.IsNullOrWhiteSpace(field.ValidationRules))
                {
                    try
                    {
                        var rules = JsonSerializer.Deserialize<FieldValidationRules>(field.ValidationRules, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        if (rules != null)
                        {
                            if (rules.MinLength.HasValue && submittedValue.Length < rules.MinLength.Value)
                                validationErrors[field.Name] = $"فیلد '{field.Label}' باید حداقل {rules.MinLength.Value} کاراکتر باشد.";
                            if (rules.MaxLength.HasValue && submittedValue.Length > rules.MaxLength.Value)
                                validationErrors[field.Name] = $"فیلد '{field.Label}' نمی‌تواند بیش از {rules.MaxLength.Value} کاراکتر باشد.";
                            if (!string.IsNullOrWhiteSpace(rules.RegexPattern) && !Regex.IsMatch(submittedValue, rules.RegexPattern))
                                validationErrors[field.Name] = string.IsNullOrWhiteSpace(rules.RegexErrorMessage) ? $"فیلد '{field.Label}' فرمت نامعتبر دارد." : rules.RegexErrorMessage;
                        }
                    }
                    catch (JsonException) { /* لاگ خطا */ }
                }
            }

            if (validationErrors.Any())
            {
                // (اصلاح شد: ارسال پیام در Failure)
                return FormSubmitResult.Failure(validationErrors, "خطا در اعتبارسنجی داده‌های ارسالی.");
            }

            // (پردازش ذخیره‌سازی - بدون تغییر)
            var submission = new FormSubmission
            {
                FormId = form.Id,
                IpAddress = ipAddress,
                UserAgent = userAgent,
            };

            submission.SubmissionData = form.Fields
                .Where(f => request.SubmissionData.ContainsKey(f.Name))
                .Select(f => new FormSubmissionData
                {
                    FieldName = f.Name,
                    FieldValue = request.SubmissionData[f.Name],
                    Submission = submission
                }).ToList();

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            // (اصلاح شد: ارسال پیام موفقیت‌آمیز بر اساس تنظیمات فرم)
            string successMessage = string.IsNullOrWhiteSpace(form.ConfirmationMessage)
                ? "اطلاعات شما با موفقیت ثبت شد."
                : form.ConfirmationMessage;

            return FormSubmitResult.Success(successMessage);
        }


        public Task<List<FormSubmissionDto>> GetFormSubmissionsAsync(Guid formId)
        {
            throw new NotImplementedException();
        }
    }
}