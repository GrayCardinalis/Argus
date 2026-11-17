using Argus.Dtos.SupportRequest;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface ISupportRequestService
    {
        Task<ErrorOr<SupportRequestDetailDto>> CreateSupportRequestAsync(CreateSupportRequestDto dto, CancellationToken ct = default);
        Task<ErrorOr<SupportRequestDetailDto>> GetSupportRequestByIdAsync(Guid id, CancellationToken ct = default);
        //Task<ErrorOr<List<SupportRequestDetailDto>>> GetSupportRequestsByStatusAsync(SupportRequestStatus status, CancellationToken ct = default);
        Task<ErrorOr<Success>> AssignExecutorAsync(Guid supportRequestId, AssignExecutorDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> UpdateSupportRequestStatusAsync(Guid supportRequestId, UpdateSupportRequestStatusDto dto, CancellationToken ct = default);
    }
}
