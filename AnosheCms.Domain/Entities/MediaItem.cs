// File: AnosheCms.Domain/Entities/MediaItem.cs
using AnosheCms.Domain.Common;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// موجودیت نگهداری اطلاعات فایل‌های آپلود شده
    /// </summary>
    public class MediaItem : AuditableBaseEntity
    {
        /// <summary>
        /// نام فایل رمزنگاری شده روی دیسک (مثلاً: guid.jpg)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// نام اصلی فایل که کاربر آپلود کرده (مثلاً: my-photo.jpg)
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// نوع فایل (مثلاً: image/jpeg)
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// حجم فایل به بایت
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// متن جایگزین برای SEO
        /// </summary>
        public string? AltText { get; set; }

        /// <summary>
        /// مسیر ذخیره‌سازی نسبی (مثلاً: /uploads/2025/10/)
        /// </summary>
        public string FolderPath { get; set; }
    }
}