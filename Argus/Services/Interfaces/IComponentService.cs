using Argus.Dtos.Auditoriums;
using Argus.Dtos.Components;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IComponentService
    {
        Task<List<ComponentDto>> GetAllComponentAsync();
        Task<ComponentDto?> GetComponent(Guid id);
        Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto);
        Task<bool> UpdateComponentStockAsync(Guid id, [FromBody] UpdateComponentStockDto updateComponentStockDto);
        Task<bool> DeleteComponentAsync(Guid id);
    }
}
