// AnosheCms/Api/Controllers/AdminFormController.cs
// FULL REWRITE

using AnosheCms.Application.DTOs.Form;
using AnosheCms.Application.Interfaces;
using AnosheCms.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/admin/forms")]

    [Authorize] 
    public class AdminFormController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ISubmissionService _submissionService;

        public AdminFormController(IFormService formService, ISubmissionService submissionService)
        {
            _formService = formService;
            _submissionService = submissionService;
        }

        // --- Forms ---

        [HttpGet]
        [Authorize(Policy = Permissions.ViewForms)]
        public async Task<IActionResult> GetAllForms()
        {
            var forms = await _formService.GetAllFormsAsync();
            return Ok(forms);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = Permissions.ViewForms)]
        public async Task<IActionResult> GetFormById(Guid id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            return form == null ? NotFound() : Ok(form);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.CreateForms)]
        public async Task<IActionResult> CreateForm([FromBody] FormCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var form = await _formService.CreateFormAsync(request);

            return form == null
                ? BadRequest(new { Message = "فرم با این ApiSlug (اسلاگ) تکراری است." })
                : CreatedAtAction(nameof(GetFormById), new { id = form.Id }, form);
        }

        [HttpPut("{id}/settings")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> UpdateGeneralSettings(Guid id, [FromBody] FormGeneralSettingsDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var form = await _formService.UpdateGeneralSettingsAsync(id, request);
            return form == null
                ? NotFound(new { Message = "فرم یافت نشد یا ApiSlug تکراری است." })
                : Ok(form);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteForms)]
        public async Task<IActionResult> DeleteForm(Guid id)
        {
            var result = await _formService.DeleteFormAsync(id);
            return result ? NoContent() : NotFound();
        }

        // --- Fields ---

        [HttpPost("{id}/fields")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> AddField(Guid id, [FromBody] FormFieldCreateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var field = await _formService.AddFieldToFormAsync(id, request);
            return field == null
                ? BadRequest(new { Message = "فرم یافت نشد یا نام فیلد (Name) تکراری است." })
                : Ok(field);
        }

        [HttpPut("fields/{fieldId}")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> UpdateField(Guid fieldId, [FromBody] FormFieldUpdateDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var field = await _formService.UpdateFieldAsync(fieldId, request);
            return field == null
                ? NotFound(new { Message = "فیلد یافت نشد یا نام فیلد (Name) تکراری است." })
                : Ok(field);
        }

        [HttpDelete("fields/{fieldId}")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> DeleteField(Guid fieldId)
        {
            var result = await _formService.DeleteFormFieldAsync(fieldId);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("fields/order")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> UpdateFieldOrders([FromBody] UpdateFieldOrdersRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _formService.UpdateFieldOrdersAsync(request);
            return result
                ? NoContent()
                : BadRequest(new { Message = "خطا در به‌روزرسانی ترتیب. شناسه‌ها را بررسی کنید." });
        }


        // --- Submissions ---

        [HttpGet("{id}/submissions")]
        [Authorize(Policy = Permissions.ViewSubmissions)]
        public async Task<IActionResult> GetSubmissionsGrid(Guid id)
        {
            var gridData = await _submissionService.GetSubmissionGridAsync(id);
            return Ok(gridData);
        }

        [HttpGet("{id}/export/csv")]
        [Authorize(Policy = Permissions.ViewSubmissions)]
        public async Task<IActionResult> ExportSubmissionsAsCsv(Guid id)
        {
            var form = await _formService.GetFormByIdAsync(id);
            if (form == null) return NotFound();

            var fileBytes = await _submissionService.ExportSubmissionsAsCsvAsync(id);

            string fileName = $"submissions_{form.ApiSlug}_{DateTime.UtcNow:yyyyMMdd}.csv";
            return File(fileBytes, "text/csv; charset=utf-8", fileName);
        }
    }
}