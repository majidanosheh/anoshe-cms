// مسیر: AnosheCms.Application/Interfaces/ISubmissionService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    // --- DTOs ---
    public record FormSubmissionDataDto(string FieldName, string? FieldValue);
    public record FormSubmissionDto(Guid Id, DateTime SubmittedDate, string IpAddress, List<FormSubmissionDataDto> Data);
    public record PublicFormSubmissionRequest(Dictionary<string, string> SubmissionData);
    public record FormSubmitResult(bool Succeeded, string? Message, Dictionary<string, string>? ValidationErrors);

    // --- Interface ---
    public interface ISubmissionService
    {
        Task<FormSubmitResult> SubmitFormAsync(string apiSlug, PublicFormSubmissionRequest request, string ipAddress, string userAgent);
        Task<List<FormSubmissionDto>> GetFormSubmissionsAsync(Guid formId);
    }
}