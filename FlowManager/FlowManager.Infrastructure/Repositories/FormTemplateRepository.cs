using FlowManager.Application.DTOs.Responses;
using FlowManager.Application.DTOs.Responses.FormTemplate;
using FlowManager.Application.DTOs.Responses.FormTemplateComponent;
using FlowManager.Domain.Dtos;
using FlowManager.Domain.Entities;
using FlowManager.Domain.IRepositories;
using FlowManager.Infrastructure.Context;
using FlowManager.Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowManager.Infrastructure.Repositories
{
    public class FormTemplateRepository : IFormTemplateRepository
    {
        private readonly AppDbContext _context;

        public FormTemplateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(List<FormTemplate> Data, int TotalCount)> GetAllFormTemplatesQueriedAsync(string? name, QueryParams parameters)
        {
            IQueryable<FormTemplate> query = _context.FormTemplates
                                                       .Where(ft => ft.DeletedAt == null)
                                                       .Include(ft => ft.Components);

            if (name != null)
            {
                query = query.Where(ft => ft.Name.ToUpper().Contains(name.ToUpper()));
            }

            int totalCount = await query.CountAsync();

            if (parameters == null)
            {
                var data = await query.ToListAsync();

                return (data,totalCount);
            }

            if (parameters.SortBy != null)
            {
                if (parameters.SortDescending is bool SortDesc)
                    query = query.ApplySorting<FormTemplate>(parameters.SortBy, SortDesc);
                else
                    query = query.ApplySorting<FormTemplate>(parameters.SortBy, false);
            }

            if (parameters.Page == null || parameters.Page < 0 ||
                parameters.PageSize == null || parameters.PageSize < 0)
            {
                List<FormTemplate> data = await query.ToListAsync();
                return (data, totalCount);
            }
            else
            {
                List<FormTemplate> data = await query.Skip((int)parameters.PageSize * ((int)parameters.Page - 1))
                                                     .Take((int)parameters.PageSize)
                                                     .ToListAsync();
                return (data, totalCount);
            }
        }

        public async Task<FormTemplate?> GetFormTemplateByNameAsync(string name)
        {
            return await _context.FormTemplates
                .Where(ft => ft.DeletedAt == null)
                .Include(ft => ft.Components)
                .FirstOrDefaultAsync(ft => ft.Name == name);
        }

        public async Task<FormTemplate?> GetFormTemplateByIdAsync(Guid id)
        {
            return await _context.FormTemplates
                .Where(ft => ft.DeletedAt == null)
                .Include(ft => ft.Components)
                .FirstOrDefaultAsync(ft => ft.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(FormTemplate formTemplate)
        {
            _context.FormTemplates.Add(formTemplate);
            await SaveChangesAsync();
        }

        public async Task<FormTemplateComponent?> GetFormTemplateComponentByIdAsync(Guid id, bool includeDeleted = false)
        {
            IQueryable<FormTemplateComponent> query = _context.FormTemplateComponents;
            if (!includeDeleted)
                query = query.Where(ftc => ftc.DeletedAt == null);

            return await query.FirstOrDefaultAsync(ftc => ftc.Id == id);
        }
    }
}
