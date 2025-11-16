namespace AnosheCms.Application.DTOs.ContentType
{
    public class CreateContentFieldDto
    {
        public string Name { get; set; }
        public string ApiSlug { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }

        // (جدید) فیلد پنجم که در SettingsService.cs به آن null پاس داده شده است
        // (ما فرض می‌کنیم این فیلد برای تنظیمات اضافی مانند گزینه‌های Select است)
        public object? Options { get; set; }

        // (جدید)
        // سازنده‌ای که در SettingsService.cs فراخوانی شده است
        public CreateContentFieldDto(string name, string apiSlug, string fieldType, bool isRequired, object? options)
        {
            Name = name;
            ApiSlug = apiSlug;
            FieldType = fieldType;
            IsRequired = isRequired;
            Options = options;
        }
    }
}