//// File: AnosheCms.Application/Interfaces/IMediaService.cs
//using AnosheCms.Domain.Entities;
//using Microsoft.AspNetCore.Http; // <-- به این using نیاز داریم
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace AnosheCms.Application.Interfaces
//{
//    // DTO (Data Transfer Object) برای پاسخ آپلود
//    public record MediaUploadResult(bool Succeeded, MediaItem? MediaItem = null, string? ErrorMessage = null);

//    // DTO برای به‌روزرسانی
//    public record MediaUpdateDto(string? AltText);

//    public interface IMediaService
//    {
//        /// <summary>
//        /// آپلود یک فایل جدید
//        /// </summary>
//        /// <param name="file">فایل دریافت شده از درخواست HTTP</param>
//        /// <param name="altText">متن جایگزین (اختیاری)</param>
//        /// <returns>اطلاعات فایل ذخیره شده در دیتابیس</returns>
//        Task<MediaUploadResult> UploadFileAsync(IFormFile file, string? altText);

//        /// <summary>
//        /// دریافت لیست تمام فایل‌ها (برای پنل مدیریت)
//        /// </summary>
//        Task<List<MediaItem>> GetAllMediaItemsAsync();

//        /// <summary>
//        /// به‌روزرسانی متادیتای یک فایل (مثلاً Alt Text)
//        /// </summary>
//        Task<bool> UpdateMediaItemAsync(Guid id, MediaUpdateDto updateDto);

//        /// <summary>
//        /// حذف یک فایل (Soft Delete در دیتابیس و حذف فیزیکی فایل)
//        /// </summary>
//        Task<bool> DeleteMediaItemAsync(Guid id);

//        /// <summary>
//        /// دریافت یک آیتم خاص
//        /// </summary>
//        Task<MediaItem?> GetMediaItemByIdAsync(Guid id);
//    }
//}

// File: AnosheCms.Application/Interfaces/IMediaService.cs
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    // DTO برای بازگرداندن اطلاعات فایل
    public record MediaDto(
        Guid Id,
        string FileName,
        string ContentType,
        long Size,
        string Url
    );

    public interface IMediaService
    {
        Task<List<MediaDto>> GetAllMediaAsync();
        Task<MediaDto?> GetMediaByIdAsync(Guid id);
        Task<List<MediaDto>> UploadMediaAsync(IFormFileCollection files);
        Task<bool> DeleteMediaAsync(Guid id);
    }
}