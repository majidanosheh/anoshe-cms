// File: AnosheCms.Application/DTOs/Admin/PermissionDto.cs
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Admin
{
    public class PermissionDto
    {
        // (تصحیح شد: قابل-تهی)
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class PermissionGroupDto
    {
        // (تصحیح شد: قابل-تهی)
        public string? GroupName { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class UpdateRolePermissionsRequest
    {
        // (این لیست می‌تواند خالی باشد، اما خود لیست نباید null باشد)
        public List<string> PermissionNames { get; set; } = new List<string>();
    }
}