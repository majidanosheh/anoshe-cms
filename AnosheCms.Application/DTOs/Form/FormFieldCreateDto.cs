// AnosheCms/Application/DTOs/Form/FormFieldCreateDto.cs
// NEW FILE

using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormFieldCreateDto
    {
        [Required(ErrorMessage = "فیلد 'Label' اجباری است.")]
        [StringLength(200)]
        public string Label { get; set; }

        [Required(ErrorMessage = "فیلد 'Name' (نام برنامه‌نویسی) اجباری است.")]
        [StringLength(100)]
        [RegularExpression("^[a-z0-9_]+$", ErrorMessage = "فیلد 'Name' فقط می‌تواند شامل حروف کوچک انگلیسی، اعداد و آندرلاین (_) باشد.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "فیلد 'FieldType' اجباری است.")]
        [StringLength(50)]
        public string FieldType { get; set; }

        public bool IsRequired { get; set; }

        [Range(0, 1000, ErrorMessage = "مقدار 'Order' باید معتبر باشد.")]
        public int Order { get; set; }

        [StringLength(500)]
        public string? Placeholder { get; set; }

        [StringLength(1000)]
        public string? HelpText { get; set; }

        public string? Settings { get; set; } // JSON
    }
}