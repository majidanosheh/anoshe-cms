using AnosheCms.Application.DTOs.ContentType;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IContentTypeService
    {
        Task<ContentTypeDto?> CreateContentTypeAsync(CreateContentTypeDto dto);
        Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug);
        Task<List<ContentTypeDto>> GetAllContentTypesAsync();
        Task<bool> DeleteContentTypeAsync(Guid id);
        Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto);

        // (اصلاح شد) نام متد برای مطابقت با کنترلر تغییر کرد
        Task<bool> DeleteContentFieldAsync(Guid contentTypeId, Guid fieldId);
    }
}