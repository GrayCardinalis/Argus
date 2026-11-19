using Argus.Dtos.Auditoriums;
using Argus.Dtos.Common;
using Argus.Dtos.Components;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IAuditoriumService
    {
        Task<PagedResultDto<AuditoriumDto>> GetAllAuditoriumsAsync(PaginationQueryDto dto, CancellationToken ct = default);
        Task<ErrorOr<AuditoriumDto>> GetAuditoriumByIdAsync(Guid id, CancellationToken ct = default);
        Task<ErrorOr<AuditoriumDto>> CreateAuditoriumAsync(CreateAuditoriumDto dto, CancellationToken ct = default);
        Task<ErrorOr<Success>> DeleteAuditoriumAsync(Guid id, CancellationToken ct = default);
    }
}
