using Argus.Dtos.Common;
using Argus.Dtos.PlacementHistory;
using AutoMapper;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IPlacementHistoryService
    {
        Task<ErrorOr<Success>> MoveEquipmentAsync(Guid equipmentId, MoveEquipmentDto dto, CancellationToken ct = default);
        Task<ErrorOr<PagedResultDto<PlacementHistoryDto>>> GetEquipmentHistoryAsync(Guid equipmentId, PaginationQueryDto dto, CancellationToken ct = default);
    }
}