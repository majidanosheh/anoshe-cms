// AnosheCms/Application/DTOs/Form/FormFieldUpdateDto.cs
// FULL REWRITE

using System;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormFieldUpdateDto
    {
        [Required(ErrorMessage = "فیلد 'Label' اجbari است.")]
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

        public int Order { get; set; }

        [StringLength(500)]
        public string? Placeholder { get; set; } // NEW (From Frontend Doc [cite: 4])

        [StringLength(1000)]
        public string? HelpText { get; set; } // NEW (From Frontend Doc [cite: 7])

        // (Settings شامل قوانین اعتبارسنجی و منطق شرطی از بخش ۳ سند فرانت‌اند [cite: 8, 9] خواهد بود)
        public string? Settings { get; set; } // JSON string for options, validations, etc.

        public string? ValidationRules { get; set; }
        public string? ConditionalLogic { get; set; }
        public string? Options { get; set; }
    }
}