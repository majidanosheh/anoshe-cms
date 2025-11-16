using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.User
{
    // (این DTO کامل، خطای 'does not contain UserName' را رفع می‌کند)
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } // (مورد نیاز خطای 204.txt)
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}