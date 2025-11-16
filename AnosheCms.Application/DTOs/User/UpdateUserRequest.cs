using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.User
{
    // (این DTO توسط کنترلر شما استفاده می‌شود)
    public class UpdateUserRequest
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