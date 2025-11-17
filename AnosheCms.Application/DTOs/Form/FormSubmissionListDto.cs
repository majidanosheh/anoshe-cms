// AnosheCms/Application/DTOs/Form/FormSubmissionListDto.cs
// NEW FILE

using System;

namespace AnosheCms.Application.DTOs.Form
{
    // DTO سبک برای نمایش لیست کلی پاسخ‌ها
    public class FormSubmissionListDto
    {
        public Guid Id { get; set; }
        public DateTime SubmittedDate { get; set; } // (همان CreatedDate از AuditableBaseEntity)
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
    }
}