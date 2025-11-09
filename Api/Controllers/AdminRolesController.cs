// File: Api/Controllers/AdminRolesController.cs
using AnosheCms.Application.DTOs.Admin;
using AnosheCms.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/roles")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminRolesController : ControllerBase
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminRolesController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
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
    }
}