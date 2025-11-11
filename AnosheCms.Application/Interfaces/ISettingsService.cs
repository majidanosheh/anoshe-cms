// File: AnosheCms.Application/Interfaces/ISettingsService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AnosheCms.Application.Interfaces
{
    public interface ISettingsService
    {
        Task<(Dictionary<string, object>? Data, string? ErrorMessage)> GetSettingsAsync(string contentTypeSlug);
        Task<(Dictionary<string, object>? Data, string? ErrorMessage)> UpdateSettingsAsync(string contentTypeSlug, Dictionary<string, object> data, Guid userId);
    }
}