using AnosheCms.Application.DTOs.ContentEntry;
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

        public async Task<PagedResult<ContentEntryDto>> GetEntriesAsync(string typeSlug, int page, int pageSize)
        {
            var query = _context.ContentItems
                .AsNoTracking()
                .Where(i => i.ContentType.ApiSlug == typeSlug && !i.IsDeleted)
                .OrderByDescending(i => i.CreatedDate);

            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedResult<ContentEntryDto>
            {
                TotalCount = total,
                Items = items.Select(MapToDto).ToList()
            };
        }

        public async Task<ContentEntryDto> GetEntryByIdAsync(Guid id)
        {
            var item = await _context.ContentItems.FindAsync(id);
            return item == null ? null : MapToDto(item);
        }

        public async Task<Guid> CreateEntryAsync(string typeSlug, ContentEntryCreateDto request)
        {
            var contentType = await _context.ContentTypes.FirstOrDefaultAsync(c => c.ApiSlug == typeSlug);
            if (contentType == null) throw new Exception("نوع محتوا یافت نشد.");

            var item = new ContentItem
            {
                ContentTypeId = contentType.Id,
                DataJson = System.Text.Json.JsonSerializer.Serialize(request.ContentData),
                Status = request.Status ?? "Published"
            };

            _context.ContentItems.Add(item);
            await _context.SaveChangesAsync();
            return item.Id;
        }

        public async Task UpdateEntryAsync(Guid id, ContentEntryCreateDto request)
        {
            var item = await _context.ContentItems.FindAsync(id);
            if (item == null) throw new Exception("آیتم یافت نشد.");

            item.DataJson = System.Text.Json.JsonSerializer.Serialize(request.ContentData);
            item.Status = request.Status ?? item.Status;
            item.LastModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEntryAsync(Guid id)
        {
            var item = await _context.ContentItems.FindAsync(id);
            if (item != null)
            {
                // Soft Delete (توسط Context هندل می‌شود یا دستی)
                _context.ContentItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        private ContentEntryDto MapToDto(ContentItem item)
        {
            // تبدیل JSON استرینگ به دیکشنری
            var data = string.IsNullOrEmpty(item.DataJson)
                ? new Dictionary<string, object>()
                : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(item.DataJson);

            return new ContentEntryDto(
                item.Id,
                item.ContentTypeId,
                item.Status,
                item.CreatedDate,
                item.CreatedBy?.ToString(),
                item.LastModifiedDate,
                data
            );
        }
    }
}