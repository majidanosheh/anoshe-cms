// File: AnosheCms.Domain/Entities/ContentType.cs
using AnosheCms.Domain.Common;
using System.Collections.Generic;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// تعریف کننده یک نوع محتوا (مانند: مقاله وبلاگ، محصول، صفحه)
    /// </summary>
    public class ContentType : AuditableBaseEntity
    {
        /// <summary>
        /// نام قابل خواندن (مثلاً: مقاله وبلاگ)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// نام فنی برای استفاده در API (مثلاً: blog-posts)
        /// </summary>
        public string ApiSlug { get; set; }

        /// <summary>
        /// لیستی از فیلدهای تعریف شده برای این نوع محتوا
        /// </summary>
        public virtual ICollection<ContentField> Fields { get; set; } = new List<ContentField>();
        public virtual ICollection<ContentItem> ContentItems { get; set; }
        // TODO: در آینده، لیستی از ContentItem های واقعی را اضافه خواهیم کرد
        // public virtual ICollection<ContentItem> Items { get; set; } = new List<ContentItem>();
    }
}