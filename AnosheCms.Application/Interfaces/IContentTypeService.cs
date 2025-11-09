// File: AnosheCms.Application/Interfaces/IContentTypeService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public record CreateContentTypeDto(string Name, string ApiSlug);
    public record CreateContentFieldDto(string Name, string ApiSlug, string FieldType, bool IsRequired, string? Settings);
    public record ContentFieldDto(Guid Id, string Name, string ApiSlug, string FieldType, bool IsRequired, string? Settings);
    public record ContentTypeDto(Guid Id, string Name, string ApiSlug, List<ContentFieldDto> Fields);

    public interface IContentTypeService
    {
        Task<ContentTypeDto?> CreateContentTypeAsync(CreateContentTypeDto dto);
        Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug);
        Task<List<ContentTypeDto>> GetAllContentTypesAsync();
        Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto);
        Task<bool> DeleteContentTypeAsync(Guid id);
        Task<bool> DeleteContentFieldAsync(Guid fieldId);
    }
}