// AnosheCms/Application/DTOs/Form/FormFieldDto.cs
// NEW FILE

using System;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormFieldDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string FieldType { get; set; }
        public bool IsRequired { get; set; }
        public int Order { get; set; }

        // (فیلدهای جدید بر اساس سند فرانت‌اند)
        public string? Placeholder { get; set; }
        public string? HelpText { get; set; }

        // (JSON شامل ValidationRules, ConditionalLogic, Options)
        public string? Settings { get; set; }
        public string? ValidationRules { get; set; }
        public string? ConditionalLogic { get;set; }
    }
}