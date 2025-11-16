// مسیر: AnosheCms.Domain/Entities/Form.cs
using AnosheCms.Domain.Common;
using System.Collections.Generic;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// تعریف کننده یک فرم (مانند "فرم تماس با ما")
    /// </summary>
    public class Form : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string ApiSlug { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<FormField> Fields { get; set; } = new List<FormField>();
        public virtual ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();
    }
}