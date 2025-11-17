// AnosheCms/Domain/Entities/Form.cs
// FULL REWRITE

using AnosheCms.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Domain.Entities
{
    public class Form : AuditableBaseEntity, ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string ApiSlug { get; set; }

        
        [StringLength(100)]
        public string SubmitButtonText { get; set; } = "Submit";

        [StringLength(1000)]
        public string? ConfirmationMessage { get; set; }

        [StringLength(500)]
        public string? RedirectUrl { get; set; }

        public bool SendEmailNotification { get; set; }

        [StringLength(255)]
        public string? NotificationEmailRecipient { get; set; }

        // DateTime ModifiedBy { get; set; }
        // ----------------------------------------


        public string? Settings { get; set; }

        public virtual ICollection<FormField> Fields { get; set; } = new List<FormField>();
        public virtual ICollection<FormSubmission> Submissions { get; set; } = new List<FormSubmission>();

        public bool IsDeleted { get; set; }
    }
}