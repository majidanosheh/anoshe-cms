// File: AnosheCms.Domain/Entities/UserSession.cs
// (بر اساس 177.txt، بازنویسی‌شده برای Guid)
using System;

namespace AnosheCms.Domain.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } // (Guid)
        public string SessionId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public DateTime? LastActivityAt { get; set; }
        public bool IsActive { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        
        public virtual ApplicationUser User { get; set; }
    }
}