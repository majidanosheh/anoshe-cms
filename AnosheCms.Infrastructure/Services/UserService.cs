// File: AnosheCms.Infrastructure/Services/UserService.cs
using AnosheCms.Application.DTOs.Admin;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Entities;
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

        // --- (متدهای Read - که قبلاً ناقص بودند) ---

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                userDtos.Add(await MapToUserDto(user));
            }
            return userDtos;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;

            return await MapToUserDto(user);
        }

        private async Task<UserDto> MapToUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                CreatedDate = user.CreatedDate,
                LastLoginDate = user.LastLoginDate,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };
        }

        // --- (متدهای CUD - از پاسخ قبلی) ---

        public async Task<(UserDto User, IEnumerable<string> Errors)> CreateUserAsync(CreateUserRequest request, Guid createdByUserId)
        {
            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IsActive = request.IsActive,
                EmailConfirmed = true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = createdByUserId
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return (null, result.Errors.Select(e => e.Description));
            }

            var roleResult = await _userManager.AddToRolesAsync(user, request.Roles);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return (null, roleResult.Errors.Select(e => e.Description));
            }

            return (await MapToUserDto(user), null);
        }

        public async Task<(UserDto User, IEnumerable<string> Errors)> UpdateUserAsync(Guid userIdToUpdate, UpdateUserRequest request, Guid modifiedByUserId)
        {
            var user = await _userManager.FindByIdAsync(userIdToUpdate.ToString());
            if (user == null)
            {
                return (null, new[] { "کاربر یافت نشد." });
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IsActive = request.IsActive;
            user.LastModifiedDate = DateTime.UtcNow;
            user.LastModifiedBy = modifiedByUserId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return (null, updateResult.Errors.Select(e => e.Description));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = currentRoles.Except(request.Roles).ToList();
            var rolesToAdd = request.Roles.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                    return (null, removeResult.Errors.Select(e => e.Description));
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                    return (null, addResult.Errors.Select(e => e.Description));
            }

            return (await MapToUserDto(user), null);
        }

        public async Task<bool> DeleteUserAsync(Guid userIdToDelete, Guid deletedByUserId)
        {
            var user = await _userManager.FindByIdAsync(userIdToDelete.ToString());
            if (user == null) return false;

            user.IsDeleted = true;
            user.IsActive = false;
            user.DeletedBy = deletedByUserId;
            user.DeletedDate = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}