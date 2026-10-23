using Argus.Dtos.Equipment;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<ErrorOr<List<EquipmentListDto>>> GetAllEquipmentAsync(CancellationToken ct = default);
        Task<ErrorOr<EquipmentDetailDto>> GetEquipmentByIdAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<EquipmentDetailDto>> CreateEquipmentAsync(CreateEquipmentDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateEquipmentAsync(Guid id, UpdateEquipmentDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> DeleteEquipmentAsync(Guid id, CancellationToken ct = default);
    }
}
