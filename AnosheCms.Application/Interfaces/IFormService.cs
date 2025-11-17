// AnosheCms/Application/Interfaces/IFormService.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IFormService
    {
        // --- Form CRUD ---
        Task<FormDto> GetFormByIdAsync(Guid id);
        Task<List<FormDto>> GetAllFormsAsync();
        Task<FormDto> CreateFormAsync(FormCreateDto request);
        Task<FormDto> UpdateGeneralSettingsAsync(Guid formId, FormGeneralSettingsDto request);
        Task<bool> DeleteFormAsync(Guid id);

        // --- Fields ---
        Task<FormFieldDto> AddFieldToFormAsync(Guid formId, FormFieldCreateDto request);
        Task<FormFieldDto> UpdateFieldAsync(Guid fieldId, FormFieldUpdateDto request);
        Task<bool> DeleteFormFieldAsync(Guid fieldId);
        Task<bool> UpdateFieldOrdersAsync(UpdateFieldOrdersRequest request);

        // --- Public ---
        Task<PublicFormDto> GetFormBySlugAsync(string slug);
    }
}