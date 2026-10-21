using Argus.Dtos.PlacementHistory;
using AutoMapper;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IPlacementHistoryService
    {
        Task<ErrorOr<Success>> MoveEquipmentAsync(Guid equipmentId, MoveEquipmentDto dto, CancellationToken ct = default);
        Task<ErrorOr<List<PlacementHistoryDto>>> GetEquipmentHistoryAsync(Guid equipmentId, CancellationToken ct = default);
    }
}