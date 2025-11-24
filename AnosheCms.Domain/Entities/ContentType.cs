using AnosheCms.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Domain.Entities
{
    public class ContentType : AuditableBaseEntity, ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string ApiSlug { get; set; }

        public string? Description { get; set; } // (فیلد جدید)

        public virtual ICollection<ContentField> Fields { get; set; }

        public bool IsDeleted { get; set; }
    }
}