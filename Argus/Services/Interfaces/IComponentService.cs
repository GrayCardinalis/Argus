using Argus.Dtos.Auditoriums;
using Argus.Dtos.Components;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IComponentService
    {
        Task<List<ComponentDto>> GetAllComponentAsync(CancellationToken ct = default);
        Task<ComponentDto?> GetComponentByIdAsync(Guid id, CancellationToken ct = default);
        Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto, CancellationToken ct = default);
        Task<bool> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto updateComponentNameDto, CancellationToken ct = default);
        Task<bool> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto updateComponentStockDto, CancellationToken ct = default);
        Task<bool> DeleteComponentAsync(Guid id, CancellationToken ct = default);
    }
}
