using AnosheCms.Application.DTOs.ContentEntry; // استفاده از DTOهای جدید
using System;
using System.Collections.Generic; // اضافه شد
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IContentEntryService
    {
        // متدهای جدید که کنترلر صدا می‌زند
        Task<PagedResult<ContentEntryDto>> GetEntriesAsync(string typeSlug, int page, int pageSize);
        Task<ContentEntryDto> GetEntryByIdAsync(Guid id);
        Task<Guid> CreateEntryAsync(string typeSlug, ContentEntryCreateDto request);
        Task UpdateEntryAsync(Guid id, ContentEntryCreateDto request);
        Task DeleteEntryAsync(Guid id);
    }
}