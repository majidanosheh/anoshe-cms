using AnosheCms.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnosheCms.Domain.Entities
{
    public class FormField : AuditableBaseEntity, ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid FormId { get; set; }

        [ForeignKey("FormId")]
        public virtual Form Form { get; set; }

        [Required]
        [StringLength(200)]
        public string Label { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // Programmatic name

        [Required]
        [StringLength(50)]
        public string FieldType { get; set; } // e.g., "Text", "Email", "Checkbox"

        public bool IsRequired { get; set; }

        public int Order { get; set; }

        [StringLength(500)]
        public string? Placeholder { get; set; } // (From Frontend Doc)

        [StringLength(1000)]
        public string? HelpText { get; set; } // (From Frontend Doc)

        // (JSON: شامل rules, conditions, options)
        public string? ValidationRules { get; set; } 
        public string? ConditionalLogic { get; set; } 

        // (Settings برای Options مانند Dropdown استفاده خواهد شد)
        public string? Settings { get; set; } // JSON (e.g., Options for Radio/Dropdown)

        public bool IsDeleted { get; set; }

        public string? Options { get; set; } // برای ذخیره گزینه‌های Dropdown/Radio (با کاما یا خط جدید)
    }
}