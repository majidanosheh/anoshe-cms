// File: AnosheCms.Application/DTOs/Admin/RoleDto.cs
using System;

namespace AnosheCms.Application.DTOs.Admin
{
    // DTO برای نمایش لیست نقش‌ها
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsSystemRole { get; set; }
    }
}