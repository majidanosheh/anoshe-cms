// File: AnosheCms.Application/Interfaces/IContentItemService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public record ContentItemCreateDto(Dictionary<string, object> ContentData, string Status);
    public record ContentItemDto(
        Guid Id, Guid ContentTypeId, string Status, DateTime CreatedDate,
        string? CreatedBy, DateTime? LastModifiedDate, Dictionary<string, object> ContentData
    );

    public interface IContentItemService
    {
        // Admin
        Task<List<ContentItemDto>> GetContentItemsAsync(string contentTypeSlug);
        Task<ContentItemDto?> GetContentItemByIdAsync(string contentTypeSlug, Guid itemId);
        Task<(ContentItemDto? Dto, string? ErrorMessage)> CreateContentItemAsync(string contentTypeSlug, ContentItemCreateDto dto);
        Task<(ContentItemDto? Dto, string? ErrorMessage)> UpdateContentItemAsync(string contentTypeSlug, Guid itemId, ContentItemCreateDto dto);
        Task<bool> DeleteContentItemAsync(string contentTypeSlug, Guid itemId);

        // Public (رفع خطای 162.txt)
        Task<List<ContentItemDto>> GetPublishedContentItemsAsync(string contentTypeSlug);
        Task<ContentItemDto?> GetPublishedContentItemAsync(string contentTypeSlug, string itemApiSlug);
    }
}