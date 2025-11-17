// AnosheCms/Application/DTOs/Form/PublicFormSubmissionRequest.cs
// NEW FILE

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AnosheCms.Application.DTOs.Form
{
    public class PublicFormSubmissionRequest
    {
        [Required]
        public Dictionary<string, string> SubmissionData { get; set; }

        // (سازنده پیش‌فرض برای Model Binding)
        public PublicFormSubmissionRequest()
        {
            SubmissionData = new Dictionary<string, string>();
        }

        // (سازنده مورد استفاده در تست‌ها)
        public PublicFormSubmissionRequest(Dictionary<string, string> data)
        {
            SubmissionData = data;
        }
    }
}