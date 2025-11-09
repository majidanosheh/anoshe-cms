// File: AnosheCms.Domain/Entities/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using AnosheCms.Domain.Common; // (برای IAuditable و ISoftDelete)

namespace AnosheCms.Domain.Entities
{
    // (ما IAuditable و ISoftDelete را پیاده‌سازی می‌کنیم تا با DbContext سازگار باشیم)
    public class ApplicationUser : IdentityUser<Guid>, IAuditable, ISoftDelete
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // --- (تصحیح شد: string به string? تغییر کرد) ---
        public string? ProfilePictureUrl { get; set; }

        public bool IsActive { get; set; } = true;

        // Timestamps (IAuditable)
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public Guid? LastModifiedBy { get; set; }

        // Soft Delete (ISoftDelete)
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedDate { get; set; }
        public Guid? DeletedBy { get; set; }

        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }

        // --- (تصحیح شد: string به string? تغییر کرد) ---
        public string? TwoFactorSecretKey { get; set; }

        // Navigation Properties
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public virtual ICollection<UserLoginHistory> LoginHistories { get; set; }
        public virtual ICollection<UserSession> Sessions { get; set; }
    }
}