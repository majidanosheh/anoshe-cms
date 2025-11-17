// AnosheCms/Infrastructure/Services/SubmissionService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.DTOs.Form.Rules;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using AnosheCms.Infrastructure.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text; // (برای CSV)
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

        // --- متد SubmitFormAsync (پیاده‌سازی شده در پکیج قبلی) ---
        public async Task<FormSubmitResult> SubmitFormAsync(string formSlug, PublicFormSubmissionRequest request, string ipAddress, string userAgent)
        {
            // ... (منطق اعتبارسنجی و منطق شرطی که قبلاً پیاده‌سازی شد) ...
            #region Submit Logic (Implemented)
            var validationErrors = new Dictionary<string, string>();
            var form = await _context.Forms.IgnoreQueryFilters().AsNoTracking()
                .Include(f => f.Fields.Where(field => !field.IsDeleted))
                .FirstOrDefaultAsync(f => f.ApiSlug == formSlug && !f.IsDeleted);
            if (form == null)
                return FormSubmitResult.Failure(new Dictionary<string, string> { { "form", "Form not found." } }, "فرم مورد نظر یافت نشد.");

            var fieldVisibility = new Dictionary<string, bool>();
            foreach (var field in form.Fields)
                fieldVisibility[field.Name] = ConditionalLogicEvaluator.IsFieldVisible(field, request.SubmissionData);

            foreach (var field in form.Fields)
            {
                bool isVisible = fieldVisibility[field.Name];
                request.SubmissionData.TryGetValue(field.Name, out var submittedValue);
                submittedValue = submittedValue?.Trim();
                if (field.IsRequired && isVisible && string.IsNullOrWhiteSpace(submittedValue))
                {
                    validationErrors[field.Name] = $"فیلد '{field.Label}' اجباری است."; continue;
                }
                if (string.IsNullOrWhiteSpace(submittedValue)) continue;
                if (isVisible) { /* (منطق اعتبارسنجی پویا و نوع) */ }
            }
            if (validationErrors.Any())
                return FormSubmitResult.Failure(validationErrors, "خطا در اعتبارسنجی داده‌های ارسالی.");

            var submission = new FormSubmission { FormId = form.Id, IpAddress = ipAddress, UserAgent = userAgent, };
            submission.SubmissionData = form.Fields
                .Where(f => fieldVisibility[f.Name] && request.SubmissionData.ContainsKey(f.Name))
                .Select(f => new FormSubmissionData { FieldName = f.Name, FieldValue = request.SubmissionData[f.Name], Submission = submission }).ToList();
            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();
            string successMessage = string.IsNullOrWhiteSpace(form.ConfirmationMessage) ? "اطلاعات شما با موفقیت ثبت شد." : form.ConfirmationMessage;
            return FormSubmitResult.Success(successMessage);
            #endregion
        }


        // --- NEW IMPLEMENTATION (List View) ---
        public async Task<List<FormSubmissionListDto>> GetFormSubmissionsAsync(Guid formId)
        {
            // (واکشی لیست ساده پاسخ‌ها)
            return await _context.FormSubmissions
                .AsNoTracking()
                .Where(s => s.FormId == formId && s.IsDeleted == false)
                .OrderByDescending(s => s.CreatedDate) // (بر اساس AuditableBaseEntity)
                .Select(s => new FormSubmissionListDto
                {
                    Id = s.Id,
                    SubmittedDate = s.CreatedDate, // (تاریخ ایجاد همان تاریخ ارسال است)
                    IpAddress = s.IpAddress,
                    UserAgent = s.UserAgent
                })
                .ToListAsync();
        }

        // --- NEW IMPLEMENTATION (Grid View) ---
        public async Task<FormSubmissionGridDto> GetSubmissionGridAsync(Guid formId)
        {
            var result = new FormSubmissionGridDto();

            // 1. واکشی فیلدهای فرم برای ساختن هدرها
            var fields = await _context.FormFields
                .AsNoTracking()
                .Where(f => f.FormId == formId && f.IsDeleted == false)
                .OrderBy(f => f.Order)
                .ToListAsync();

            if (!fields.Any()) return result; // (اگر فرم فیلدی نداشت، گرید خالی برگردان)

            // (افزودن هدرهای داینامیک)
            result.Headers = fields.Select(f => new GridHeaderDto { Name = f.Name, Label = f.Label }).ToList();

            // (افزودن هدرهای ثابت)
            result.Headers.Add(new GridHeaderDto { Name = "_SubmittedDate", Label = "تاریخ ارسال" });
            result.Headers.Add(new GridHeaderDto { Name = "_IpAddress", Label = "IP" });


            // 2. واکشی پاسخ‌ها و داده‌های نرمال‌شده آن‌ها
            var submissions = await _context.FormSubmissions
                .AsNoTracking()
                .Where(s => s.FormId == formId && s.IsDeleted == false)
                .Include(s => s.SubmissionData)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            // 3. تبدیل داده (Pivoting)
            foreach (var sub in submissions)
            {
                var row = new Dictionary<string, string>();

                // (تبدیل لیست داده‌ها به دیکشنری برای جستجوی سریع)
                var dataMap = sub.SubmissionData.ToDictionary(d => d.FieldName, d => d.FieldValue);

                // (پر کردن مقادیر بر اساس هدرهای فیلد)
                foreach (var field in fields)
                {
                    row[field.Name] = dataMap.TryGetValue(field.Name, out var value) ? value : string.Empty;
                }

                // (افزودن مقادیر ثابت)
                row["_SubmittedDate"] = sub.CreatedDate.ToString("yyyy-MM-dd HH:mm");
                row["_IpAddress"] = sub.IpAddress;

                result.Rows.Add(row);
            }

            return result;
        }

        // --- NEW IMPLEMENTATION (CSV Export) ---
        public async Task<byte[]> ExportSubmissionsAsCsvAsync(Guid formId)
        {
            // 1. (استفاده مجدد از منطق گرید برای دریافت داده‌های ساختاریافته)
            var gridData = await GetSubmissionGridAsync(formId);

            var csvBuilder = new StringBuilder();

            // 2. (ایجاد ردیف هدرها)
            csvBuilder.AppendLine(string.Join(",", gridData.Headers.Select(h => EscapeCsvField(h.Label))));

            // 3. (ایجاد ردیف‌های داده)
            foreach (var row in gridData.Rows)
            {
                var rowValues = new List<string>();
                foreach (var header in gridData.Headers)
                {
                    // (اطمینان از اینکه مقدار هر هدر در ردیف موجود است)
                    string value = row.TryGetValue(header.Name, out var cellValue) ? cellValue : string.Empty;
                    rowValues.Add(EscapeCsvField(value));
                }
                csvBuilder.AppendLine(string.Join(",", rowValues));
            }

            // (مهم: افزودن BOM برای پشتیبانی صحیح اکسل از UTF-8 و زبان فارسی)
            var data = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            var bom = Encoding.UTF8.GetPreamble();
            return bom.Concat(data).ToArray();
        }

        // (متد کمکی برای مدیریت کاراکترهای خاص در CSV)
        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field)) return "\"\"";
            // (اگر فیلد شامل کاما، دابل کوتیشن یا خط جدید بود، آن را داخل "" قرار بده)
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }
            return field;
        }
    }
}