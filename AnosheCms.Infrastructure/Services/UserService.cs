// مسیر: AnosheCms.Infrastructure/Services/UserService.cs

// (اصلاح‌شده - استفاده از Aliases برای رفع ابهام)
using UserDto = AnosheCms.Application.DTOs.User.UserDto;
using CreateUserRequest = AnosheCms.Application.DTOs.Admin.CreateUserRequest;
using UpdateUserRequest = AnosheCms.Application.DTOs.Admin.UpdateUserRequest;

using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // UserDto به ...DTOs.User.UserDto اشاره دارد
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .Select(u => new UserDto // بدون ابهام
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    IsActive = u.IsActive,
                    EmailConfirmed = u.EmailConfirmed
                })
                .ToListAsync();
        }

        // UserDto به ...DTOs.User.UserDto اشاره دارد
        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;

            return MapToUserDto(user, (await _userManager.GetRolesAsync(user)).ToList());
        }

        // CreateUserRequest به ...DTOs.Admin.CreateUserRequest اشاره دارد
        public async Task<(UserDto? user, string[]? Errors)> CreateUserAsync(CreateUserRequest request, Guid currentUserId)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = request.IsActive,
                EmailConfirmed = request.EmailConfirmed,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserId
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return (null, result.Errors.Select(e => e.Description).ToArray());
            }

            if (request.Roles != null && request.Roles.Any())
            {
                await _userManager.AddToRolesAsync(user, request.Roles);
            }

            return (MapToUserDto(user, request.Roles), null);
        }

        // UpdateUserRequest به ...DTOs.Admin.UpdateUserRequest اشاره دارد
        public async Task<(UserDto? user, string[]? Errors)> UpdateUserAsync(Guid id, UpdateUserRequest request, Guid currentUserId)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return (null, new[] { "User not found." });
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsActive = request.IsActive;
            user.EmailConfirmed = request.EmailConfirmed;
            user.LastModifiedBy = currentUserId;
            user.LastModifiedDate = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return (null, result.Errors.Select(e => e.Description).ToArray());
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = request.Roles.Except(currentRoles);
            var rolesToRemove = currentRoles.Except(request.Roles);

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return (MapToUserDto(user, request.Roles), null);
        }

        public async Task<bool> DeleteUserAsync(Guid id, Guid currentUserId)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            user.IsDeleted = true;
            user.DeletedDate = DateTime.UtcNow;
            user.DeletedBy = currentUserId;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        // --- Helper ---
        // UserDto به ...DTOs.User.UserDto اشاره دارد
        private UserDto MapToUserDto(ApplicationUser user, List<string> roles)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles ?? new List<string>()
            };
        }
    }
}