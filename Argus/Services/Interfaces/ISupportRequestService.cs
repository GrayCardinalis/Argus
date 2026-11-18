using Argus.Dtos.Common;
using Argus.Dtos.SupportRequest;
using Argus.Dtos.SupportRequest.Comments;
using Argus.Dtos.SupportRequest.Components;
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
        Task<ErrorOr<PagedResultDto<SupportRequestListDto>>> GetSupportRequestsAsync(SupportRequestQueryDto dto,CancellationToken ct = default);
        Task<ErrorOr<SupportRequestCommentDto>> AddCommentAsync(Guid supportRequestId,CreateSupportRequestCommentDto dto,CancellationToken ct = default);
        Task<ErrorOr<PagedResultDto<SupportRequestCommentDto>>> GetCommentsAsync(Guid supportRequestId,PaginationQueryDto dto,CancellationToken ct = default);
        Task<ErrorOr<Success>> AddComponentAsync(Guid supportRequestId, AddSupportRequestComponentDto dto, CancellationToken ct = default);
        Task<ErrorOr<PagedResultDto<SupportRequestComponentDto>>> GetComponentsAsync(Guid supportRequestId, PaginationQueryDto dto, CancellationToken ct = default);
    }
}