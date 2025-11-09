// File: AnosheCms.Domain/Entities/ApplicationUserRole.cs
// (بر اساس 177.txt، بازنویسی‌شده برای Guid)
using Microsoft.AspNetCore.Identity;
using System;

namespace AnosheCms.Domain.Entities
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public Guid? AssignedBy { get; set; }
        
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}