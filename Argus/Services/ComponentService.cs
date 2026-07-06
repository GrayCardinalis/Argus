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
        public async Task<List<ComponentDto>> GetAllComponentAsync()
        {
            return await context.Components
                .AsNoTracking()
                .ProjectTo<ComponentDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ComponentDto?> GetComponentByIdAsync(Guid id)
        {
            return await context.Components
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<ComponentDto?>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }



        public async Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto)
        {
            var newComponent = mapper.Map<Component>(createComponentDto);

            context.Components.Add(newComponent);

            await context.SaveChangesAsync();

            return mapper.Map<ComponentDto>(newComponent);
        }
        public async Task<bool> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto updateComponentStockDto)
        {
            var existingComponent = await context.Components.FindAsync(id);
            if (existingComponent is null)
                return false;

            mapper.Map(updateComponentStockDto, existingComponent);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto updateComponentNameDto)
        {
            var existingComponent = await context.Components.FindAsync(id);
            if (existingComponent is null)
                return false;
            mapper.Map(updateComponentNameDto, existingComponent);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteComponentAsync(Guid id)
        {
            var deleteComponent = await context.Components
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            return deleteComponent > 0;
        }
    }
}
