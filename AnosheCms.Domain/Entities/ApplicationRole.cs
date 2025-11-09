// File: AnosheCms.Domain/Entities/ApplicationRole.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using AnosheCms.Domain.Common;

namespace AnosheCms.Domain.Entities
{
    public class ApplicationRole : IdentityRole<Guid>, ISoftDelete, IAuditable // (اطمینان از پیاده‌سازی اینترفیس‌ها)
    {
        // --- (تصحیح شد: string به string? تغییر کرد) ---
        public string? Description { get; set; }

        public string DisplayName { get; set; }
        public bool IsSystemRole { get; set; }

        // --- IAuditable ---
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }

        // --- ISoftDelete ---
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        // Navigation Properties
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}