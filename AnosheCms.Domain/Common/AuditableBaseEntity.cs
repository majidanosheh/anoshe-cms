// مسیر: AnosheCms.Domain/Common/AuditableBaseEntity.cs
using AnosheCms.Domain.Common;
using System;

namespace AnosheCms.Domain.Common
{
    // (اکنون از BaseEntity ارث‌بری می‌کند - که باید موجود باشد)
    public abstract class AuditableBaseEntity : BaseEntity, IAuditable, ISoftDelete
    {
        // (Id از BaseEntity می‌آید)

        // --- IAuditable ---
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }

        // --- ISoftDelete ---

        // (اصلاح کلیدی: افزودن مقدار پیش‌فرض = false)
        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}