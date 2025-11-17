// AnosheCms/Application/DTOs/Form/Rules/FieldConditionalLogic.cs
// NEW FILE

using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Form.Rules
{
    // (این مدل، تنظیمات کامل ConditionalLogic برای یک فیلد است)
    public class FieldConditionalLogic
    {
        [JsonPropertyName("action")]
        public string Action { get; set; } // "Show", "Hide"

        [JsonPropertyName("condition")]
        public string Condition { get; set; } // "All", "Any" (برای چند شرطی)

        [JsonPropertyName("rules")]
        public List<ConditionalLogicRule> Rules { get; set; } = new List<ConditionalLogicRule>();
    }
}