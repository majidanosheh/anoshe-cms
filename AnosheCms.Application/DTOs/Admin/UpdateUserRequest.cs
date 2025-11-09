// File: AnosheCms.Application/DTOs/Admin/UpdateUserRequest.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Admin
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // (رمز عبور در یک endpoint جداگانه تغییر خواهد کرد، نه در اینجا)

        [Required]
        public List<string> Roles { get; set; } = new List<string>();
    }
}