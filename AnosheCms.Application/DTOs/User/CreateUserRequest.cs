using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.User
{
    // (این DTO توسط کنترلر شما استفاده می‌شود)
    public class CreateUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; } = true;
        public List<string> Roles { get; set; }
    }
}