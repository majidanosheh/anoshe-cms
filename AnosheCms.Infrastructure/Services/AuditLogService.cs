using AnosheCms.Application.DTOs.Admin;
using AnosheCms.Application.DTOs.Common;
using AnosheCms.Application.Interfaces;
using AnosheCms.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;

        public AuditLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AuditLogDto>> GetLogsAsync(int page, int pageSize)
        {
            var query = _context.AuditLogs.AsNoTracking().OrderByDescending(a => a.Timestamp);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuditLogDto
                {
                    Id = a.Id,
                    // پیدا کردن نام کاربر از روی ID، اگر نبود "System"
                    UserName = a.UserId.HasValue
                        ? (_context.Users.Where(u => u.Id == a.UserId).Select(u => u.UserName).FirstOrDefault() ?? "Unknown")
                        : "System",
                    Action = a.Action,
                    EntityName = a.EntityName,
                    EntityId = a.EntityId,
                    Timestamp = a.Timestamp,
                    IpAddress = a.IpAddress,
                    OldValues = a.OldValues,
                    NewValues = a.NewValues
                })
                .ToListAsync();

            return new PagedResult<AuditLogDto>
            {
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}