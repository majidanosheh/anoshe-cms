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

        Task<List<FormSubmissionDto>> GetFormSubmissionsAsync(Guid formId);
    }

    // (DTOی گمشده که برای Build لازم است)
    public class FormSubmissionDto
    {
        public Guid Id { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string IpAddress { get; set; }
        public Dictionary<string, string> Data { get; set; }
    }
}