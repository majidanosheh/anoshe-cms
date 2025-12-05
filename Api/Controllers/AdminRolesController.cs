// File: Api/Controllers/AdminRolesController.cs
using AnosheCms.Application.DTOs.Admin;
using AnosheCms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using AnosheCms.Domain.Constants; // <-- (Using
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/roles")]
    // [Authorize(Roles = "SuperAdmin")] // <-- حذف شد
    [Authorize] // <-- عمومی اعمال شد
    public class AdminRolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public AdminRolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ViewRoles)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    DisplayName = r.DisplayName,
                    IsSystemRole = r.IsSystemRole
                })
                .ToListAsync();
            return Ok(roles);
        }

        [HttpGet("permissions")]
        [Authorize(Policy = Permissions.ManagePermissions)] // <-- پالیسی اعمال شد
        public IActionResult GetAllPermissions()
        {
            var permissionGroups = GetAllPermissionGroupsInternal();
            return Ok(permissionGroups);
        }

        [HttpGet("{roleId}/permissions")]
        [Authorize(Policy = Permissions.ManagePermissions)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> GetRolePermissions(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return NotFound("Role not found.");
            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims.Where(c => c.Type == "Permission")
                                    .Select(c => c.Value)
                                    .ToList();
            return Ok(permissions);
        }

        [HttpPut("{roleId}/permissions")]
        [Authorize(Policy = Permissions.ManagePermissions)] // <-- پالیسی اعمال شد
        public async Task<IActionResult> UpdateRolePermissions(Guid roleId, [FromBody] UpdateRolePermissionsRequest request)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return NotFound("Role not found.");
            if (role.Name == "SuperAdmin")
            {
                return BadRequest(new { Message = "Cannot modify SuperAdmin permissions." });
            }
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            var permissionsToRemove = currentClaims.Where(c => c.Type == "Permission").ToList();
            foreach (var claim in permissionsToRemove)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            foreach (var permissionName in request.PermissionNames.Distinct())
            {
                if (!string.IsNullOrEmpty(permissionName))
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", permissionName));
                }
            }
            return Ok(new { Message = "Permissions updated successfully." });
        }
        [HttpPost]
        [Authorize(Policy = Permissions.CreateRoles)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto request)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
            {
                return BadRequest(new { Message = "این نقش قبلاً وجود دارد." });
            }

            var role = new ApplicationRole
            {
                Name = request.Name,
                DisplayName = request.DisplayName ?? request.Name,
                IsSystemRole = false // نقش‌های دستی
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "نقش جدید با موفقیت ایجاد شد." });
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteRoles)]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return NotFound();

            if (role.IsSystemRole)
            {
                return BadRequest(new { Message = "امکان حذف نقش‌های سیستمی وجود ندارد." });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "نقش با موفقیت حذف شد." });
        }
        private List<PermissionGroupDto> GetAllPermissionGroupsInternal()
        {
            return new List<PermissionGroupDto> {
                new PermissionGroupDto {
                    GroupName = "داشبورد", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewDashboard, Description = "مشاهده داشبورد" }
                    }
                },
                new PermissionGroupDto {
                    GroupName = "انواع محتوا", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewContentTypes, Description = "مشاهده لیست انواع محتوا" },
                        new PermissionDto { Name = Permissions.CreateContentTypes, Description = "ایجاد نوع محتوای جدید" },
                        new PermissionDto { Name = Permissions.EditContentTypes, Description = "ویرایش انواع محتوا و فیلدها" },
                        new PermissionDto { Name = Permissions.DeleteContentTypes, Description = "حذف انواع محتوا" }
                    }
                },
                new PermissionGroupDto {
                    GroupName = "محتوا (کلی)", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewContent, Description = "مشاهده لیست آیتم‌های محتوا" },
                        new PermissionDto { Name = Permissions.CreateContent, Description = "ایجاد آیتم محتوای جدید" },
                        new PermissionDto { Name = Permissions.EditContent, Description = "ویرایش آیتم‌های محتوا" },
                        new PermissionDto { Name = Permissions.DeleteContent, Description = "حذف آیتم‌های محتوا" }
                    }
                },
                new PermissionGroupDto {
                    GroupName = "رسانه", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewMedia, Description = "مشاهده کتابخانه رسانه" },
                        new PermissionDto { Name = Permissions.CreateMedia, Description = "آپلود فایل جدید" },
                        new PermissionDto { Name = Permissions.DeleteMedia, Description = "حذف فایل‌ها" }
                    }
                },
                new PermissionGroupDto {
                    GroupName = "تنظیمات", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewSettings, Description = "مشاهده تنظیمات سراسری" },
                        new PermissionDto { Name = Permissions.EditSettings, Description = "ویرایش تنظیمات سراسری" }
                    }
                },
                new PermissionGroupDto {
                    GroupName = "مدیریت کاربران", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewUsers, Description = "مشاهده لیست کاربران" },
                        new PermissionDto { Name = Permissions.CreateUsers, Description = "ایجاد کاربر جدید" },
                        new PermissionDto { Name = Permissions.EditUsers, Description = "ویرایش کاربران" },
                        new PermissionDto { Name = Permissions.DeleteUsers, Description = "حذف (غیرفعال کردن) کاربران" }
                    }
                },
                new PermissionGroupDto {
                    GroupName = "مدیریت نقش‌ها", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewRoles, Description = "مشاهده لیست نقش‌ها" },
                        new PermissionDto { Name = Permissions.CreateRoles, Description = "ایجاد نقش جدید" },
                        new PermissionDto { Name = Permissions.EditRoles, Description = "ویرایش نقش‌ها" },
                        new PermissionDto { Name = Permissions.DeleteRoles, Description = "حذف نقش‌ها" },
                        new PermissionDto { Name = Permissions.ManagePermissions, Description = "مدیریت دسترسی‌های نقش‌ها" }
                    }

                },
                new PermissionGroupDto {
                    GroupName = "فرم‌ساز", Permissions = new List<PermissionDto> {
                        new PermissionDto { Name = Permissions.ViewForms, Description = "مشاهده لیست فرم‌ها" },
                        new PermissionDto { Name = Permissions.CreateForms, Description = "ساخت فرم جدید" },
                        new PermissionDto { Name = Permissions.EditForms, Description = "ویرایش و طراحی فرم" },
                        new PermissionDto { Name = Permissions.DeleteForms, Description = "حذف فرم" },
                        new PermissionDto { Name = Permissions.ViewSubmissions, Description = "مشاهده ورودی‌ها" }
                    } 
                },
                new PermissionGroupDto {
                    GroupName = "نظارت و لاگ‌ها", Permissions = new List<PermissionDto> {
                        // (این ثابت را باید در Permissions.cs اضافه کنیم)
                        new PermissionDto { Name = "Permissions.Audit.View", Description = "مشاهده لاگ‌های سیستم" }
                    }
                }
            };
        }

        
    }
}