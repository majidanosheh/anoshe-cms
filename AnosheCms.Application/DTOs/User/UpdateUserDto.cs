using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.User
{
    // (این DTO خطای 'UpdateUserDto could not be found' را رفع می‌کند)
    public class UpdateUserDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public List<string> Roles { get; set; }
    }
}