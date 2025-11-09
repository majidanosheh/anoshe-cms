// File: AnosheCms.Domain/Entities/UserLoginHistory.cs
// (بر اساس 177.txt، بازنویسی‌شده برای Guid)
using System;

namespace AnosheCms.Domain.Entities
{
    public class UserLoginHistory
    {
        public int Id { get; set; }
        public Guid UserId { get; set; } // (Guid)
        public DateTime LoginDate { get; set; } = DateTime.UtcNow;
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string Browser { get; set; }
        public string OS { get; set; }
        public string Device { get; set; }
        public bool IsSuccessful { get; set; }
        public string FailureReason { get; set; }
        
        public virtual ApplicationUser User { get; set; }
    }
}