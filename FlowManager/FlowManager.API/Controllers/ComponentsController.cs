using FlowManager.Application.DTOs.Requests.Component;
using FlowManager.Application.DTOs.Responses;
using FlowManager.Application.DTOs.Responses.Component;
using FlowManager.Application.Interfaces;
using FlowManager.Application.IServices;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace FlowManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentsController> _logger;

        public ComponentsController(IComponentService componentService, ILogger<ComponentsController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        [HttpGet("queried")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComponentsQueriedAsync([FromQuery] QueriedComponentRequestDto payload)
        {
            PagedResponseDto<ComponentResponseDto> result = await _componentService.GetComponentsQueriedAsync(payload);

            return Ok(new
            {
                Result = result,
                Message = "Components retrieved successfully",
                Success = true,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetComponentByIdAsync(Guid id)
        {
            ComponentResponseDto? result = await _componentService.GetComponentByIdAsync(id);

            return Ok(new
            {
                Result = result,
                Message = "Component retrieved successfully",
                Success = true,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostComponentAsync([FromBody] PostComponentRequestDto payload)
        {
            ComponentResponseDto? result = await _componentService.PostComponentAsync(payload);

            return Created($"/api/components/{result.Id}", new
            {
                Result = result,
                Message = "Component created successfully",
                Success = true,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchComponentAsync(Guid id, [FromBody] PatchComponentRequestDto payload)
        {
            ComponentResponseDto? result = await _componentService.PatchComponentAsync(id,payload);

            return Ok(new
            {
                Result = result,
                Message = "Component patched successfully",
                Success = true,
                Timestamp = DateTime.UtcNow
            });
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComponentAsync(Guid id)
        {
            ComponentResponseDto? result = await _componentService.DeleteComponentAsync(id);

            return Ok(new
            {
                Result = result,
                Message = "Component deleted successfully",
                Success = true,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
