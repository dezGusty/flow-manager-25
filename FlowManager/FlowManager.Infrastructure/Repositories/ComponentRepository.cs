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
    public class ComponentRepository : IComponentRepository
    {
        private readonly AppDbContext _context;

        public ComponentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Component formTemplate)
        {
            _context.Components.Add(formTemplate);
            await SaveChangesAsync();
        }

        public async Task<(List<Component> Data, int TotalCount)> GetAllComponentsQueriedAsync(string? type, string? label, QueryParams parameters)
        {
            IQueryable<Component> query = _context.Components.Where(c => c.DeletedAt == null);

            // filtering
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(c => c.Type.ToUpper().Contains(type.ToUpper()));
            }

            if(!string.IsNullOrEmpty(label))
            {
                query = query.Where(c => c.Label.ToUpper().Contains(label.ToUpper()));
            }

            int totalCount = query.Count();

            if (parameters == null)
            {
                return (await query.ToListAsync(), totalCount);
            }

            // sorting
            if (parameters.SortBy != null)
            {
                if (parameters.SortDescending is bool SortDesc)
                    query = query.ApplySorting<Component>(parameters.SortBy, SortDesc);
                else
                    query = query.ApplySorting<Component>(parameters.SortBy, false);
            }

            // pagination
            if (parameters.Page == null || parameters.Page < 0 || parameters.PageSize == null || parameters.PageSize < 0)
            {
                return (await query.ToListAsync(), totalCount);
            }
            else
            {
                List<Component> data = await query.Skip((int)parameters.PageSize * ((int)parameters.Page - 1))
                                                     .Take((int)parameters.PageSize)
                                                     .ToListAsync();
                return (data, totalCount);
            }
        }

        public async Task<Component?> GetComponentByIdAsync(Guid id)
        {
            return await _context.Components
                            .Where(c => c.DeletedAt == null)
                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
