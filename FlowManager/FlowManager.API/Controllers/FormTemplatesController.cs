using FlowManager.Application.DTOs.Requests.FormTemplate;
using FlowManager.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlowManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormTemplatesController : ControllerBase
    {
        private readonly IFormTemplateService _formTemplateService;
        private readonly ILogger<FormTemplatesController> _logger;

        public FormTemplatesController(IFormTemplateService formTemplateService, ILogger<FormTemplatesController> logger)
        {
            _formTemplateService = formTemplateService ?? throw new ArgumentNullException(nameof(formTemplateService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("queried")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFormTemplatesQueriedAsync([FromQuery] QueriedFormTemplateRequestDto payload)
        {
            var result = await _formTemplateService.GetAllFormTemplatesQueriedAsync(payload);

            return Ok(new
            {
                Result = result,
                Success = true,
                Message = "Form templates retrieved successfully.",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFormTemplateByIdAsync(Guid id)
        {
            var result = await _formTemplateService.GetFormTemplateByIdAsync(id);

            return Ok(new
            {
                Result = result,
                Success = true,
                Message = "Form templates retrieved successfully.",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostFormTemplateAsync([FromBody] PostFormTemplateRequestDto payload)
        {
            var result = await _formTemplateService.PostFormTemplateAsync(payload);

            return Created($"/api/formTemplates/{result.Id}",new
            {
                Result = result,
                Success = true,
                Message = "Form templates post successfully.",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchFormTemplateAsync(Guid id,[FromBody] PatchFormTemplateRequestDto payload)
        {
            var result = await _formTemplateService.PatchFormTemplateAsync(id, payload);

            return Ok(new
            {
                Result = result,
                Success = true,
                Message = "Form templates patched successfully.",
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchFormTemplateAsync(Guid id)
        {
            var result = await _formTemplateService.DeleteFormTemplateAsync(id);

            return Ok(new
            {
                Result = result,
                Success = true,
                Message = "Form templates deleted successfully.",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
