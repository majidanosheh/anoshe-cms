// --- شروع Using Directives ---
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Hosting; // برای IWebHostEnvironment
using Microsoft.AspNetCore.Http;   // برای IFormFile
using Microsoft.EntityFrameworkCore; // برای ToListAsync
using System;
using System.Collections.Generic;
using System.IO; // برای Path و File
using System.Linq;
using System.Threading.Tasks;
// --- پایان Using Directives ---

namespace AnosheCms.Infrastructure.Services
{
    public class MediaService : IMediaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment; // برای پیدا کردن مسیر wwwroot

        // مسیر پایه برای ذخیره فایل‌ها در داخل پروژه Api
        private readonly string _baseUploadPath = "uploads";

        public MediaService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<MediaUploadResult> UploadFileAsync(IFormFile file, string? altText)
        {
            if (file == null || file.Length == 0)
            {
                return new MediaUploadResult(false, ErrorMessage: "هیچ فایلی ارائه نشده است.");
            }

            // TODO: افزودن اعتبارسنجی برای نوع فایل و حجم فایل

            var year = DateTime.UtcNow.Year.ToString();
            var month = DateTime.UtcNow.Month.ToString("D2"); // D2: 10, 09, 08

            // 1. ایجاد مسیر نسبی (e.g., /uploads/2025/10)
            var relativeFolderPath = Path.Combine(_baseUploadPath, year, month);

            // 2. ایجاد مسیر فیزیکی کامل (e.g., C:\...\AnosheCms.Api\wwwroot\uploads\2025\10)
            var physicalFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, relativeFolderPath);

            // اطمینان از وجود پوشه
            Directory.CreateDirectory(physicalFolderPath);

            // 3. ایجاد نام فایل یونیک
            var fileExtension = Path.GetExtension(file.FileName);
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var physicalFilePath = Path.Combine(physicalFolderPath, uniqueFileName);

            // 4. ذخیره فایل فیزیکی
            try
            {
                await using (var stream = new FileStream(physicalFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // TODO: لاگ کردن خطا
                return new MediaUploadResult(false, ErrorMessage: $"ذخیره فایل با شکست مواجه شد: {ex.Message}");
            }

            // 5. ایجاد موجودیت متادیتا
            var mediaItem = new MediaItem
            {
                FileName = uniqueFileName,
                OriginalFileName = file.FileName,
                MimeType = file.ContentType,
                FileSize = file.Length,
                AltText = altText,
                FolderPath = relativeFolderPath.Replace("\\", "/") // استفاده از اسلش در دیتابیس
            };

            // 6. ذخیره متادیتا در دیتابیس
            _context.MediaItems.Add(mediaItem);
            await _context.SaveChangesAsync(); // فیلدهای Auditing در اینجا پر می‌شوند

            return new MediaUploadResult(true, MediaItem: mediaItem);
        }

        public async Task<bool> DeleteMediaItemAsync(Guid id)
        {
            var mediaItem = await _context.MediaItems.FindAsync(id);
            if (mediaItem == null || mediaItem.IsDeleted)
            {
                return false; // یافت نشد یا قبلاً حذف شده
            }

            // 1. حذف فایل فیزیکی
            var physicalFilePath = Path.Combine(_webHostEnvironment.WebRootPath, mediaItem.FolderPath, mediaItem.FileName);
            if (File.Exists(physicalFilePath))
            {
                try
                {
                    File.Delete(physicalFilePath);
                }
                catch (Exception)
                {
                    // TODO: لاگ کردن خطا - نتوانستیم فایل را پاک کنیم اما آیتم دیتابیس را حذف می‌کنیم
                }
            }

            // 2. حذف از دیتابیس (Soft Delete به طور خودکار توسط DbContext انجام می‌شود)
            _context.MediaItems.Remove(mediaItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MediaItem>> GetAllMediaItemsAsync()
        {
            // Soft Delete به طور خودکار فیلتر می‌شود
            return await _context.MediaItems.OrderByDescending(m => m.CreatedDate).ToListAsync();
        }

        public async Task<MediaItem?> GetMediaItemByIdAsync(Guid id)
        {
            return await _context.MediaItems.FindAsync(id);
        }

        public async Task<bool> UpdateMediaItemAsync(Guid id, MediaUpdateDto updateDto)
        {
            var mediaItem = await _context.MediaItems.FindAsync(id);
            if (mediaItem == null)
            {
                return false;
            }

            mediaItem.AltText = updateDto.AltText;
            // فیلد LastModifiedBy به طور خودکار توسط DbContext آپدیت می‌شود
            await _context.SaveChangesAsync();
            return true;
        }
    }
}