// File: AnosheCms.Application/DTOs/Admin/UserDto.cs
using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Admin
{
    // DTO برای نمایش لیست کاربران در پنل ادمین
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public List<string> Roles { get; set; }
    }
}