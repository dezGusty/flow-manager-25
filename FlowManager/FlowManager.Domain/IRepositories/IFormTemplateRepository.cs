using FlowManager.Domain.Dtos;
using FlowManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Domain.IRepositories
{
    public interface IFormTemplateRepository
    {
        Task<(List<FormTemplate> Data, int TotalCount)> GetAllFormTemplatesQueriedAsync(string? name, QueryParams parameters);
        Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id);
        Task<FormTemplate?> GetFormTemplateByNameAsync(string email);
        Task SaveChangesAsync();
        Task AddAsync(FormTemplate formTemplate);
        Task<FormTemplateComponent?> GetFormTemplateComponentByIdAsync(Guid id, bool includeDeleted = false);
    }
}
