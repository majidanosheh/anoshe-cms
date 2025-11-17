// AnosheCms/Infrastructure/Services/SubmissionService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.DTOs.Form.Rules;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using AnosheCms.Infrastructure.Services.Helpers; // (Import کردن Helper)
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
                return FormSubmitResult.Failure(new Dictionary<string, string> { { "form", "Form not found." } }, "فرم مورد نظر یافت نشد.");
            }

            // --- مرحله ۱: ارزیابی منطق شرطی (Evaluation Pass) ---
            var fieldVisibility = new Dictionary<string, bool>();
            foreach (var field in form.Fields)
            {
                // (استفاده از Helper برای تعیین وضعیت مشاهده)
                fieldVisibility[field.Name] = ConditionalLogicEvaluator.IsFieldVisible(field, request.SubmissionData);
            }

            // --- مرحله ۲: اعتبارسنجی (Validation Pass) ---
            foreach (var field in form.Fields)
            {
                bool isVisible = fieldVisibility[field.Name];

                request.SubmissionData.TryGetValue(field.Name, out var submittedValue);
                submittedValue = submittedValue?.Trim();

                // --- 2.1. بررسی IsRequired (فقط اگر فیلد قابل مشاهده بود) ---
                if (field.IsRequired && isVisible && string.IsNullOrWhiteSpace(submittedValue))
                {
                    validationErrors[field.Name] = $"فیلد '{field.Label}' اجباری است.";
                    continue;
                }

                // (اگر فیلد خالی است، چه مخفی باشد چه اختیاری، ادامه می‌دهیم)
                if (string.IsNullOrWhiteSpace(submittedValue))
                {
                    continue;
                }

                // (اعتبارسنجی‌های دیگر فقط روی فیلدهای قابل مشاهده اجرا می‌شوند)
                if (isVisible)
                {
                    // --- 2.2. بررسی نوع پایه (ایمیل) ---
                    if (field.FieldType.Equals("Email", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!new EmailAddressAttribute().IsValid(submittedValue))
                            validationErrors[field.Name] = $"فیلد '{field.Label}' یک ایمیل معتبر نیست.";
                    }

                    // --- 2.3. پردازش اعتبارسنجی پویا (ValidationRules) ---
                    if (!string.IsNullOrWhiteSpace(field.ValidationRules))
                    {
                        // (منطق اعتبارسنجی پویا که قبلاً نوشتیم)
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
            }

            if (validationErrors.Any())
            {
                return FormSubmitResult.Failure(validationErrors, "خطا در اعتبارسنجی داده‌های ارسالی.");
            }

            // --- مرحله ۳: ذخیره‌سازی (فقط فیلدهای قابل مشاهده) ---
            var submission = new FormSubmission
            {
                FormId = form.Id,
                IpAddress = ipAddress,
                UserAgent = userAgent,
            };

            submission.SubmissionData = form.Fields
                // (فقط فیلدهایی که هم ارسال شده‌اند و هم قابل مشاهده بوده‌اند)
                .Where(f => fieldVisibility[f.Name] && request.SubmissionData.ContainsKey(f.Name))
                .Select(f => new FormSubmissionData
                {
                    FieldName = f.Name,
                    FieldValue = request.SubmissionData[f.Name],
                    Submission = submission
                }).ToList();

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

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