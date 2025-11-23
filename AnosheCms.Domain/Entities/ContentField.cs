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
        public string Name { get; set; } // نام سیستمی (مثلاً price)

        // (اصلاح شد: فیلدهای Label, Order, Options اضافه شدند)
        [Required]
        [StringLength(200)]
        public string Label { get; set; } // نام نمایشی (مثلاً قیمت محصول)

        [Required]
        public string FieldType { get; set; } // Text, Number, Image, ...

        public int Order { get; set; } // ترتیب نمایش

        public bool IsRequired { get; set; }

        public string? Options { get; set; } // JSON options (مثلاً گزینه‌های Dropdown)

        public bool IsDeleted { get; set; }
    }
}