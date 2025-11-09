// File: AnosheCms.Infrastructure/Services/ContentEntryService.cs
// (تصحیح‌شده برای خطای Client Projection)

using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class ContentEntryService : IContentEntryService
    {
        private readonly ApplicationDbContext _context;
        public ContentEntryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // (متدهای کمکی بدون تغییر)
        private async Task<ContentType?> GetContentTypeWithFields(string contentTypeSlug)
        {
            return await _context.ContentTypes
                .Include(ct => ct.Fields.Where(f => !f.IsDeleted))
                .FirstOrDefaultAsync(ct => ct.ApiSlug == contentTypeSlug && !ct.IsDeleted);
        }

        private string? ValidateContentData(ContentType contentType, Dictionary<string, object> contentData)
        {
            foreach (var field in contentType.Fields.Where(f => f.IsRequired))
            {
                if (!contentData.ContainsKey(field.ApiSlug) || contentData[field.ApiSlug] == null ||
                    (contentData[field.ApiSlug] is string s && string.IsNullOrWhiteSpace(s)))
                {
                    return $"فیلد اجباری '{field.Name}' مقداردهی نشده است.";
                }
            }
            return null;
        }

        private ContentEntryDto MapToDto(ContentItem item)
        {
            return new ContentEntryDto(
                item.Id, item.ContentTypeId, item.Status, item.CreatedDate,
                item.CreatedBy.ToString(), item.LastModifiedDate, item.ContentData
            );
        }

        // --- متدهای ادمین ---

        // ... (متدهای Create, Update, Delete, GetById بدون تغییر هستند چون Select ندارند) ...
        public async Task<(ContentEntryDto? Dto, string? ErrorMessage)> CreateContentEntryAsync(string contentTypeSlug, ContentEntryCreateDto dto)
        {
            var contentType = await GetContentTypeWithFields(contentTypeSlug);
            if (contentType == null) return (null, "نوع محتوا یافت نشد.");
            var validationError = ValidateContentData(contentType, dto.ContentData);
            if (validationError != null) return (null, validationError);
            var newItem = new ContentItem { ContentTypeId = contentType.Id, Status = dto.Status ?? "Draft", ContentData = dto.ContentData };
            _context.ContentItems.Add(newItem);
            await _context.SaveChangesAsync();
            return (MapToDto(newItem), null);
        }

        public async Task<(ContentEntryDto? Dto, string? ErrorMessage)> UpdateContentEntryAsync(string contentTypeSlug, Guid itemId, ContentEntryCreateDto dto)
        {
            var contentType = await GetContentTypeWithFields(contentTypeSlug);
            if (contentType == null) return (null, "نوع محتوا یافت نشد.");
            var itemToUpdate = await _context.ContentItems.FirstOrDefaultAsync(ci => ci.Id == itemId && ci.ContentType.ApiSlug == contentTypeSlug);
            if (itemToUpdate == null) return (null, "آیتم محتوا یافت نشد.");
            var validationError = ValidateContentData(contentType, dto.ContentData);
            if (validationError != null) return (null, validationError);
            itemToUpdate.ContentData = dto.ContentData;
            itemToUpdate.Status = dto.Status ?? itemToUpdate.Status;
            await _context.SaveChangesAsync();
            return (MapToDto(itemToUpdate), null);
        }

        public async Task<bool> DeleteContentEntryAsync(string contentTypeSlug, Guid itemId)
        {
            var itemToDelete = await _context.ContentItems.FirstOrDefaultAsync(ci => ci.Id == itemId && ci.ContentType.ApiSlug == contentTypeSlug);
            if (itemToDelete == null) return false;
            _context.ContentItems.Remove(itemToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ContentEntryDto?> GetContentEntryByIdAsync(string contentTypeSlug, Guid itemId)
        {
            var item = await _context.ContentItems.AsNoTracking().FirstOrDefaultAsync(ci => ci.Id == itemId && ci.ContentType.ApiSlug == contentTypeSlug);
            return item == null ? null : MapToDto(item);
        }


        // --- *** متد تصحیح‌شده (خطای 170.txt) *** ---
        public async Task<List<ContentEntryDto>> GetContentEntriesAsync(string contentTypeSlug)
        {
            // 1. ابتدا داده‌ها را از دیتابیس واکشی کن
            var itemsFromDb = await _context.ContentItems.AsNoTracking()
                .Where(ci => ci.ContentType.ApiSlug == contentTypeSlug)
                .OrderByDescending(ci => ci.CreatedDate)
                .ToListAsync(); // <-- .ToListAsync() قبل از .Select() فراخوانی می‌شود

            // 2. حالا در حافظه (Client-side) آن‌ها را Map کن
            return itemsFromDb.Select(item => MapToDto(item)).ToList();
        }

        // --- *** متد تصحیح‌شده (خطای 170.txt) *** ---
        public async Task<List<ContentEntryDto>> GetPublishedContentEntriesAsync(string contentTypeSlug)
        {
            // 1. ابتدا داده‌ها را از دیتابیس واکشی کن
            var itemsFromDb = await _context.ContentItems.AsNoTracking()
                .Where(ci => ci.ContentType.ApiSlug == contentTypeSlug && ci.Status == "Published")
                .OrderByDescending(ci => ci.CreatedDate)
                .ToListAsync(); // <-- .ToListAsync() قبل از .Select() فراخوانی می‌شود

            // 2. حالا در حافظه (Client-side) آن‌ها را Map کن
            return itemsFromDb.Select(item => MapToDto(item)).ToList();
        }

        // --- (متد GetPublishedContentEntryAsync از قبل درست بود) ---
        public async Task<ContentEntryDto?> GetPublishedContentEntryAsync(string contentTypeSlug, string itemApiSlug)
        {
            if (Guid.TryParse(itemApiSlug, out Guid itemId))
            {
                var item = await _context.ContentItems.AsNoTracking()
                   .FirstOrDefaultAsync(ci => ci.ContentType.ApiSlug == contentTypeSlug && ci.Status == "Published" && ci.Id == itemId);
                return item == null ? null : MapToDto(item);
            }
            return null;
        }
    }
}