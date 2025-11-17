// AnosheCms/Application/Interfaces/ISubmissionService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface ISubmissionService
    {
        Task<FormSubmitResult> SubmitFormAsync(string formSlug, PublicFormSubmissionRequest request, string ipAddress, string userAgent);

        // (متد قبلی بازنویسی شده تا DTOی صحیح را برگرداند)
        Task<List<FormSubmissionListDto>> GetFormSubmissionsAsync(Guid formId);

        // (متد جدید برای گرید داده‌ها)
        Task<FormSubmissionGridDto> GetSubmissionGridAsync(Guid formId);

        // (متد جدید برای خروجی CSV)
        Task<byte[]> ExportSubmissionsAsCsvAsync(Guid formId);
    }
}