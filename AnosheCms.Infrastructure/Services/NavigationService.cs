// AnosheCms/Infrastructure/Services/NavigationService.cs
// FULL REWRITE (نسخه استاندارد و تضمینی)

using AnosheCms.Application.DTOs.Navigation;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public NavigationService(
            ICurrentUserService currentUserService,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<MenuItemDto>> GetAdminMenuAsync()
        {
            // 1. اگر کاربر لاگین نیست، لیست خالی بده
            if (!_currentUserService.UserId.HasValue)
                return new List<MenuItemDto>();

            var userId = _currentUserService.UserId.Value;

            // 2. واکشی دسترسی‌ها (به روش امن)
            var perms = await GetUserPermissionsSafeAsync(userId);
            var menu = new List<MenuItemDto>();

            // 3. ساختار منو (چک کردن دسترسی‌ها)

            // --- داشبورد (همیشه هست) ---
            menu.Add(new MenuItemDto { Title = "داشبورد", Icon = "mdi mdi-av-timer", Path = "/admin/dashboard" });

            // --- مدیریت محتوا ---
            if (perms.Contains(Permissions.ManageContentTypes) || perms.Contains(Permissions.ManageContentEntries))
            {
                var node = new MenuItemDto { Title = "مدیریت محتوا", Icon = "mdi mdi-buffer" };

                if (perms.Contains(Permissions.ManageContentTypes))
                    node.Children.Add(new MenuItemDto { Title = "انواع محتوا", Icon = "mdi mdi-widgets", Path = "/admin/content-types" });

                if (perms.Contains(Permissions.ManageContentEntries))
                    node.Children.Add(new MenuItemDto { Title = "مدیریت آیتم‌ها", Icon = "mdi mdi-view-list", Path = "/admin/content-items" });

                if (node.Children.Any()) menu.Add(node);
            }

            // --- فرم‌ساز ---
            if (perms.Contains(Permissions.ViewForms))
            {
                var node = new MenuItemDto { Title = "فرم‌ساز", Icon = "mdi mdi-file-document-box" };

                node.Children.Add(new MenuItemDto { Title = "لیست فرم‌ها", Icon = "mdi mdi-format-list-bulleted", Path = "/admin/forms" });

                if (perms.Contains(Permissions.ManageForms))
                    node.Children.Add(new MenuItemDto { Title = "ایجاد فرم", Icon = "mdi mdi-plus-box", Path = "/admin/forms/create" });

                menu.Add(node);
            }

            // --- رسانه ---
            if (perms.Contains(Permissions.ManageMedia))
            {
                menu.Add(new MenuItemDto { Title = "رسانه", Icon = "mdi mdi-image-album", Path = "/admin/media" });
            }

            // --- کاربران ---
            if (perms.Contains(Permissions.ViewUsers) || perms.Contains(Permissions.ManageRoles))
            {
                var node = new MenuItemDto { Title = "کاربران و دسترسی", Icon = "mdi mdi-account-multiple" };

                if (perms.Contains(Permissions.ViewUsers))
                    node.Children.Add(new MenuItemDto { Title = "مدیریت کاربران", Icon = "mdi mdi-account-network", Path = "/admin/users" });

                if (perms.Contains(Permissions.ManageRoles))
                    node.Children.Add(new MenuItemDto { Title = "مدیریت نقش‌ها", Icon = "mdi mdi-account-key", Path = "/admin/roles" });

                if (node.Children.Any()) menu.Add(node);
            }

            // --- تنظیمات ---
            if (perms.Contains(Permissions.ManageSettings))
            {
                menu.Add(new MenuItemDto { Title = "تنظیمات", Icon = "mdi mdi-settings", Path = "/admin/settings" });
            }

            return menu;
        }

        // --- متد حیاتی: واکشی امن دسترسی‌ها ---
        private async Task<HashSet<string>> GetUserPermissionsSafeAsync(Guid userId)
        {
            var allPermissions = new HashSet<string>();

            // الف) پیدا کردن کاربر
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return allPermissions;

            // ب) گرفتن نام نقش‌های کاربر
            var roleNames = await _userManager.GetRolesAsync(user);

            // پ) حلقه روی نقش‌ها و گرفتن Claims (تضمینی)
            foreach (var roleName in roleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var claims = await _roleManager.GetClaimsAsync(role);
                    foreach (var claim in claims)
                    {
                        // ما فقط کلیم‌های نوع Permission را می‌خواهیم
                        if (claim.Type == "Permission")
                        {
                            allPermissions.Add(claim.Value);
                        }
                    }
                }
            }

            return allPermissions;
        }
    }
}