// File: AnosheCms.Infrastructure/Services/SettingsService.cs
using AnosheCms.Application.DTOs.ContentEntry;
using AnosheCms.Application.DTOs.ContentType;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentEntryCreateDto = AnosheCms.Application.Interfaces.ContentEntryCreateDto;

namespace AnosheCms.Infrastructure.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IContentEntryService _contentEntryService;
        private readonly IContentTypeService _contentTypeService;
        private readonly ApplicationDbContext _context;

        public SettingsService(
            IContentEntryService contentEntryService,
            IContentTypeService contentTypeService,
            ApplicationDbContext context)
        {
            _contentEntryService = contentEntryService;
            _contentTypeService = contentTypeService;
            _context = context;
        }

        private async Task<ContentItem> GetOrCreateSettingsEntry(string contentTypeSlug, Guid contentTypeId, Guid userId)
        {
            var entry = await _context.ContentItems
                .FirstOrDefaultAsync(ci => ci.ContentType.ApiSlug == contentTypeSlug);

            if (entry == null)
            {
                // اگر ورودی تنظیمات وجود نداشته باشد، اولین ورودی را ایجاد می‌کند
                var (dto, error) = await _contentEntryService.CreateContentEntryAsync(contentTypeSlug, new ContentEntryCreateDto(
                    new Dictionary<string, object>(), // با دیتای خالی شروع می‌کند
                    "Published"
                ));

                if (dto == null)
                {
                    throw new InvalidOperationException($"Could not create initial settings entry: {error}");
                }

                entry = await _context.ContentItems.FindAsync(dto.Id);
            }
            return entry;
        }

        public async Task<(Dictionary<string, object>? Data, string? ErrorMessage)> GetSettingsAsync(string contentTypeSlug)
        {
            var contentType = await _contentTypeService.GetContentTypeBySlugAsync(contentTypeSlug);
            if (contentType == null)
            {
                //CreateContentTypeDto باعث ارور شده

                var newContentType = await _contentTypeService.CreateContentTypeAsync(new CreateContentTypeDto("Global Settings", contentTypeSlug));
                if (newContentType == null)
                {
                    return (null, "Failed to create the Global Settings Content Type.");
                }
                // اضافه کردن چند فیلد پیش‌فرض برای نمونه
                await _contentTypeService.AddFieldToContentTypeAsync(newContentType.Id,
                    //CreateContentFieldDto باعث ارور شده
                    new CreateContentFieldDto("Site Title", "site_title", "Text", true, null));
                await _contentTypeService.AddFieldToContentTypeAsync(newContentType.Id,
                    //CreateContentFieldDto باعث ارور شده

                    new CreateContentFieldDto("Site Logo URL", "site_logo", "Media", false, null));
            }

            var entry = await _context.ContentItems
                .AsNoTracking()
                .FirstOrDefaultAsync(ci => ci.ContentType.ApiSlug == contentTypeSlug);

            if (entry == null)
            {
                // هنوز هیچ تنظیماتی ذخیره نشده، دیکشنری خالی برمی‌گرداند
                return (new Dictionary<string, object>(), null);
            }

            return (entry.ContentData, null);
        }

        public async Task<(Dictionary<string, object>? Data, string? ErrorMessage)> UpdateSettingsAsync(string contentTypeSlug, Dictionary<string, object> data, Guid userId)
        {
            var contentType = await _context.ContentTypes
                .AsNoTracking()
                .FirstOrDefaultAsync(ct => ct.ApiSlug == contentTypeSlug);

            if (contentType == null)
            {
                return (null, "Global Settings Content Type not found.");
            }

            var entry = await GetOrCreateSettingsEntry(contentTypeSlug, contentType.Id, userId);

            // استفاده از منطق به‌روزرسانی سرویس ContentEntryService
            var (updatedDto, error) = await _contentEntryService.UpdateContentEntryAsync(
                contentTypeSlug,
                entry.Id,
                new ContentEntryCreateDto(data, "Published")
            );

            if (error != null)
            {
                return (null, error);
            }

            return (updatedDto?.ContentData, null);
        }
    }
}