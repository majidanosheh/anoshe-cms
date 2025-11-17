// AnosheCms/Application/DTOs/Form/FormCreateDto.cs

using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormCreateDto
    {
        [Required(ErrorMessage = "فیلد 'Name' اجباری است.")]
        [StringLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "فیلد 'ApiSlug' اجباری است.")]
        [StringLength(100)]
        [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "اسلاگ فقط می‌تواند شامل حروف کوچک انگلیسی، اعداد و خط تیره (-) باشد.")]
        public string ApiSlug { get; set; }
    }
}