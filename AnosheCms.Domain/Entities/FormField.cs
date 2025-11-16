// مسیر: AnosheCms.Domain/Entities/FormField.cs
using AnosheCms.Domain.Common;
using System;
using System.Collections.Generic;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// تعریف کننده یک فیلد در یک فرم (مانند "فیلد ایمیل")
    /// </summary>
    public class FormField : AuditableBaseEntity
    {
        public Guid FormId { get; set; }
        public virtual Form Form { get; set; }

        public string Name { get; set; } // (نام فنی، e.g. "email_address")
        public string Label { get; set; } // (برچسب قابل نمایش، e.g. "آدرس ایمیل")
        public string FieldType { get; set; } // (e.g., "Text", "Email", "Number", "Textarea", "Checkbox", "Dropdown")
        public bool IsRequired { get; set; }

        /// <summary>
        /// تنظیمات اضافی فیلد به صورت JSON
        /// (e.g., {"options": ["Option 1", "Option 2"]})
        /// </summary>
        public Dictionary<string, object> Settings { get; set; } = new Dictionary<string, object>();
    }
}