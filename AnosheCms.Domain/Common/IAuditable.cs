namespace AnosheCms.Domain.Common
{
    /// <summary>
    /// اینترفیسی برای تمام موجودیت‌هایی که نیازمند ردیابی (Auditing)
    /// و حذف منطقی (Soft Delete) هستند.
    /// </summary>
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
        string? LastModifiedBy { get; set; }
        bool IsDeleted { get; set; }
    }
}