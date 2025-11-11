// File: Api/Authorization/PermissionHandler.cs
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Api.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {

        // (کد قبلی شما از AI_PROJECT_LOG.md اصلاح شد)
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // ۱. بررسی SuperAdmin
            if (context.User.IsInRole("SuperAdmin"))
            {
                context.Succeed(requirement);
                // return Task.CompletedTask; // <-- *** این خط حذف شد ***
                // (این بازگشت زودهنگام باعث شکست سایر بررسی‌های پالیسی می‌شد)
            }

            // ۲. بررسی Claimها (برای سایر رول‌ها)
            // (این بررسی اکنون به درستی برای رول Admin که به کاربر اضافه می‌کنیم کار خواهد کرد)
            var hasPermission = context.User.Claims
                .Any(c => c.Type == "Permission" && c.Value == requirement.Permission);

            if (hasPermission)
            {
                context.Succeed(requirement);
            }

            // ۳. هندلر همیشه باید این را در انتها برگرداند
            return Task.CompletedTask;
        }
    }
}