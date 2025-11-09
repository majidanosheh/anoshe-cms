// File: AnosheCms.Domain/Common/AuditableBaseEntity.cs
using AnosheCms.Domain.Common;
using System;

namespace AnosheCms.Domain.Common
{
    public abstract class AuditableBaseEntity : IAuditable, ISoftDelete
    {
        public Guid Id { get; set; }

        // --- IAuditable ---
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }

        // --- ISoftDelete ---
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}