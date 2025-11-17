// AnosheCms/Application/DTOs/Form/FormGeneralSettingsDto.cs
// NEW FILE

using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormGeneralSettingsDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression("^[a-z0-9-]+$", ErrorMessage = "اسلاگ فقط می‌تواند شامل حروف کوچک انگلیسی، اعداد و خط تیره (-) باشد.")]
        public string ApiSlug { get; set; }

        [StringLength(100)]
        public string SubmitButtonText { get; set; } = "Submit";

        [StringLength(1000)]
        public string? ConfirmationMessage { get; set; }

        [StringLength(500)]
        public string? RedirectUrl { get; set; }

        // Email Notification Settings
        public bool SendEmailNotification { get; set; }

        [StringLength(255)]
        [EmailAddress(ErrorMessage = "آدرس ایمیل گیرنده نامعتبر است.")]
        public string? NotificationEmailRecipient { get; set; }
    }
}