using Argus.Dtos.Auditoriums;
using Argus.Dtos.Common;
using Argus.Dtos.Components;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IComponentService
    {
        Task<ComponentDto?> GetComponentByIdAsync(Guid id, CancellationToken ct = default);
        Task<ComponentDto> CreateComponentAsync(CreateComponentDto createComponentDto, CancellationToken ct = default);
        Task<bool> UpdateComponentNameAsync(Guid id, UpdateComponentNameDto updateComponentNameDto, CancellationToken ct = default);
        Task<ErrorOr<Success>> AdjustComponentStockAsync(Guid id, AdjustComponentStockDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> DeleteComponentAsync(Guid id, CancellationToken ct = default);
        Task<PagedResultDto<ComponentDto>> GetAllComponentAsync(PaginationQueryDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> ReceiveComponentStockAsync(Guid id, ReceiveComponentStockDto dto, CancellationToken ct = default);
    }
}
