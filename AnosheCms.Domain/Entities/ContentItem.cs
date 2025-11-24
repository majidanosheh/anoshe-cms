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

        public string DataJson { get; set; } 

        public string Status { get; set; } 

        public bool IsDeleted { get; set; }
        public object ContentData { get; set; }
    }
}