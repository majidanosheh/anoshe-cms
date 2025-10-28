using AnosheCms.Domain.Common;
using Microsoft.AspNetCore.Identity;
using AnosheCms.Domain.Common; // اضافه شدن using

namespace AnosheCms.Domain.Entities
{
    /// <summary>
    /// گسترش IdentityUser و پیاده‌سازی IAuditable
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>, IAuditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }

        // پیاده‌سازی فیلدهای IAuditable
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}