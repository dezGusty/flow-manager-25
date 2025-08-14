using FlowManager.Application.DTOs.Requests.FormTemplate;
using FlowManager.Application.DTOs.Responses;
using FlowManager.Application.DTOs.Responses.FormTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Application.Interfaces
{
    public interface IFormTemplateService
    {
        Task<PagedResponseDto<FormTemplateResponseDto>> GetAllFormTemplatesQueriedAsync(QueriedFormTemplateRequestDto payload);
        Task<FormTemplateResponseDto?> GetFormTemplateByIdAsync(Guid id);
        Task<FormTemplateResponseDto> PostFormTemplateAsync(PostFormTemplateRequestDto payload);
        Task<FormTemplateResponseDto?> DeleteFormTemplateAsync(Guid id);
        Task<FormTemplateResponseDto?> PatchFormTemplateAsync(Guid id,PatchFormTemplateRequestDto payload);
    }
}
