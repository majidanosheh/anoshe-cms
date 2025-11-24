using AnosheCms.Application.DTOs.Common;
using AnosheCms.Application.DTOs.ContentEntry;


namespace AnosheCms.Application.Interfaces
{
    public interface IContentEntryService
    {
        Task<PagedResult<ContentEntryDto>> GetEntriesAsync(string typeSlug, int page, int pageSize);
        Task<ContentEntryDto> GetEntryByIdAsync(Guid id);
        Task<Guid> CreateEntryAsync(string typeSlug, ContentEntryCreateDto request);
        Task UpdateEntryAsync(Guid id, ContentEntryCreateDto request);
        Task DeleteEntryAsync(Guid id);

        
        Task<List<ContentEntryDto>> GetPublishedContentEntriesAsync(string apiSlug);

        // گرفتن یک آیتم منتشر شده با شناسه (ID یا Slug)
        Task<ContentEntryDto?> GetPublishedContentEntryAsync(string apiSlug, string itemIdentifier);
    }
}