// AnosheCms/Application/DTOs/Form/Rules/FieldValidationRules.cs
// NEW FILE

using System.Text.Json.Serialization;

namespace AnosheCms.Application.DTOs.Form.Rules
{
    // (این کلاس برای سادگی در لایه Application DTOs تعریف می‌شود)
    // (در معماری‌های پیچیده‌تر می‌توان آن را در Domain.ValueObjects قرار داد)
    public class FieldValidationRules
    {
        [JsonPropertyName("minLength")]
        public int? MinLength { get; set; }

        [JsonPropertyName("maxLength")]
        public int? MaxLength { get; set; }

        [JsonPropertyName("regexPattern")]
        public string? RegexPattern { get; set; }

        [JsonPropertyName("regexErrorMessage")]
        public string? RegexErrorMessage { get; set; }
    }
}