using FlowManager.Application.DTOs.Requests.FormTemplate;
using FlowManager.Application.DTOs.Responses;
using FlowManager.Application.DTOs.Responses.FormTemplate;
using FlowManager.Application.DTOs.Responses.FormTemplateComponent;
using FlowManager.Application.Interfaces;
using FlowManager.Domain.Dtos;
using FlowManager.Domain.Entities;
using FlowManager.Domain.IRepositories;
using FlowManager.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Infrastructure.Services
{
    public class FormTemplateService : IFormTemplateService
    {
        private readonly IFormTemplateRepository _formTemplateRepository;
        
        public FormTemplateService(IFormTemplateRepository formTemplateRepository)
        {
            _formTemplateRepository = formTemplateRepository;
        }

        public async Task<FormTemplateResponseDto?> DeleteFormTemplateAsync(Guid id)
        {
            FormTemplate? formTemplateToDelete = await _formTemplateRepository.GetFormTemplateByIdAsync(id);

            if(formTemplateToDelete == null)
            {
                return null; // middleware exception later
            }
            
            formTemplateToDelete.DeletedAt = DateTime.UtcNow;
            await _formTemplateRepository.SaveChangesAsync();

            return new FormTemplateResponseDto
            {
                Id = formTemplateToDelete.Id,
                Name = formTemplateToDelete.Name,
                Content = formTemplateToDelete.Content,
                CreatedAt = formTemplateToDelete.CreatedAt,
                UpdatedAt = formTemplateToDelete.UpdatedAt,
                DeletedAt = formTemplateToDelete.DeletedAt
            };
        }

        public async Task<PagedResponseDto<FormTemplateResponseDto>> GetAllFormTemplatesQueriedAsync(QueriedFormTemplateRequestDto payload)
        {
            QueryParams? parameters = payload.QueryParams?.ToQueryParams();
            (List<FormTemplate> data, int totalCount) = await _formTemplateRepository.GetAllFormTemplatesQueriedAsync(payload.Name, parameters);

            return new PagedResponseDto<FormTemplateResponseDto>
            {
                Data = data.Select(ft => new FormTemplateResponseDto
                {
                    Id = ft.Id,
                    Name = ft.Name,
                    Content = ft.Content,
                    Components = ft.Components.Where(ftc => ftc.DeletedAt == null).Select(ftc => new FormTemplateComponentResponseDto
                    {
                        Id = ftc.ComponentId,
                    }).ToList(),
                    CreatedAt = ft.CreatedAt,
                    UpdatedAt = ft.UpdatedAt,
                    DeletedAt = ft.DeletedAt
                }).ToList(),
                TotalCount = totalCount,
                Page = payload.QueryParams?.Page ?? 1,
                PageSize = payload.QueryParams?.PageSize ?? totalCount,
            };
        }
        

        public async Task<FormTemplateResponseDto?> GetFormTemplateByIdAsync(Guid id)
        {
            FormTemplate? formTemplate = await _formTemplateRepository.GetFormTemplateByIdAsync(id);

            if (formTemplate == null)
            {
                return null; // middleware exception later
            }

            return new FormTemplateResponseDto
            {
                Id = formTemplate.Id,
                Name = formTemplate.Name,
                Content = formTemplate.Content,
                Components = formTemplate.Components.Where(ftc => ftc.DeletedAt == null).Select(ftc => new FormTemplateComponentResponseDto
                {
                    Id = ftc.ComponentId,
                }).ToList(),
                CreatedAt = formTemplate.CreatedAt,
                UpdatedAt = formTemplate.UpdatedAt,
                DeletedAt = formTemplate.DeletedAt
            };
        }

        public async Task<FormTemplateResponseDto?> PatchFormTemplateAsync(Guid id, PatchFormTemplateRequestDto payload)
        {
            var formTemplateToPatch = await _formTemplateRepository.GetFormTemplateByIdAsync(id);

            if (formTemplateToPatch == null)
            {
                return null; // middleware exception later
            }

            if(payload.Name != null && !string.IsNullOrEmpty(payload.Name))
                formTemplateToPatch.Name = payload.Name;

            if(payload.Content != null && !string.IsNullOrEmpty(payload.Content))
                formTemplateToPatch.Content = payload.Content;

            if(payload.Components != null)
            {
                foreach(FormTemplateComponent component in formTemplateToPatch.Components)
                {
                    component.DeletedAt = DateTime.UtcNow;
                }

                foreach(Guid componentId in payload.Components)
                {
                    if(formTemplateToPatch.Components.Any(c => c.ComponentId == componentId))
                    {
                        FormTemplateComponent? existingComponent = await _formTemplateRepository.GetFormTemplateComponentByIdAsync(componentId, includeDeleted: true);
                        existingComponent.DeletedAt = null;
                    }
                    else
                    {
                        FormTemplateComponent newComponent = new FormTemplateComponent
                        {
                            ComponentId = componentId,
                            FormTemplateId = formTemplateToPatch.Id
                        };
                        formTemplateToPatch.Components.Add(newComponent);
                    }
                }

                formTemplateToPatch.Components = payload.Components.Select(c => new FormTemplateComponent
                {
                    ComponentId = c,
                    FormTemplateId = formTemplateToPatch.Id
                }).ToList();
            }

            formTemplateToPatch.UpdatedAt = DateTime.UtcNow;

            await _formTemplateRepository.SaveChangesAsync();

            return new FormTemplateResponseDto
            {
                Id = formTemplateToPatch.Id,
                Name = formTemplateToPatch.Name,
                Content = formTemplateToPatch.Content,
                Components = formTemplateToPatch.Components.Where(ftc => ftc.DeletedAt == null).Select(ftc => new FormTemplateComponentResponseDto
                {
                    Id = ftc.ComponentId,
                }).ToList(),
                CreatedAt = formTemplateToPatch.CreatedAt,
                UpdatedAt = formTemplateToPatch.UpdatedAt,
                DeletedAt = formTemplateToPatch.DeletedAt
            };
        }

        public async Task<FormTemplateResponseDto> PostFormTemplateAsync(PostFormTemplateRequestDto payload)
        {
            if(await _formTemplateRepository.GetFormTemplateByNameAsync(payload.Name) != null)
            {
                return null; // middleware exception later
            }

            FormTemplate newFormTemplate = new FormTemplate
            {
                Name = payload.Name,
                Content = payload.Content,
                CreatedAt = DateTime.UtcNow,
            };

            newFormTemplate.Components = payload.Components.Select(c => new FormTemplateComponent
            {
                ComponentId = c,
                FormTemplateId = newFormTemplate.Id 
            }).ToList();

            await _formTemplateRepository.AddAsync(newFormTemplate);

            return new FormTemplateResponseDto
            {
                Id = newFormTemplate.Id,
                Name = newFormTemplate.Name,
                Content = newFormTemplate.Content,
                Components = newFormTemplate.Components.Where(ftc => ftc.DeletedAt == null).Select(ftc => new FormTemplateComponentResponseDto
                {
                    Id = ftc.Id
                }).ToList(),
                CreatedAt = newFormTemplate.CreatedAt,
                UpdatedAt = newFormTemplate.UpdatedAt,
                DeletedAt = newFormTemplate.DeletedAt
            };
        }
    }
}
