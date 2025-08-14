using FlowManager.Application.DTOs.Requests.Component;
using FlowManager.Application.DTOs.Requests.FormTemplate;
using FlowManager.Application.DTOs.Responses;
using FlowManager.Application.DTOs.Responses.Component;
using FlowManager.Application.DTOs.Responses.FormTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Application.IServices
{
    public interface IComponentService
    {
        Task<PagedResponseDto<ComponentResponseDto>> GetComponentsQueriedAsync(QueriedComponentRequestDto payload);
        Task<ComponentResponseDto?> GetComponentByIdAsync(Guid id);
        Task<ComponentResponseDto> PostComponentAsync(PostComponentRequestDto payload);
        Task<ComponentResponseDto?> DeleteComponentAsync(Guid id);
        Task<ComponentResponseDto?> PatchComponentAsync(Guid id, PatchComponentRequestDto payload);
    }
}
