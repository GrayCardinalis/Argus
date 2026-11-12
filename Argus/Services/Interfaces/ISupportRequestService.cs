using Argus.Dtos.SupportRequest;
using ErrorOr;

namespace Argus.Services.Interfaces
{
    public interface ISupportRequestService
    {
        Task<ErrorOr<SupportRequestDetailDto>> CreateSupportRequestAsync(CreateSupportRequestDto dto, CancellationToken ct = default);
        Task<ErrorOr<SupportRequestDetailDto>> GetSupportRequestByIdAsync(Guid id, CancellationToken ct = default);
    }
}
