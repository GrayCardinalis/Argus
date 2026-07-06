using Argus.Dtos.Auditoriums;
using Argus.Dtos.Components;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IComponentService
    {
        Task<List<ComponentDto>> GetAllComponentAsync();
        Task<ComponentDto?> GetComponentByIdAsync(Guid id);
        Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto);
        Task<bool> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto updateComponentNameDto);
        Task<bool> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto updateComponentStockDto);
        Task<bool> DeleteComponentAsync(Guid id);
    }
}
