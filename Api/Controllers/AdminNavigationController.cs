// AnosheCms/Api/Controllers/AdminNavigationController.cs
// FULL REWRITE

using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [Route("api/admin/navigation")]
    [ApiController]
    [Authorize]
    public class AdminNavigationController : ControllerBase
    {
        private readonly INavigationService _navigationService;

        public AdminNavigationController(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu()
        {
            var menuItems = await _navigationService.GetAdminMenuAsync();
            return Ok(menuItems);
        }

        ////// (متد تست برای اطمینان خاطر - بعداً می‌توانید حذف کنید)
        ////[HttpGet("test")]
        ////[AllowAnonymous]
        ////public async Task<IActionResult> TestMenu()
        ////{
        ////    // نکته: در حالت Anonymous، سرویس منوی خالی برمی‌گرداند چون کاربر ندارد.
        ////    // این متد فقط برای چک کردن زنده بودن کنترلر است.
        ////    return Ok(new { Status = "Controller is Alive" });
        ////}
    }
}