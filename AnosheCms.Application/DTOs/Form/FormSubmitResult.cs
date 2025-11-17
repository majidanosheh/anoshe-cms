// AnosheCms/Application/DTOs/Form/FormSubmitResult.cs
// FULL REWRITE

using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Form
{
    public class FormSubmitResult
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; } // (فیلد مورد نیاز خطا)
        public Dictionary<string, string> ValidationErrors { get; private set; }

        private FormSubmitResult(bool succeeded, string message, Dictionary<string, string> errors = null)
        {
            Succeeded = succeeded;
            Message = message;
            ValidationErrors = errors;
        }

        public static FormSubmitResult Success(string message = "اطلاعات با موفقیت ثبت شد.")
        {
            return new FormSubmitResult(true, message);
        }

        public static FormSubmitResult Failure(Dictionary<string, string> errors, string message = "خطا در اعتبارسنجی داده‌ها.")
        {
            return new FormSubmitResult(false, message, errors);
        }
    }
}