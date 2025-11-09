// File: AnosheCms.Application/DTOs/Auth/AuthenticationResult.cs
using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Auth
{
    // (بر اساس 177.txt)
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}