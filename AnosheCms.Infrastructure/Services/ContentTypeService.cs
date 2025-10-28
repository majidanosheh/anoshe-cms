// ---*** شروع Using Directives ***---
// (این بخش خطاهای شما را برطرف می‌کند)
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data; // <-- برای ApplicationDbContext
using Microsoft.EntityFrameworkCore; // <-- برای EntityFrameworkCore و .AnyAsync()
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
// ---*** پایان Using Directives ***---

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
            // بررسی اینکه ApiSlug یونیک باشد
            var slugExists = await _context.ContentTypes.AnyAsync(ct => ct.ApiSlug == dto.ApiSlug);
            if (slugExists)
            {
                // در یک API واقعی، باید یک خطای سفارشی یا null با پیام برگردانیم
                return null;
            }

            var contentType = new ContentType
            {
                Name = dto.Name,
                ApiSlug = dto.ApiSlug
            };

            _context.ContentTypes.Add(contentType);
            await _context.SaveChangesAsync(); // Auditing (CreatedBy) در اینجا اعمال می‌شود

            return MapToContentTypeDto(contentType);
        }

        public async Task<ContentFieldDto?> AddFieldToContentTypeAsync(Guid contentTypeId, CreateContentFieldDto dto)
        {
            var contentType = await _context.ContentTypes.FindAsync(contentTypeId);
            if (contentType == null)
            {
                return null; // نوع محتوا یافت نشد
            }

            // بررسی اینکه ApiSlug فیلد در این ContentType یونیک باشد
            var fieldSlugExists = await _context.ContentFields
                .AnyAsync(cf => cf.ContentTypeId == contentTypeId && cf.ApiSlug == dto.ApiSlug);

            if (fieldSlugExists)
            {
                return null; // فیلد تکراری
            }

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

            _context.ContentTypes.Remove(contentType); // Soft Delete خودکار
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteContentFieldAsync(Guid fieldId)
        {
            var contentField = await _context.ContentFields.FindAsync(fieldId);
            if (contentField == null) return false;

            _context.ContentFields.Remove(contentField); // Soft Delete خودکار
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ContentTypeDto>> GetAllContentTypesAsync()
        {
            // اطمینان از اعمال فیلتر Soft Delete (که به طور خودکار انجام می‌شود)
            return await _context.ContentTypes
                .OrderBy(ct => ct.Name)
                .Select(ct => new ContentTypeDto(ct.Id, ct.Name, ct.ApiSlug, new List<ContentFieldDto>())) // فیلدها را برای لیست کامل برنمی‌گردانیم
                .ToListAsync();
        }

        public async Task<ContentTypeDto?> GetContentTypeBySlugAsync(string apiSlug)
        {
            var contentType = await _context.ContentTypes
                .Include(ct => ct.Fields) // فیلدهای مرتبط را بارگذاری می‌کنیم
                .FirstOrDefaultAsync(ct => ct.ApiSlug == apiSlug);

            if (contentType == null) return null;

            return MapToContentTypeDto(contentType);
        }


        // --- متدهای کمکی برای Map کردن Entity به DTO ---
        private ContentTypeDto MapToContentTypeDto(ContentType contentType)
        {
            return new ContentTypeDto(
                contentType.Id,
                contentType.Name,
                contentType.ApiSlug,
                // فیلتر کردن فیلدهایی که Soft Delete نشده‌اند
                contentType.Fields.Where(f => !f.IsDeleted).Select(MapToContentFieldDto).ToList()
            );
        }

        private ContentFieldDto MapToContentFieldDto(ContentField field)
        {
            return new ContentFieldDto(
                field.Id,
                field.Name,
                field.ApiSlug,
                field.FieldType,
                field.IsRequired,
                field.Settings
            );
        }
    }
}