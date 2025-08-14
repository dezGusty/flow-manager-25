using FlowManager.Application.DTOs.Requests.Component;
using FlowManager.Application.DTOs.Requests.FormTemplate;
using FlowManager.Application.DTOs.Responses;
using FlowManager.Application.DTOs.Responses.Component;
using FlowManager.Application.IServices;
using FlowManager.Domain.Dtos;
using FlowManager.Domain.Entities;
using FlowManager.Domain.IRepositories;
using FlowManager.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace FlowManager.Application.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _componentRepository;

        public ComponentService(IComponentRepository componentRepository)
        {
            _componentRepository = componentRepository;
        }

        public async Task<ComponentResponseDto?> DeleteComponentAsync(Guid id)
        {
            Component? component = await _componentRepository.GetComponentByIdAsync(id);

            if (component == null)
            {
                return null;
            }

            component.DeletedAt = DateTime.UtcNow;
            await _componentRepository.SaveChangesAsync();

            return new ComponentResponseDto
            {
                Id = component.Id,
                Type = component.Type,
                Label = component.Label,
                Required = component.Required,
                Properties = component.Properties,
                CreatedAt = component.CreatedAt,
                UpdatedAt = component.UpdatedAt,
                DeletedAt = component.DeletedAt
            };
        }

        public async Task<PagedResponseDto<ComponentResponseDto>> GetComponentsQueriedAsync(QueriedComponentRequestDto payload)
        {
            QueryParams? parameters = payload.QueryParams?.ToQueryParams();
            (List<Component> result,int totalCount) = await _componentRepository.GetAllComponentsQueriedAsync(payload.Type, payload.Label, parameters);
        
            return new PagedResponseDto<ComponentResponseDto>
            {
                Data = result.Select(c => new ComponentResponseDto
                {
                    Id = c.Id,
                    Type = c.Type,
                    Label = c.Label,
                    Required = c.Required,
                    Properties = c.Properties,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    DeletedAt = c.DeletedAt
                }).ToList(),
                TotalCount = totalCount,
                Page = payload.QueryParams?.Page ?? 1,
                PageSize = payload.QueryParams?.PageSize ?? totalCount,
            };
        }

        public async Task<ComponentResponseDto?> GetComponentByIdAsync(Guid id)
        {
            Component? result = await _componentRepository.GetComponentByIdAsync(id);

            if (result == null)
            {
                return null; // middleware
            }

            return new ComponentResponseDto
            {
                Id = result.Id,
                Type = result.Type,
                Label = result.Label,
                Required = result.Required,
                Properties = result.Properties,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                DeletedAt = result.DeletedAt
            };
        }

        public async Task<ComponentResponseDto?> PatchComponentAsync(Guid id,PatchComponentRequestDto payload)
        {
            Component? componentToPatch = _componentRepository.GetComponentByIdAsync(id).Result;

            if(componentToPatch == null)
            {
                return null; // middleware
            }

            PatchHelper.PatchFrom<PatchComponentRequestDto, Component>(componentToPatch, payload);    
            componentToPatch.UpdatedAt = DateTime.UtcNow;

            await _componentRepository.SaveChangesAsync();

            return new ComponentResponseDto
            {
                Id = componentToPatch.Id,
                Type = componentToPatch.Type,
                Label = componentToPatch.Label,
                Required = componentToPatch.Required,
                Properties = componentToPatch.Properties,
                CreatedAt = componentToPatch.CreatedAt,
                UpdatedAt = componentToPatch.UpdatedAt,
                DeletedAt = componentToPatch.DeletedAt
            };
        }

        public async Task<ComponentResponseDto> PostComponentAsync(PostComponentRequestDto payload)
        {
            Component component = new Component
            {
                Type = payload.Type,
                Label = payload.Label,
                Required = payload.Required,
                Properties = payload.Properties,
            };

            await _componentRepository.AddAsync(component);

            return new ComponentResponseDto
            {
                Id = component.Id,
                Type = component.Type,
                Label = component.Label,
                Required = component.Required,
                Properties = component.Properties,
                CreatedAt = component.CreatedAt,
                UpdatedAt = component.UpdatedAt,
                DeletedAt = component.DeletedAt
            };
        }
    }
}
