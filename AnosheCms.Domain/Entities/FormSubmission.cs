// مسیر: AnosheCms.Domain/Entities/FormSubmission.cs
using AnosheCms.Domain.Common;
using System;
using System.Collections.Generic;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// نگه‌دارنده یک رکورد ارسال شده از یک فرم
    /// </summary>
    public class FormSubmission : AuditableBaseEntity
    {
        public Guid FormId { get; set; }
        public virtual Form Form { get; set; }

        /// <summary>
        /// داده‌های ارسالی فرم به صورت JSON
        /// (e.g., {"email_address": "test@example.com", "message": "Hello"})
        /// </summary>
        public Dictionary<string, object> SubmissionData { get; set; } = new Dictionary<string, object>();
    }
}