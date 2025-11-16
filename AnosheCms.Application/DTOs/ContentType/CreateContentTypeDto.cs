namespace AnosheCms.Application.DTOs.ContentType
{
    public class CreateContentTypeDto
    {
        public string Name { get; set; }
        public string ApiSlug { get; set; }

        // (جدید)
        // سازنده‌ای که در SettingsService.cs فراخوانی شده است
        public CreateContentTypeDto(string name, string apiSlug)
        {
            Name = name;
            ApiSlug = apiSlug;
        }
    }
}