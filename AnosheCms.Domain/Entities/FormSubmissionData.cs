// مسیر: AnosheCms.Domain/Entities/FormSubmissionData.cs
using AnosheCms.Domain.Common;
using System;

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// (جدید - طبق docx)
    /// </summary>
    public class FormSubmissionData : BaseEntity
    {
        public Guid SubmissionId { get; set; }
        public virtual FormSubmission Submission { get; set; }

        public string FieldName { get; set; }

        public string? FieldValue { get; set; }
    }
}