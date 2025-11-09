// File: AnosheCms.Domain/Entities/RefreshToken.cs
// (بر اساس 177.txt، بازنویسی‌شده برای Guid)
using System;

namespace AnosheCms.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } // (Guid)
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ExpiryDate { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        
        public virtual ApplicationUser User { get; set; }
    }
}