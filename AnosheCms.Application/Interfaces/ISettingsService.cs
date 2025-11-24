namespace AnosheCms.Application.Interfaces
{
    public interface ISettingsService
    {
        Task<Dictionary<string, string>> GetAllSettingsAsync();
        Task UpdateSettingsAsync(Dictionary<string, string> settings);
    }
}