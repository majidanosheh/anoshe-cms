using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface ISettingsService
    {
        // حذف Tupleهای قدیمی و استفاده از متدهای ساده
        Task<Dictionary<string, string>> GetAllSettingsAsync();
        Task UpdateSettingsAsync(Dictionary<string, string> settings);
    }
}