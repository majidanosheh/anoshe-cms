// File: AnosheCms.Infrastructure/Services/ContentTypeService.cs
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

        // (متدهای Create, AddField, Delete ... بدون تغییر از قبل)
        public async Task<ContentTypeDto?> CreateContentTypeAsync(CreateContentTypeDto dto)
        {
            if (await _context.ContentTypes.AnyAsync(ct => ct.ApiSlug == dto.ApiSlug)) return null;
            var contentType = new ContentType { Name = dto.Name, ApiSlug = dto.ApiSlug };
            _context.ContentTypes.Add(contentType);
            await _context.SaveChangesAsync();
            return MapToContentTypeDto(contentType);
        }

        public async Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto)
        {
            var contentType = await _context.ContentTypes.FindAsync(contentTypeId);
            if (contentType == null) return null;
            if (await _context.ContentFields.AnyAsync(cf => cf.ContentTypeId == contentTypeId && cf.ApiSlug == dto.ApiSlug)) return null;
            var contentField = new ContentField
            {
                Name = dto.Name,
                ApiSlug = dto.ApiSlug,
                FieldType = dto.FieldType,
                IsRequired = dto.IsRequired,
                Settings = dto.Settings,
                ContentTypeId = contentTypeId
            };
            _context.ContentFields.Add(contentField);
            await _context.SaveChangesAsync();
            return MapToContentFieldDto(contentField);
        }

        public async Task<bool> DeleteContentTypeAsync(Guid id)
        {
            var contentType = await _context.ContentTypes.FindAsync(id);
            if (contentType == null) return false;
            _context.ContentTypes.Remove(contentType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContentFieldAsync(Guid fieldId)
        {
            var contentField = await _context.ContentFields.FindAsync(fieldId);
            if (contentField == null) return false;
            _context.ContentFields.Remove(contentField);
            await _context.SaveChangesAsync();
            return true;
        }

        // ---*** شروع بخش اصلاح‌شده (رفع خطای 165.txt) ***---
        public async Task<List<ContentTypeDto>> GetAllContentTypesAsync()
        {
            // ۱. ابتدا داده‌ها را از دیتابیس به حافظه می‌آوریم
            var contentTypesFromDb = await _context.ContentTypes
                .AsNoTracking()
                .Include(ct => ct.Fields.Where(f => !f.IsDeleted))
                .OrderBy(ct => ct.Name)
                .ToListAsync(); // <-- اجرای کوئری در دیتابیس

            // ۲. اکنون در حافظه C# مپ می‌کنیم
            return contentTypesFromDb.Select(MapToContentTypeDto).ToList();
        }
        // ---*** پایان بخش اصلاح‌شده ***---

        public async Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug)
        {
            var contentType = await _context.ContentTypes
                .AsNoTracking()
                .Include(ct => ct.Fields.Where(f => !f.IsDeleted))
                .FirstOrDefaultAsync(ct => ct.ApiSlug == apiSlug);
            if (contentType == null) return null;
            return MapToContentTypeDto(contentType);
        }

        // --- متدهای کمکی خصوصی ---
        private ContentTypeDto MapToContentTypeDto(ContentType contentType)
        {
            return new ContentTypeDto(
                contentType.Id, contentType.Name, contentType.ApiSlug,
                contentType.Fields.Select(MapToContentFieldDto).ToList()
            );
        }

        private ContentFieldDto MapToContentFieldDto(ContentField field)
        {
            return new ContentFieldDto(
                field.Id, field.Name, field.ApiSlug, field.FieldType, field.IsRequired, field.Settings
            );
        }
    }
}