// AnosheCms/Application/DTOs/Form/Rules/ConditionalLogicRule.cs
// NEW FILE

using System.Text.Json.Serialization;

namespace AnosheCms.Application.DTOs.Form.Rules
{
    // (این مدل، یک قانون واحد را تعریف می‌کند. مثال: "اگر field1 برابر 'Hello' بود")
    public class ConditionalLogicRule
    {
        [JsonPropertyName("field")]
        public string FieldName { get; set; }

        [JsonPropertyName("operator")]
        public string Operator { get; set; } // "Equals", "NotEquals"

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}