// File: AnosheCms.Application/Interfaces/ICurrentUserService.cs
using System;

namespace AnosheCms.Application.Interfaces
{
    /// <summary>
    /// سرویسی برای دریافت اطلاعات کاربر لاگین کرده فعلی
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// شناسه (Guid) کاربر لاگین کرده
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// ایمیل کاربر لاگین کرده
        /// </summary>
        string? UserEmail { get; }
    }
}