using AnosheCms.Domain.Common;
using Microsoft.AspNetCore.Identity;
using AnosheCms.Domain.Common; // اضافه شدن using

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// گسترش IdentityRole و پیاده‌سازی IAuditable
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>, IAuditable
    {
        public string? Description { get; set; }

        // پیاده‌سازی فیلدهای IAuditable
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}