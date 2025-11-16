// مسیر: Api/Controllers/AdminFormController.cs
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
    [Authorize] // (تمام متدهای این کنترلر نیاز به احراز هویت دارند)
    public class AdminFormController : ControllerBase
    {
        private readonly IFormService _formService;

        public AdminFormController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ViewForms)]
        public async Task<IActionResult> GetAllForms()
        {
            return Ok(await _formService.GetAllFormsAsync());
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
        public async Task<IActionResult> CreateForm([FromBody] CreateFormRequest request)
        {
            var form = await _formService.CreateFormAsync(request);
            return form == null
                ? BadRequest(new { Message = "Form with this ApiSlug already exists." })
                : CreatedAtAction(nameof(GetFormById), new { id = form.Id }, form);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> UpdateForm(Guid id, [FromBody] CreateFormRequest request)
        {
            var form = await _formService.UpdateFormAsync(id, request);
            return form == null ? NotFound() : Ok(form);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.DeleteForms)]
        public async Task<IActionResult> DeleteForm(Guid id)
        {
            return await _formService.DeleteFormAsync(id) ? NoContent() : NotFound();
        }

        // --- Fields ---

        [HttpPost("{id}/fields")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> AddField(Guid id, [FromBody] CreateFormFieldRequest request)
        {
            var field = await _formService.AddFieldToFormAsync(id, request);
            return field == null ? NotFound(new { Message = "Form not found." }) : Ok(field);
        }

        [HttpDelete("fields/{fieldId}")]
        [Authorize(Policy = Permissions.EditForms)]
        public async Task<IActionResult> DeleteField(Guid fieldId)
        {
            return await _formService.DeleteFormFieldAsync(fieldId) ? NoContent() : NotFound();
        }

        // --- Submissions ---

        [HttpGet("{id}/submissions")]
        [Authorize(Policy = Permissions.ViewSubmissions)]
        public async Task<IActionResult> GetSubmissions(Guid id)
        {
            return Ok(await _formService.GetFormSubmissionsAsync(id));
        }
    }
}