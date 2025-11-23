using AnosheCms.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnosheCms.Domain.Entities
{
    public class ContentItem : AuditableBaseEntity, ISoftDelete
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ContentTypeId { get; set; }

        [ForeignKey("ContentTypeId")]
        public virtual ContentType ContentType { get; set; }

        // (اصلاح شد: فیلد DataJson که ارور می‌داد اضافه شد)
        public string DataJson { get; set; } // JSON string storing dynamic data

        public string Status { get; set; } // Published, Draft, Archived

        public bool IsDeleted { get; set; }
    }
}