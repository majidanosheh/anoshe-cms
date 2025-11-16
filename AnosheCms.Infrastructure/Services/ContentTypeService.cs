using AnosheCms.Application.DTOs.ContentType;
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
    public class ContentTypeService : IContentTypeService
    {
        private readonly ApplicationDbContext _context;

        public ContentTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ContentTypeDto?> CreateContentTypeAsync(CreateContentTypeDto dto)
        {
            if (await _context.ContentTypes.AnyAsync(ct => ct.ApiSlug == dto.ApiSlug))
            {
                return null; // Slug exists
            }

            var contentType = new ContentType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                ApiSlug = dto.ApiSlug
            };

            await _context.ContentTypes.AddAsync(contentType);
            await _context.SaveChangesAsync();

            return MapToDto(contentType);
        }

        public async Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug)
        {
            var contentType = await _context.ContentTypes
                .AsNoTracking()
                .Include(ct => ct.Fields)
                .FirstOrDefaultAsync(ct => ct.ApiSlug == apiSlug);

            return contentType == null ? null : MapToDto(contentType);
        }

       
        public async Task<List<ContentTypeDto>> GetAllContentTypesAsync()
        {
            // ۱. ابتدا داده‌ها را از دیتابیس به حافظه می‌آوریم
            var contentTypesFromDb = await _context.ContentTypes
                .AsNoTracking()
                .Include(ct => ct.Fields) // (یا .Where(f => !f.IsDeleted) اگر فیلتر دارید)
                .OrderBy(ct => ct.Name)
                .ToListAsync(); // <-- .ToListAsync() به اینجا منتقل شد

            // ۲. اکنون در حافظه (Client-side) آن‌ها را Map می‌کنیم
            // (مطمئن شوید که نام متد Map شما "MapToDto" است)
            return contentTypesFromDb.Select(ct => MapToDto(ct)).ToList();
        }
        public async Task<bool> DeleteContentTypeAsync(Guid id)
        {
            var contentType = await _context.ContentTypes
                .Include(ct => ct.ContentItems)
                .Include(ct => ct.Fields)
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (contentType == null) return false;

            foreach (var item in contentType.ContentItems)
            {
                _context.ContentItems.Remove(item);
            }
            foreach (var field in contentType.Fields)
            {
                _context.ContentFields.Remove(field);
            }

            _context.ContentTypes.Remove(contentType);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto)
        {
            var contentType = await _context.ContentTypes.FindAsync(contentTypeId);
            if (contentType == null) return null;

            var field = new ContentField
            {
                Id = Guid.NewGuid(),
                ContentTypeId = contentTypeId,
                Name = dto.Name,
                ApiSlug = dto.ApiSlug,
                FieldType = dto.FieldType,
                IsRequired = dto.IsRequired
                // (توجه: فیلد پنجم 'Options' که در DTO بود، در Entity ذخیره نمی‌شود)
                // (این برای پیاده‌سازی‌های آینده است)
            };

            await _context.ContentFields.AddAsync(field);
            await _context.SaveChangesAsync();

            return MapToFieldDto(field);
        }

        // (اصلاح شد) نام متد برای مطابقت با کنترلر تغییر کرد
        public async Task<bool> DeleteContentFieldAsync(Guid contentTypeId, Guid fieldId)
        {
            var field = await _context.ContentFields
                .FirstOrDefaultAsync(f => f.Id == fieldId && f.ContentTypeId == contentTypeId);

            if (field == null) return false;

            _context.ContentFields.Remove(field);
            return await _context.SaveChangesAsync() > 0;
        }


        // --- Helpers ---
        private ContentTypeDto MapToDto(ContentType ct)
        {
            return new ContentTypeDto
            {
                Id = ct.Id,
                Name = ct.Name,
                ApiSlug = ct.ApiSlug,
                Fields = ct.Fields?.Select(MapToFieldDto).ToList() ?? new List<ContentFieldDto>()
            };
        }

        private ContentFieldDto MapToFieldDto(ContentField f)
        {
            return new ContentFieldDto
            {
                Id = f.Id,
                Name = f.Name,
                ApiSlug = f.ApiSlug,
                FieldType = f.FieldType,
                IsRequired = f.IsRequired
            };
        }
    }
}