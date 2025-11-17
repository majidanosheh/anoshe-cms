// مسیر: AnosheCms.Infrastructure/Services/SubmissionService.cs
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<FormSubmissionDto>> GetFormSubmissionsAsync(Guid formId)
        {
            var submissions = await _context.FormSubmissions
                .AsNoTracking()
                .Where(s => s.FormId == formId)
                .Include(s => s.SubmissionData)
                .OrderByDescending(s => s.CreatedDate)
                .ToListAsync();

            return submissions.Select(s => new FormSubmissionDto(
                s.Id,
                s.CreatedDate,
                s.IpAddress,
                s.SubmissionData.Select(sd => new FormSubmissionDataDto(sd.FieldName, sd.FieldValue)).ToList()
            )).ToList();
        }

        public async Task<FormSubmitResult> SubmitFormAsync(string apiSlug, PublicFormSubmissionRequest request, string ipAddress, string userAgent)
        {
            var form = await _context.Forms
                .AsNoTracking()
                .Include(f => f.Fields)
                .FirstOrDefaultAsync(f => f.ApiSlug == apiSlug);

            if (form == null || form.IsDeleted)
            {
                return new FormSubmitResult(false, "Form not found.", null);
            }

            var (isValid, validationErrors) = ValidateSubmission(form.Fields, request.SubmissionData);
            if (!isValid)
            {
                return new FormSubmitResult(false, "Validation failed.", validationErrors);
            }

            var submission = new FormSubmission
            {
                FormId = form.Id,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsDeleted = false,
                SubmissionData = request.SubmissionData.Select(kvp => new FormSubmissionData
                {
                    FieldName = kvp.Key,
                    FieldValue = kvp.Value
                }).ToList()
            };

            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();

            return new FormSubmitResult(true, "Submission successful.", null);
        }

        private (bool IsValid, Dictionary<string, string> Errors) ValidateSubmission(
            IEnumerable<FormField> fields,
            Dictionary<string, string> data)
        {
            var errors = new Dictionary<string, string>();

            foreach (var field in fields)
            {
                data.TryGetValue(field.Name, out var value);

                if (field.IsRequired && string.IsNullOrWhiteSpace(value))
                {
                    errors[field.Name] = $"فیلد '{field.Label}' اجباری است.";
                    continue;
                }

                if (string.IsNullOrWhiteSpace(value))
                    continue;

                if (field.FieldType == "Email" && !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errors[field.Name] = $"فیلد '{field.Label}' یک ایمیل معتبر نیست.";
                }

                if (field.FieldType == "Number" && !decimal.TryParse(value, out _))
                {
                    errors[field.Name] = $"فیلد '{field.Label}' باید یک عدد باشد.";
                }
            }

            return (errors.Count == 0, errors.Count == 0 ? null : errors);
        }
    }
}