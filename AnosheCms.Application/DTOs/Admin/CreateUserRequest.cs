// File: AnosheCms.Application/DTOs/Admin/CreateUserRequest.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Admin
{
    public class CreateUserRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public List<string> Roles { get; set; } = new List<string>();
    }
}