using System;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormSubmissionDto
    {
        // شناسه فرمی که پر شده است
        public Guid FormId { get; set; }

        // پاسخ‌های کاربر به صورت رشته JSON
        // مثال: {"name": "Ali", "age": "25"}
        public string Data { get; set; }
    }
}