// File: Api/Controllers/AdminNavigationController.cs
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/navigation")]
    [Authorize] // (کاربر باید لاگین باشد)
    public class AdminNavigationController : ControllerBase
    {
        private readonly INavigationService _navigationService;

        public AdminNavigationController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // GET: /api/admin/navigation/menu
        [HttpGet("menu")]
        public async Task<IActionResult> GetAdminMenu()
        {
            var menu = await _navigationService.GetAdminMenuAsync();
            return Ok(menu);
        }
    }
}