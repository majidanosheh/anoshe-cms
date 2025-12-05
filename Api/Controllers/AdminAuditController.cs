using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using AnosheCms.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/audit-logs")]
    [Authorize(Policy = Permissions.ViewAuditLogs)] // فقط با دسترسی مخصوص
    public class AdminAuditController : ControllerBase
    {
        private readonly IAuditLogService _auditService;

        public AdminAuditController(IAuditLogService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _auditService.GetLogsAsync(page, pageSize);
            return Ok(result);
        }
    }
}