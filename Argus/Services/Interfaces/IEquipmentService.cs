using Argus.Dtos.Common;
using Argus.Dtos.Equipment;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<ErrorOr<PagedResultDto<EquipmentListDto>>> GetAllEquipmentAsync(PaginationQueryDto dto, CancellationToken ct = default);
        Task<ErrorOr<EquipmentDetailDto>> GetEquipmentByIdAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<EquipmentDetailDto>> CreateEquipmentAsync(CreateEquipmentDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateEquipmentStatusAsync(Guid id, UpdateEquipmentStatusDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateEquipmentAsync(Guid id, UpdateEquipmentDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> DeleteEquipmentAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<PagedResultDto<EquipmentListDto>>> GetEquipmentByAuditoriumAsync(Guid auditoriumId,PaginationQueryDto dto,CancellationToken ct = default);
    }
}
