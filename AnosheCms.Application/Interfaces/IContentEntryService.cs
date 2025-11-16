// مسیر: AnosheCms.Application/Interfaces/IContentEntryService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public record ContentEntryCreateDto(
        Dictionary<string, object> ContentData,
        string Status
    );

    // (تعریف DTO به عنوان رکورد 7 آرگومانی)
    public record ContentEntryDto(
        Guid Id,
        Guid ContentTypeId,
        string Status,
        DateTime CreatedDate,
        string? CreatedBy,
        DateTime? LastModifiedDate,
        Dictionary<string, object> ContentData
    );

    public interface IContentEntryService
    {
        // (امضای متد با نام‌های Dto و ErrorMessage)
        Task<List<ContentEntryDto>> GetContentEntriesAsync(string contentTypeSlug);
        Task<ContentEntryDto?> GetContentEntryByIdAsync(string contentTypeSlug, Guid itemId);
        Task<(ContentEntryDto? Dto, string? ErrorMessage)> CreateContentEntryAsync(string contentTypeSlug, ContentEntryCreateDto dto);
        Task<(ContentEntryDto? Dto, string? ErrorMessage)> UpdateContentEntryAsync(string contentTypeSlug, Guid itemId, ContentEntryCreateDto dto);
        Task<bool> DeleteContentEntryAsync(string contentTypeSlug, Guid itemId);
        Task<List<ContentEntryDto>> GetPublishedContentEntriesAsync(string contentTypeSlug);
        Task<ContentEntryDto?> GetPublishedContentEntryAsync(string contentTypeSlug, string itemApiSlug);
    }
}