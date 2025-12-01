using AnosheCms.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnosheCms.Domain.Entities
{
    public class ContentField : BaseEntity, ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ContentTypeId { get; set; }

        [ForeignKey("ContentTypeId")]
        public virtual ContentType ContentType { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // (فیلدهای جدید که ارور می‌دادند)
        [Required]
        [StringLength(200)]
        public string Label { get; set; }

        [Required]
        public string FieldType { get; set; }

        public int Order { get; set; }

        public bool IsRequired { get; set; }

        public string? Options { get; set; }

        // (پیاده‌سازی ISoftDelete)
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        [StringLength(100)]
        public string ApiSlug { get; set; }


    }
}

