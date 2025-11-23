using AnosheCms.Application.DTOs.ContentType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IContentTypeService
    {
        Task<List<ContentTypeDto>> GetAllContentTypesAsync();

        // متد جدید برای ویرایش
        Task<ContentTypeDto> GetContentTypeByIdAsync(Guid id);

        Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug);

        // تغییر خروجی به Guid برای هماهنگی با کنترلر
        Task<Guid> CreateContentTypeAsync(CreateContentTypeDto dto);

        // متد جدید برای آپدیت
        Task UpdateContentTypeAsync(Guid id, CreateContentTypeDto dto);

        Task<bool> DeleteContentTypeAsync(Guid id);

        Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto);
        Task<bool> DeleteContentFieldAsync(Guid contentTypeId, Guid fieldId);
    }
}