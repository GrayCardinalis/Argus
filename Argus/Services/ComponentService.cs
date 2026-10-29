using Argus.Data;
using AutoMapper;
using Argus.Models;
using Argus.Dtos.Components;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Argus.Services.Interfaces;

namespace Argus.Services
{
    public class ComponentService(AppDbContext context, IMapper mapper) : IComponentService
    {
        public async Task<List<ComponentDto>> GetAllComponentAsync(CancellationToken ct = default)
        {
            return await context.Components
                .AsNoTracking()
                .ProjectTo<ComponentDto>(mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<ComponentDto?> GetComponentByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await context.Components
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ComponentDto?>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }



        public async Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto, CancellationToken ct = default)
        {
            var newComponent = mapper.Map<Component>(createComponentDto);

            context.Components.Add(newComponent);

            await context.SaveChangesAsync(ct);

            return mapper.Map<ComponentDto>(newComponent);
        }
        public async Task<bool> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto updateComponentStockDto, CancellationToken ct = default)
        {
            var existingComponent = await context.Components.FindAsync(id);
            if (existingComponent is null)
                return false;

            mapper.Map(updateComponentStockDto, existingComponent);
            await context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto updateComponentNameDto, CancellationToken ct = default)
        {
            var existingComponent = await context.Components.FindAsync(id);
            if (existingComponent is null)
                return false;
            mapper.Map(updateComponentNameDto, existingComponent);
            await context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteComponentAsync(Guid id, CancellationToken ct = default)
        {
            var deleteComponent = await context.Components
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(ct);

            return deleteComponent > 0;
        }
    }
}
