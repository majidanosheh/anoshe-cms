// File: AnosheCms.Infrastructure/Services/NavigationService.cs
using AnosheCms.Application.DTOs.Navigation;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // <-- (جدید)
using AnosheCms.Domain.Constants;     // <-- (جدید)

namespace AnosheCms.Infrastructure.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService; // <-- (جدید)

        public NavigationService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context,
            IAuthorizationService authorizationService) // <-- (تزریق سرویس)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _context = context;
            _authorizationService = authorizationService; // <-- (جدید)
        }

        public async Task<List<MenuGroupDto>> GetAdminMenuAsync()
        {
            var userPrincipal = _httpContextAccessor.HttpContext?.User;
            if (userPrincipal == null)
            {
                return new List<MenuGroupDto>();
            }

            var menu = new List<MenuGroupDto>();
            var mainGroup = new MenuGroupDto { Title = "اصلی" };

            // (اصلاح شد: بررسی پالیسی داشبورد)
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewDashboard)).Succeeded)
            {
                mainGroup.Items.Add(new MenuItemDto { Title = "داشبورد", Icon = "mdi-av-timer", RouteName = "admin-dashboard" });
            }

            // (اصلاح شد: بررسی پالیسی رسانه)
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewMedia)).Succeeded)
            {
                mainGroup.Items.Add(new MenuItemDto { Title = "کتابخانه رسانه", Icon = "mdi-folder-multiple-image", RouteName = "admin-media" });
            }

            if (mainGroup.Items.Any())
            {
                menu.Add(mainGroup);
            }

            // (اصلاح شد: بررسی پالیسی محتوا)
            var contentGroup = new MenuGroupDto { Title = "محتوا" };
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewContent)).Succeeded)
            {
                var contentTypes = await _context.ContentTypes
                    .AsNoTracking()
                    .OrderBy(ct => ct.Name)
                    .Select(ct => new MenuItemDto
                    {
                        Title = ct.Name,
                        Icon = "mdi-file-document-box",
                        RouteName = "admin-content-list",
                        RouteParams = new Dictionary<string, string> { { "apiSlug", ct.ApiSlug } }
                    })
                    .ToListAsync();
                contentGroup.Items.AddRange(contentTypes);
            }

            if (contentGroup.Items.Any())
            {
                menu.Add(contentGroup);
            }

            var structureGroup = new MenuGroupDto { Title = "ساختار" };

            // (اصلاح شد: بررسی پالیسی انواع محتوا)
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewContentTypes)).Succeeded)
            {
                structureGroup.Items.Add(new MenuItemDto { Title = "انواع محتوا", Icon = "mdi-pencil-box-outline", RouteName = "admin-content-types" });
            }

            // (اصلاح شد: بررسی پالیسی تنظیمات)
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewSettings)).Succeeded)
            {
                structureGroup.Items.Add(new MenuItemDto { Title = "تنظیمات سراسری", Icon = "mdi-settings", RouteName = "admin-settings" });
            }

            if (structureGroup.Items.Any())
            {
                menu.Add(structureGroup);
            }

            var managementGroup = new MenuGroupDto { Title = "مدیریت" };

            // (اصلاح شد: بررسی پالیسی کاربران)
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewUsers)).Succeeded)
            {
                managementGroup.Items.Add(new MenuItemDto { Title = "مدیریت کاربران", Icon = "mdi-account-multiple", RouteName = "admin-users" });
            }

            // (اصلاح شد: بررسی پالیسی نقش‌ها)
            if ((await _authorizationService.AuthorizeAsync(userPrincipal, Permissions.ViewRoles)).Succeeded)
            {
                managementGroup.Items.Add(new MenuItemDto { Title = "نقش‌ها و دسترسی‌ها", Icon = "mdi-account-key", RouteName = "admin-roles" });
            }

            if (managementGroup.Items.Any())
            {
                menu.Add(managementGroup);
            }

            return menu;
        }
    }
}