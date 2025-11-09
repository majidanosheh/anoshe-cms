// File: AnosheCms.Application/Interfaces/INavigationService.cs
using AnosheCms.Application.DTOs.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface INavigationService
    {
        Task<List<MenuGroupDto>> GetAdminMenuAsync();
    }
}