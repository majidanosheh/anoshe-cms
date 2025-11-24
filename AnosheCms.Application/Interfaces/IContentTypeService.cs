using AnosheCms.Application.DTOs.ContentType;

namespace AnosheCms.Application.Interfaces
{
    public interface IContentTypeService
    {
        Task<List<ContentTypeDto>> GetAllContentTypesAsync();
        Task<ContentTypeDto> GetContentTypeByIdAsync(Guid id);
        Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug);
        Task<Guid> CreateContentTypeAsync(CreateContentTypeDto dto);

        Task UpdateContentTypeAsync(Guid id, CreateContentTypeDto dto);
        Task DeleteContentTypeAsync(Guid id);

        Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto);
        Task<bool> DeleteContentFieldAsync(Guid contentTypeId, Guid fieldId);
    }
}