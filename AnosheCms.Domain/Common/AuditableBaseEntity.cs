namespace AnosheCms.Domain.Common
{
    /// <summary>
    /// کلاس پایه برای تمام موجودیت‌های غیر Identity که Auditable هستند.
    /// </summary>
    public abstract class AuditableBaseEntity : IAuditable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}