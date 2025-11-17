// مسیر: AnosheCms.Domain/Entities/FormSubmission.cs
using AnosheCms.Domain.Common;
using System;
using System.Collections.Generic;

namespace AnosheCms.Domain.Entities
{
    public class FormSubmission : AuditableBaseEntity
    {
        public Guid FormId { get; set; }
        public virtual Form Form { get; set; }

        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public virtual ICollection<FormSubmissionData> SubmissionData { get; set; } = new List<FormSubmissionData>();
    }
}