// File: AnosheCms.Domain/Entities/ContentItem.cs
using AnosheCms.Domain.Common;
using System;
using System.Collections.Generic; // برای Dictionary

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// نگه‌دارنده یک آیتم محتوای واقعی (مانند یک پست وبلاگ خاص)
    /// </summary>
    public class ContentItem : AuditableBaseEntity
    {
        /// <summary>
        /// این آیتم به کدام نوع محتوا تعلق دارد
        /// </summary>
        public Guid ContentTypeId { get; set; }
        public virtual ContentType ContentType { get; set; }

        /// <summary>
        /// وضعیت انتشار (Draft, Published, Archived)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// داده‌های داینامیک این آیتم که به صورت JSON ذخیره می‌شوند.
        /// ما از Dictionary برای کار آسان با JSON در C# استفاده می‌کنیم.
        /// </summary>
        public Dictionary<string, object> ContentData { get; set; } = new Dictionary<string, object>();

    }
}