using System;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ApiSlug { get; set; }
        // (سایر فیلدهای تنظیمات عمومی در FormGeneralSettingsDto مدیریت می‌شوند)
        public List<FormFieldDto> Fields { get; set; } = new List<FormFieldDto>();
    }
}