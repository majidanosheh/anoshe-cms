// مسیر: AnosheCms.Application/Interfaces/IUserService.cs

// (اصلاح‌شده - استفاده از Aliases برای رفع ابهام)
using UserDto = AnosheCms.Application.DTOs.User.UserDto;
using CreateUserRequest = AnosheCms.Application.DTOs.Admin.CreateUserRequest;
using UpdateUserRequest = AnosheCms.Application.DTOs.Admin.UpdateUserRequest;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IUserService
    {
        // این UserDto اکنون به ...DTOs.User.UserDto اشاره دارد
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(Guid id);

        // اینها اکنون به درستی به ...DTOs.Admin اشاره دارند
        Task<(UserDto? user, string[]? Errors)> CreateUserAsync(CreateUserRequest request, Guid currentUserId);
        Task<(UserDto? user, string[]? Errors)> UpdateUserAsync(Guid id, UpdateUserRequest request, Guid currentUserId);

        Task<bool> DeleteUserAsync(Guid id, Guid currentUserId);
    }
}