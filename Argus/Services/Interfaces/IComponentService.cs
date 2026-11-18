using Argus.Dtos.Auditoriums;
using Argus.Dtos.Common;
using Argus.Dtos.Components;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IComponentService
    {
        Task<ErrorOr<ComponentDto>> GetComponentByIdAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<ComponentDto>> CreateComponentAsync(CreateComponentDto createComponentDto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto updateComponentNameDto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateComponentStockAsync(Guid id, UpdateComponentStockDto updateComponentStockDto, CancellationToken ct = default);
        Task<ErrorOr<Success>> DeleteComponentAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<PagedResultDto<ComponentDto>>> GetAllComponentAsync(PaginationQueryDto dto, CancellationToken ct = default);
    }
}
