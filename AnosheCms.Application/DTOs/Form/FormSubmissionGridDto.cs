// AnosheCms/Application/DTOs/Form/FormSubmissionGridDto.cs
// NEW FILE

using System.Collections.Generic;

namespace AnosheCms.Application.DTOs.Form
{
    // DTO اصلی برای نمایش گرید داده‌ها در پنل ادمین
    public class FormSubmissionGridDto
    {
        // (لیست ستون‌ها بر اساس فیلدهای فرم)
        public List<GridHeaderDto> Headers { get; set; } = new List<GridHeaderDto>();

        // (لیست ردیف‌ها، هر ردیف یک دیکشنری از نام فیلد و مقدار آن است)
        public List<Dictionary<string, string>> Rows { get; set; } = new List<Dictionary<string, string>>();
    }
}