// AnosheCms/Application/DTOs/Form/PublicFormDto.cs
// NEW FILE

using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Form
{
    public class PublicFormDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiSlug { get; set; }

        // (فیلدهای تنظیمات عمومی بر اساس بازنویسی Domain)
        public string SubmitButtonText { get; set; }
        public string? ConfirmationMessage { get; set; }
        public string? RedirectUrl { get; set; }

        // (لیست فیلدهای فرم برای رندر شدن)
        public List<FormFieldDto> Fields { get; set; } = new List<FormFieldDto>();
    }
}