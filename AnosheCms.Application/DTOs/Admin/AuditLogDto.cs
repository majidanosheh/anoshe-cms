using System;

namespace AnosheCms.Application.DTOs.Admin
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }   // Create, Update, Delete
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }
}