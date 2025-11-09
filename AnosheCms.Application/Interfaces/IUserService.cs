// File: AnosheCms.Application/Interfaces/IUserService.cs
using AnosheCms.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid userId);

        // --- (متدهای جدید) ---
        Task<(UserDto User, IEnumerable<string> Errors)> CreateUserAsync(CreateUserRequest request, Guid createdByUserId);
        Task<(UserDto User, IEnumerable<string> Errors)> UpdateUserAsync(Guid userIdToUpdate, UpdateUserRequest request, Guid modifiedByUserId);
        Task<bool> DeleteUserAsync(Guid userIdToDelete, Guid deletedByUserId);
    }
}