// File: AnosheCms.Domain/Entities/ContentField.cs
using AnosheCms.Domain.Common;
using System;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// تعریف کننده یک فیلد خاص در یک ContentType
    /// </summary>
    public class ContentField : AuditableBaseEntity
    {
        /// <summary>
        /// نام قابل خواندن فیلد (مثلاً: عنوان اصلی)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// نام فنی فیلد برای استفاده در JSON (مثلاً: title)
        /// </summary>
        public string ApiSlug { get; set; }

        /// <summary>
        /// نوع داده فیلد (Text, RichText, Number, Boolean, DateTime, Media, Relation)
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// آیا این فیلد اجباری است؟
        /// </summary>
        public bool IsRequired { get; set; } = false;

        /// <summary>
        
        /// </summary>
        public string? Settings { get; set; }

        /// <summary>
        /// این فیلد به کدام ContentType تعلق دارد
        /// </summary>
        public Guid ContentTypeId { get; set; }
        public virtual ContentType ContentType { get; set; }
    }
}