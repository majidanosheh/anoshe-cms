// File: AnosheCms.Application/DTOs/Auth/AuthenticationResult.cs
// FULL REWRITE

using System;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Auth
{
    public class AuthenticationResult
    {
        public bool Succeeded { get; set; }

        // (اصلاح شد: Token -> AccessToken)
        // این باعث می‌شود در JSON خروجی، نام فیلد "accessToken" شود که فرانت‌اند منتظر آن است.
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}