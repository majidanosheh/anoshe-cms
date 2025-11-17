// AnosheCms/Infrastructure/Services/Helpers/ConditionalLogicEvaluator.cs
// NEW FILE

using AnosheCms.Application.DTOs.Form.Rules;
using AnosheCms.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AnosheCms.Infrastructure.Services.Helpers
{
    public static class ConditionalLogicEvaluator
    {
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        // (متد اصلی که وضعیت نهایی فیلد را برمی‌گرداند)
        public static bool IsFieldVisible(FormField field, IReadOnlyDictionary<string, string> submissionData)
        {
            if (string.IsNullOrWhiteSpace(field.ConditionalLogic))
            {
                return true; // (بدون منطق شرطی، همیشه قابل مشاهده است)
            }

            FieldConditionalLogic logic;
            try
            {
                logic = JsonSerializer.Deserialize<FieldConditionalLogic>(field.ConditionalLogic, _jsonOptions);
            }
            catch (JsonException)
            {
                return true; // (خطا در پارس JSON، به حالت پیش‌فرض امن (قابل مشاهده) برمی‌گردیم)
            }

            if (logic == null || !logic.Rules.Any())
            {
                return true; // (منطق شرطی خالی است)
            }

            bool rulesMet = CheckRules(logic, submissionData);

            // (بررسی منطق نهایی)
            if (logic.Action == "Show")
            {
                return rulesMet; // (نمایش بده اگر قوانین رعایت شدند)
            }
            if (logic.Action == "Hide")
            {
                return !rulesMet; // (مخفی کن اگر قوانین رعایت شدند -> پس نمایش بده اگر نشدند)
            }

            return true; // (پیش‌فرض)
        }

        private static bool CheckRules(FieldConditionalLogic logic, IReadOnlyDictionary<string, string> data)
        {
            var results = new List<bool>();

            foreach (var rule in logic.Rules)
            {
                data.TryGetValue(rule.FieldName, out var submittedValue);
                submittedValue ??= string.Empty; // (جلوگیری از Null)

                bool ruleResult = false;
                if (rule.Operator == "Equals")
                {
                    ruleResult = submittedValue.Equals(rule.Value, System.StringComparison.OrdinalIgnoreCase);
                }
                else if (rule.Operator == "NotEquals")
                {
                    ruleResult = !submittedValue.Equals(rule.Value, System.StringComparison.OrdinalIgnoreCase);
                }
                // (می‌توان اپراتورهای دیگر مانند Contains, StartsWith را اضافه کرد)

                results.Add(ruleResult);
            }

            if (logic.Condition == "All")
            {
                return results.All(res => res); // (همه باید true باشند)
            }
            else // (پیش‌فرض "Any")
            {
                return results.Any(res => res); // (حداقل یکی باید true باشد)
            }
        }
    }
}