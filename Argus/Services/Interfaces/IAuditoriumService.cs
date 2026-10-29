using Argus.Dtos.Auditoriums;
using Argus.Dtos.Components;
using Microsoft.AspNetCore.Mvc;

namespace Argus.Services.Interfaces
{
    public interface IAuditoriumService
    {
        Task<List<AuditoriumDto>> GetAllAuditoriumsAsync(CancellationToken ct = default);
        Task<AuditoriumDto?> GetAuditoriumByIdAsync(Guid id, CancellationToken ct =default); //? because if the Auditorium with the requested id is not found in the database.
        Task<AuditoriumDto> CreateAuditoriumAsync(CreateAuditoriumDto createAuditoriumDto, CancellationToken ct = default);
        Task<bool> DeleteAuditoriumAsync(Guid id, CancellationToken ct = default);
    }
}
