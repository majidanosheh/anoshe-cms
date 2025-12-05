using AnosheCms.Application.DTOs.Admin;
using AnosheCms.Application.DTOs.Common; // برای PagedResult
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface IAuditLogService
    {
        Task<PagedResult<AuditLogDto>> GetLogsAsync(int page, int pageSize);
    }
}