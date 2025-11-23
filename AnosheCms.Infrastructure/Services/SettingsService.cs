using AnosheCms.Application.Interfaces;
using AnosheCms.Infrastructure.Persistence.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Infrastructure.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ApplicationDbContext _context;

        public SettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, string>> GetAllSettingsAsync()
        {
            // فعلاً برای رفع ارور، یک دیکشنری ساده برمی‌گردانیم
            // (بعداً می‌توانیم به دیتابیس وصل کنیم)
            return new Dictionary<string, string>
            {
                { "SiteTitle", "Anoshe CMS" },
                { "AdminEmail", "admin@system.com" }
            };
        }

        public async Task UpdateSettingsAsync(Dictionary<string, string> settings)
        {
            // منطق ذخیره (فعلاً خالی برای رفع ارور)
            await Task.CompletedTask;
        }
    }
}