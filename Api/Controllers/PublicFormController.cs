
using AnosheCms.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AnosheCms.Api.Controllers
{
    [ApiController]
    [Route("api/public/forms")]
    [AllowAnonymous]
    public class PublicFormController : ControllerBase
    {
        private readonly IFormService _formService;
        private readonly ISubmissionService _submissionService;
        private readonly ICurrentUserService _currentUserService;

        public PublicFormController(IFormService formService, ISubmissionService submissionService, ICurrentUserService currentUserService)
        {
            _formService = formService;
            _submissionService = submissionService;
            _currentUserService = currentUserService;
        }

        [HttpGet("{apiSlug}")]
        public async Task<IActionResult> GetPublicForm(string apiSlug)
        {
            var form = await _formService.GetFormBySlugAsync(apiSlug);
            return form == null ? NotFound() : Ok(form);
        }

        [HttpPost("{apiSlug}/submit")]
        public async Task<IActionResult> SubmitForm(string apiSlug, [FromBody] PublicFormSubmissionRequest request)
        {
            var result = await _submissionService.SubmitFormAsync(
                apiSlug,
                request,
                _currentUserService.RemoteIpAddress,
                _currentUserService.UserAgent
            );

            if (!result.Succeeded)
            {
                if (result.ValidationErrors != null)
                {
                    return BadRequest(new { Message = result.Message, Errors = result.ValidationErrors });
                }
                return BadRequest(new { Message = result.Message });
            }

            return Ok(new { Message = result.Message });
        }
    }
}