using AnosheCms.Application.Interfaces;
using AnosheCms.Infrastructure.Persistence.Data;


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
            // فعلاً برای تست، دیتای ثابت برمی‌گردانیم تا سیستم بالا بیاید
            return new Dictionary<string, string>
            {
                { "SiteTitle", "Anoshe CMS" },
                { "AdminEmail", "admin@system.com" }
            };
        }

        public async Task UpdateSettingsAsync(Dictionary<string, string> settings)
        {
            // اینجا باید لاجیک ذخیره در دیتابیس نوشته شود
            await Task.CompletedTask;
        }
    }
}